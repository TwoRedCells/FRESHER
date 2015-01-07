using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;


namespace RedCell.Research.Experiment.UI
{
    public static class Settings
    {
        private const string RegistryPathKey = @"Software\Red Cell Innovation Inc.\RedCell.Research.Experiment\LastPath";
        private static string _experimentDirectory;
        private static FileSystemWatcher _watcher;

        #region Initialization
        static Settings()
        {
            _watcher = new FileSystemWatcher();
            _watcher.Renamed += Watcher_Changed;
            _watcher.Changed += Watcher_Changed;
            _watcher.Created += Watcher_Changed;
            _watcher.Deleted += Watcher_Changed;
            _watcher.IncludeSubdirectories = false;
            ExperimentDirectory = LoadPath() ?? Directory.GetCurrentDirectory();
            _watcher.EnableRaisingEvents = true;

            ScreenWidth = 1920;
            ScreenHeight = 1080;
            CoordinateWidth = 1920;
            CoordinateHeight = 1080;
        }

        static void Watcher_Changed(object sender, EventArgs e)
        {
            OnExperimentDirectoryChanged(sender, e as FileSystemEventArgs);
        }

        public static void Dispose()
        {
            _watcher.Renamed -= Watcher_Changed;
            _watcher.Changed -= Watcher_Changed;
            _watcher.Created -= Watcher_Changed;
            _watcher.Deleted -= Watcher_Changed;
            _watcher.Dispose();
            _watcher = null;

        }
        #endregion

        #region Events
        public static event EventHandler<FileSystemEventArgs> ExperimentDirectoryChanged;

        private static void OnExperimentDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            if (ExperimentDirectoryChanged != null)
                ExperimentDirectoryChanged(sender, e);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the experiment directory.
        /// </summary>
        /// <value>The experiment directory.</value>
        public static string ExperimentDirectory
        {
            get { return _experimentDirectory; }
            set
            {
                _experimentDirectory = value;
                _watcher.Path = value;
                if (ExperimentDirectoryChanged != null)
                    ExperimentDirectoryChanged(null, new FileSystemEventArgs(WatcherChangeTypes.All, value, value));
            }
        }

        public static double ScreenWidth { get; set; }

        public static double ScreenHeight { get; set; }

        public static double CoordinateWidth { get; set; }

        public static double CoordinateHeight { get; set; }

        public static double ScaleX { get { return ScreenWidth / CoordinateWidth; } }

        public static double ScaleY { get { return ScreenHeight / CoordinateHeight; } }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the path.
        /// </summary>
        public static string LoadPath()
        {
            return Registry.CurrentUser.GetValue(RegistryPathKey) as string;
        }

        /// <summary>
        /// Sets the path.
        /// </summary>
        public static void SetPath(string path)
        {
            Registry.CurrentUser.SetValue(RegistryPathKey, path);
        }

        /// <summary>
        /// Enumerates the experiments.
        /// </summary>
        public static string[] EnumerateExperiments()
        {
            var root = new DirectoryInfo(Settings.ExperimentDirectory);
            var subs = root.GetDirectories();
            return (from sub in subs where IsExperiment(sub) select sub.Name).ToArray();
        }

        /// <summary>
        /// Determines whether the specified directory is experiment.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns><c>true</c> if the specified directory is experiment; otherwise, <c>false</c>.</returns>
        private static bool IsExperiment(DirectoryInfo directory)
        {
            return directory.GetFiles(Experiment.ExperimentFilename).Length == 1;
        }


        #endregion
    }
}
