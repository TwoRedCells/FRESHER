using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.NavigationService.Navigate(new HomePage());
            Settings.ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            Settings.ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
        }
    }
}
