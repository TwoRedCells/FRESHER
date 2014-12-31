using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Directory.SetCurrentDirectory(Settings.ExperimentDirectory);
            Facilitator.Initialize();
            Facilitator.UI = new ExperimentPage();

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                Facilitator.RunExperiment(args[1]);
            }
        }


    }
}
