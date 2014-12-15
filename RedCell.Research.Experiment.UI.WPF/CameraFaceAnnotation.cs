using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace RedCell.Research.Experiment.UI
{
    public class CameraFaceAnnotation : Canvas
    {
        System.Windows.Shapes.Rectangle _rectangle;

        public CameraFaceAnnotation(ICameraView view)
        {
            CameraView = view;

            this.Margin = new Thickness(view.X, view.Y, 0, 0);
            this.Width = view.Width;
            this.Height = view.Height;

            _rectangle = new System.Windows.Shapes.Rectangle 
            {
                Stroke = new SolidColorBrush(Colors.Yellow),
                StrokeThickness = 1
            };
            this.Children.Add(_rectangle);

            view.Camera.FaceFound += Camera_FaceFound;
        }

        public ICameraView CameraView { get; set; }

        void Camera_FaceFound(object sender, FaceEventArgs e)
        {
            Dispatcher.Invoke(() =>
                {
                    var bounds = e.Bounds;
                    _rectangle.Margin = new Thickness(bounds.X, bounds.Y, 0, 0);
                    _rectangle.Width = bounds.Width;
                    _rectangle.Height = bounds.Height;
                });
        }

    }
}
