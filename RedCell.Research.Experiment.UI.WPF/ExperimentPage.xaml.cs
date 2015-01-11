using System.Windows.Controls;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Interaction logic for ExperimentPage.xaml
    /// </summary>
    public partial class ExperimentPage : Page, IExperimentUI
    {
        private Experiment _experiment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentPage"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ExperimentPage(string name)
        {
            InitializeComponent();
            Facilitator.UI = this;
            _experiment = Experiment.Load(name);
            Facilitator.RunExperiment(_experiment);
        }

        /// <summary>
        /// Messages the box.
        /// </summary>
        /// <param name="message">The message.</param>
        public void MessageBox(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        /// <summary>
        /// Adds a region.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <returns>IRegion.</returns>
        public IRegion AddRegion(float x, float y, float w, float h)
        {
            var r = new Region(x, y, w, h);
            Canvas.Children.Add(r);
            return r;
        }

        /// <summary>
        /// Adds the camera face annotation.
        /// </summary>
        /// <param name="view">The view.</param>
        public void AddCameraFaceAnnotation(ICameraView view)
        {
            var c = new CameraFaceAnnotation(view);
            Canvas.Children.Add(c);
        }

        /// <summary>
        /// Waits the specified seconds.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        public void Wait(double seconds)
        {
            UISleepHelper.Wait(seconds);
        }
    }
}
