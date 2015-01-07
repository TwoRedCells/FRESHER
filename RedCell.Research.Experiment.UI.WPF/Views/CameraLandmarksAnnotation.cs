using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace RedCell.Research.Experiment.UI
{
    public class CameraLandmarksAnnotation : Canvas, IControl
    {

        public CameraLandmarksAnnotation(ICameraView view)
        {
            CameraView = view;
            view.Camera.LandmarksFound += Camera_LandmarksFound;
        }

        public ICameraView CameraView { get; set; }

        void Camera_LandmarksFound(object sender, LandmarksEventArgs e)
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
