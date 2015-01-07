using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            Directory.SetCurrentDirectory(Settings.ExperimentDirectory);
            Facilitator.Initialize();

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                var ns = NavigationService.GetNavigationService(this);
                ns.Navigate(Facilitator.UI, args[1]);
            }
            else
            {
                ExperimentListBox.ItemsSource = Settings.EnumerateExperiments();
            }
        }

        private void ExperimentListBox_Selected(object sender, RoutedEventArgs e)
        {
            string selection = (sender as ListBox).SelectedItem as string;
            this.NavigationService.Navigate(new ExperimentPage(selection), selection);
        }    
    }
}
