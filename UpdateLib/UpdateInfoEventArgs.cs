using System;

namespace UpdateLib
{
    /// <summary>
    ///     Object of this class gives you all the details about the update useful in handling the update logic yourself.
    /// </summary>
    public class UpdateInfoEventArgs : EventArgs
    {
        /// <summary>
        ///     If new update is available then returns true otherwise false.
        /// </summary>
        public bool IsUpdateAvailable { get; set; }

        /// <summary>
        ///     Download URL of the update file.
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        ///     URL of the webpage specifying changes in the new update.
        /// </summary>
        public string ChangelogUrl { get; set; }

        /// <summary>
        ///     Returns newest version of the application available to download.
        /// </summary>
        public Version CurrentVersion { get; set; }

        /// <summary>
        ///     Returns version of the application currently installed on the user's PC.
        /// </summary>
        public Version InstalledVersion { get; set; }
    }
}