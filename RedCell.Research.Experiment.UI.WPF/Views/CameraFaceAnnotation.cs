using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Class CameraFaceAnnotation.
    /// </summary>
    public class CameraFaceAnnotation : Canvas, IControl
    {
        /// <summary>
        /// The _rectangle
        /// </summary>
        readonly System.Windows.Shapes.Rectangle _rectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraFaceAnnotation"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public CameraFaceAnnotation(ICameraView view)
        {
            CameraView = view;

            _rectangle = new System.Windows.Shapes.Rectangle 
            {
                Stroke = new SolidColorBrush(Colors.Yellow),
                StrokeThickness = 1
            };
            this.Children.Add(_rectangle);

            view.Camera.FaceFound += Camera_FaceFound;
        }

        /// <summary>
        /// Gets or sets the camera view.
        /// </summary>
        /// <value>The camera view.</value>
        public ICameraView CameraView { get; set; }

        /// <summary>
        /// Handles the FaceFound event of the Camera control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FaceEventArgs"/> instance containing the event data.</param>
        void Camera_FaceFound(object sender, FaceEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                    {
                        double scaleX = this.ActualWidth / e.Width;
                        double scaleY = this.ActualHeight / e.Height;
                        var bounds = e.Bounds;
                        _rectangle.Margin = new Thickness(bounds.X * scaleX, bounds.Y * scaleY, 0, 0);
                        _rectangle.Width = bounds.Width * scaleX;
                        _rectangle.Height = bounds.Height * scaleY;
                    });
            }
            catch (TaskCanceledException)
            {
                // Do nothing.
            }
        }

    }
}
