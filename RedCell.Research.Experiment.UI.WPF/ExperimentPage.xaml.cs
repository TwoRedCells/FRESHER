using System.Windows.Controls;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Interaction logic for ExperimentPage.xaml
    /// </summary>
    public partial class ExperimentPage : Page, IExperimentUI
    {
        private Experiment _experiment;

        public ExperimentPage(string name)
        {
            InitializeComponent();
            Facilitator.UI = this;
            _experiment = Experiment.Load(name);
            Facilitator.RunExperiment(_experiment);
        }

        public void MessageBox(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        public IRegion AddRegion(float x, float y, float w, float h)
        {
            var r = new Region(x, y, w, h);
            Canvas.Children.Add(r);
            return r;
        }

        public void AddCameraFaceAnnotation(ICameraView view)
        {
            var c = new CameraFaceAnnotation(view);
            Canvas.Children.Add(c);
        }

        public void Wait(double seconds)
        {
            UISleepHelper.Wait(seconds);
        }
    }
}
