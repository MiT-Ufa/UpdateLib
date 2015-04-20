using System;
using UpdateLib.Resources;

namespace UpdateLib
{
    internal partial class RemindLaterWindow
    {
        public enum RemindDialogResult
        {
            None,
            RemindLater,
            DownloadNow
        }

        private static readonly string[] ComboboxItems =
        {
            Translations.After_30_minutes,
            Translations.After_12_hours,
            Translations.After_1_day,
            Translations.After_2_days,
            Translations.After_4_days,
            Translations.After_8_days,
            Translations.After_10_days
        };

        public RemindLaterWindow()
        {
            InitializeComponent();
            Result = RemindDialogResult.None;
            RemindComboBox.ItemsSource = ComboboxItems;
        }

        public RemindLaterFormat RemindLaterFormat { get; private set; }
        public int RemindLaterAt { get; private set; }
        public RemindDialogResult Result { get; private set; }

        private void OnLoaded(object sender, EventArgs e)
        {
            RemindComboBox.SelectedIndex = 0;
            RadioButtonYes.IsChecked = true;
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            if (RadioButtonYes.IsChecked == true)
            {
                switch (RemindComboBox.SelectedIndex)
                {
                    case 0:
                        RemindLaterFormat = RemindLaterFormat.Minutes;
                        RemindLaterAt = 30;
                        break;
                    case 1:
                        RemindLaterFormat = RemindLaterFormat.Hours;
                        RemindLaterAt = 12;
                        break;
                    case 2:
                        RemindLaterFormat = RemindLaterFormat.Days;
                        RemindLaterAt = 1;
                        break;
                    case 3:
                        RemindLaterFormat = RemindLaterFormat.Days;
                        RemindLaterAt = 2;
                        break;
                    case 4:
                        RemindLaterFormat = RemindLaterFormat.Days;
                        RemindLaterAt = 4;
                        break;
                    case 5:
                        RemindLaterFormat = RemindLaterFormat.Days;
                        RemindLaterAt = 8;
                        break;
                    case 6:
                        RemindLaterFormat = RemindLaterFormat.Days;
                        RemindLaterAt = 10;
                        break;
                }
                Result = RemindDialogResult.RemindLater;
                DialogResult = true;
            }
            else
            {
                Result = RemindDialogResult.DownloadNow;
                DialogResult = false;
            }
        }

        private void RadioButtonYesChecked(object sender, EventArgs e)
        {
            if (RemindComboBox != null)
            {
                RemindComboBox.IsEnabled = true;
            }
        }

        private void RadioButtonYesUnchecked(object sender, EventArgs e)
        {
            if (RemindComboBox != null)
            {
                RemindComboBox.IsEnabled = false;
            }
        }
    }
}