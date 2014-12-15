using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RedCell.Research.Experiment;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IExperimentUI
    {
        public MainWindow()
        {
            InitializeComponent();

            Facilitator.Initialize();
            Facilitator.UI = this;
            Experiment.Facilitator.RunExperiment("test.py");
        }

        public void MessageBox(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        public ICameraView AddCameraView(float x, float y, float w, float h, ICamera camera, CameraViews view)
        {
            var cv = new CameraView(x, y, w, h, camera, view);
            Canvas.Children.Add(cv);
            return cv;
        }

        public void AddCameraFaceAnnotation(ICameraView view)
        {
            var c = new CameraFaceAnnotation(view);
            Canvas.Children.Add(c);
        }
    }
}
