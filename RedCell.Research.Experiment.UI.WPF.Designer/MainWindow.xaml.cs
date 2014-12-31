using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace RedCell.Research.Experiment.UI.WPF.Designer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string RegistryPathKey = @"Software\Red Cell Innovation Inc.\RedCell.Research.Experiment\LastPath";
        private string _experimentDirectory;
        private FileSystemWatcher _watcher;

        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _watcher = new FileSystemWatcher();
            _watcher.Renamed += Watcher_Renamed;
            _watcher.Changed += Watcher_Changed;
            _watcher.Created += Watcher_Changed;
            _watcher.Deleted += Watcher_Changed;
            _watcher.IncludeSubdirectories = false;
            ExperimentDirectory = LoadPath() ?? Directory.GetCurrentDirectory();
            _watcher.EnableRaisingEvents = true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the experiment directory.
        /// </summary>
        /// <value>The experiment directory.</value>
        public string ExperimentDirectory
        {
            get { return _experimentDirectory; }
            set
            {
                _experimentDirectory = value;
                _watcher.Path = value;
                ExperimentFolder.Text = value;
                EnumerateExperiments();
            }
        }

        /// <summary>
        /// Gets the selected experiment.
        /// </summary>
        /// <value>The selected experiment.</value>
        public string SelectedExperiment
        {
            get { return ExperimentListBox.SelectedItem as string; }
        }

        /// <summary>
        /// Gets or sets the experiment.
        /// </summary>
        /// <value>The experiment.</value>
        public Experiment Experiment { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the path.
        /// </summary>
        private string LoadPath()
        {
            return Registry.CurrentUser.GetValue(RegistryPathKey) as string;
        }

        /// <summary>
        /// Sets the path.
        /// </summary>
        private void SetPath(string path)
        {
            Registry.CurrentUser.SetValue(RegistryPathKey, path);
        }

        /// <summary>
        /// Handles the Changed event of the Watcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="fileSystemEventArgs">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void Watcher_Changed(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            Dispatcher.Invoke(EnumerateExperiments);
        }

        /// <summary>
        /// Handles the Renamed event of the Watcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="renamedEventArgs">The <see cref="RenamedEventArgs"/> instance containing the event data.</param>
        private void Watcher_Renamed(object sender, RenamedEventArgs renamedEventArgs)
        {
            Dispatcher.Invoke(EnumerateExperiments);
        }

        /// <summary>
        /// Handles the OnClick event of the Browse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            var picker = new VistaFolderBrowserDialog {SelectedPath = ExperimentDirectory, ShowNewFolderButton = true};
            if (picker.ShowDialog().Value)
            {
                ExperimentDirectory = picker.SelectedPath;
                Directory.SetCurrentDirectory(ExperimentDirectory);
                SetPath(ExperimentDirectory);
            }
        }

        /// <summary>
        /// Enumerates the experiments.
        /// </summary>
        private void EnumerateExperiments()
        {
            var root = new DirectoryInfo(ExperimentDirectory);
            var subs = root.GetDirectories();
            var experiments = (from sub in subs where IsExperiment(sub) select sub.Name);
            ExperimentListBox.ItemsSource = experiments.ToList();
        }

        /// <summary>
        /// Determines whether the specified directory is experiment.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns><c>true</c> if the specified directory is experiment; otherwise, <c>false</c>.</returns>
        private bool IsExperiment(DirectoryInfo directory)
        {
            return directory.GetFiles(Experiment.ExperimentFilename).Length == 1;
        }

        /// <summary>
        /// Handles the OnSelected event of the ExperimentListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ExperimentListBox_OnSelected(object sender, RoutedEventArgs e)
        {
            OpenExperiment(ExperimentListBox.SelectedItem as string);
        }

        /// <summary>
        /// Opens the experiment.
        /// </summary>
        /// <param name="name">The name.</param>
        private void OpenExperiment(string name)
        {
            var path = Path.Combine(ExperimentDirectory, name);
            Experiment.Load(path);
            FileSystemTree.RootPath = path;
        }

        /// <summary>
        /// Handles the OnClick event of the CreateNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CreateNew_OnClick(object sender, RoutedEventArgs e)
        {
            CreateExperiment(ExperimentName.Text);
            ExperimentName.Text = "";
        }

        /// <summary>
        /// Creates the experiment.
        /// </summary>
        /// <param name="name">The name.</param>
        private void CreateExperiment(string name)
        {
            var experiment = new Experiment(name);
            experiment.Save(ExperimentDirectory);
        }

        /// <summary>
        /// Handles the OnClosing event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _watcher.Renamed -= Watcher_Renamed;
            _watcher.Changed -= Watcher_Changed;
            _watcher.Created -= Watcher_Changed;
            _watcher.Deleted -= Watcher_Changed;
            _watcher.Dispose();
            _watcher = null;
        }
        #endregion

        private void FileSystemTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var a = FileSystemTree.SelectedPath;
            if(a != null)
                Edit(a);
        }

        public void Edit(string path)
        {
            if (Regex.IsMatch(path, @"\.(py|js|cs|txt|html|xml|csv|tab)$") && File.Exists(path))
                Editor.Text = File.ReadAllText(path);
        }

        private void Experiment_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Experiment.Save();
        }
    }
}
