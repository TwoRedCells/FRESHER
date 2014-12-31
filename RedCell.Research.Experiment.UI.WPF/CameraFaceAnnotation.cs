﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace RedCell.Research.Experiment.UI
{
    public class CameraFaceAnnotation : Canvas, IControl
    {
        readonly System.Windows.Shapes.Rectangle _rectangle;

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

        public ICameraView CameraView { get; set; }

        void Camera_FaceFound(object sender, FaceEventArgs e)
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

    }
}
