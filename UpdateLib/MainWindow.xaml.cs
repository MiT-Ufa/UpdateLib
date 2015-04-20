using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Timers;
using Microsoft.Win32;
using UpdateLib.Resources;

namespace UpdateLib
{
    internal partial class MainWindow
    {
        private Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public MainWindow(bool remindLater = false)
        {
            if (!remindLater)
            {
                InitializeComponent();
                var resources = new ComponentResourceManager(typeof (Translations));
                Title = AutoUpdater.DialogTitle;
                UpdateTextBlock.Text =
                    string.Format(
                        resources.GetString(
                            Translations.A_new_version_is_available,
                            CultureInfo.CurrentCulture),
                        AutoUpdater.AppTitle);
                DescriptionTextBlock.Text =
                    string.Format(
                        resources.GetString(
                            Translations.Update_is_now_available,
                            CultureInfo.CurrentCulture),
                        AutoUpdater.AppTitle,
                        AutoUpdater.CurrentVersion,
                        AutoUpdater.InstalledVersion);
            }
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            WebBrowser.Navigate(AutoUpdater.ChangeLogUrl);
        }

        private void ButtonUpdateClick(object sender, EventArgs e)
        {
            if (AutoUpdater.OpenDownloadPage)
            {
                var processStartInfo = new ProcessStartInfo(AutoUpdater.DownloadUrl);
                Process.Start(processStartInfo);
            }
            else
            {
                AutoUpdater.DownloadUpdate(this);
            }
        }

        private void ButtonSkipClick(object sender, EventArgs e)
        {
            try
            {
                var registryLocation = AutoUpdater.RegistryLocation;
                if (registryLocation == null)
                {
                    return;
                }

                var updateKey = Registry.CurrentUser.CreateSubKey(registryLocation);
                if (updateKey != null)
                {
                    updateKey.SetValue("version", AutoUpdater.CurrentVersion.ToString());
                    updateKey.SetValue("skip", 1);
                    updateKey.Close();
                }
            }
            finally
            {
                Close();
            }
        }

        private void ButtonRemindLaterClick(object sender, EventArgs e)
        {
            if (AutoUpdater.LetUserSelectRemindLater)
            {
                var remindLaterForm = new RemindLaterWindow
                {
                    Owner = this
                };
                remindLaterForm.ShowDialog();

                var dialogResult = remindLaterForm.Result;

                if (dialogResult == RemindLaterWindow.RemindDialogResult.RemindLater)
                {
                    AutoUpdater.RemindLaterTimeSpan = remindLaterForm.RemindLaterFormat;
                    AutoUpdater.RemindLaterAt = remindLaterForm.RemindLaterAt;
                }
                else if (dialogResult == RemindLaterWindow.RemindDialogResult.DownloadNow)
                {
                    AutoUpdater.DownloadUpdate();
                    return;
                }
                else
                {
                    return;
                }
            }

            var registryLocation = AutoUpdater.RegistryLocation;
            if (registryLocation == null)
            {
                return;
            }

            var updateKey = Registry.CurrentUser.CreateSubKey(registryLocation);
            if (updateKey != null)
            {
                updateKey.SetValue("version", AutoUpdater.CurrentVersion);
                updateKey.SetValue("skip", 0);
                var remindLaterDateTime = DateTime.Now;
                switch (AutoUpdater.RemindLaterTimeSpan)
                {
                    case RemindLaterFormat.Days:
                        remindLaterDateTime = DateTime.Now +
                                              TimeSpan.FromDays(AutoUpdater.RemindLaterAt);
                        break;
                    case RemindLaterFormat.Hours:
                        remindLaterDateTime = DateTime.Now +
                                              TimeSpan.FromHours(AutoUpdater.RemindLaterAt);
                        break;
                    case RemindLaterFormat.Minutes:
                        remindLaterDateTime = DateTime.Now +
                                              TimeSpan.FromMinutes(AutoUpdater.RemindLaterAt);
                        break;
                }

                updateKey.SetValue(
                    "remindlater",
                    remindLaterDateTime.ToString(CultureInfo.CreateSpecificCulture("en-US")));
                SetTimer(remindLaterDateTime);
                updateKey.Close();
            }
        }

        public void SetTimer(DateTime remindLater)
        {
            var timeSpan = remindLater - DateTime.Now;
            _timer = new Timer
            {
                Interval = (int) timeSpan.TotalMilliseconds
            };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            AutoUpdater.Start();
        }
    }
}