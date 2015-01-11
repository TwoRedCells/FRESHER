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
        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Settings.ExperimentDirectoryChanged += Watcher_Changed;
            ExperimentFolder.Text = Settings.ExperimentDirectory;
            EnumerateExperiments();
        }
        #endregion

        #region Properties
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
        /// Handles the Changed event of the Watcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
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
            var picker = new VistaFolderBrowserDialog {SelectedPath = Settings.ExperimentDirectory, ShowNewFolderButton = true};
            if (picker.ShowDialog().Value)
            {
                ExperimentFolder.Text = picker.SelectedPath;
                Settings.ExperimentDirectory = picker.SelectedPath;
                Directory.SetCurrentDirectory(Settings.ExperimentDirectory);
                Settings.SetPath(Settings.ExperimentDirectory);
            }
        }

        /// <summary>
        /// Enumerates the experiments.
        /// </summary>
        private void EnumerateExperiments()
        {
            ExperimentListBox.ItemsSource = Settings.EnumerateExperiments();
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
            var path = Path.Combine(Settings.ExperimentDirectory, name);
            Experiment.Load(path);
            FileSystemTree.RootPath = path;
            //MetaResearcher.Text = Experiment.Researcher;
            //MetaName.Text = Experiment.Name;
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
            experiment.Save(Settings.ExperimentDirectory);
        }

        /// <summary>
        /// Handles the OnClosing event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Settings.Dispose();
        }

        /// <summary>
        /// Files the system tree_ on selected item changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void FileSystemTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var a = FileSystemTree.SelectedPath;
            if(a != null)
                Edit(a);
        }

        /// <summary>
        /// Edits the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Edit(string path)
        {
            if (Regex.IsMatch(path, @"\.(py|js|cs|txt|html|xml|csv|tab)$") && File.Exists(path))
                Editor.Text = File.ReadAllText(path);
        }

        /// <summary>
        /// Handles the OnTextChanged event of the Experiment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void Experiment_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Experiment.Save();
        }
        #endregion
    }
}
