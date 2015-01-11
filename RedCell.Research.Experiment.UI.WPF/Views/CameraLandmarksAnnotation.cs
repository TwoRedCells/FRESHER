using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Camera Landmarks Annotation
    /// </summary>
    public class CameraLandmarksAnnotation : Canvas, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraLandmarksAnnotation"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public CameraLandmarksAnnotation(ICameraView view)
        {
            CameraView = view;
            view.Camera.LandmarksFound += Camera_LandmarksFound;
        }

        /// <summary>
        /// Gets or sets the camera view.
        /// </summary>
        /// <value>The camera view.</value>
        public ICameraView CameraView { get; set; }

        /// <summary>
        /// Handles the LandmarksFound event of the Camera control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LandmarksEventArgs"/> instance containing the event data.</param>
        private void Camera_LandmarksFound(object sender, LandmarksEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                    {
                        Children.Clear();

                        foreach (var landmark in e.Landmarks)
                        {
                            double scaleX = this.ActualWidth / e.Width;
                            double scaleY = this.ActualHeight / e.Height;
                            var rect = new System.Windows.Shapes.Rectangle
                            {
                                Stroke = new SolidColorBrush(Colors.Yellow),
                                StrokeThickness = 1,
                                Margin = new Thickness((landmark.Value.X - 1) * scaleX, (landmark.Value.Y - 1) * scaleY, 0, 0),
                                Width = 2 * scaleX,
                                Height = 2 * scaleY
                            };
                            Children.Add(rect);
                        }
                    });
            }
            catch (TaskCanceledException)
            {
                // Do nothing.
            }
        }
    }
}
