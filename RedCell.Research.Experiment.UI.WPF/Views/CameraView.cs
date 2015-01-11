using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Class CameraView.
    /// </summary>
    public class CameraView : Image, ICameraView
    {
        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraView"/> class.
        /// </summary>
        public CameraView(ICamera camera, CameraViews view)
        {
            if (camera == null)
                throw new ArgumentNullException("camera");

            if (view == CameraViews.Unknown)
                throw new ArgumentOutOfRangeException("view");
            
            this.Stretch = Stretch.Uniform;

            View = view;
            Camera = camera;
            camera.FrameAvailable += Camera_FrameAvailable;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the camera.
        /// </summary>
        /// <value>The camera.</value>
        public ICamera Camera { get; private set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public CameraViews View { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Handles the FrameAvailable event of the Camera control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CameraFrameEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        void Camera_FrameAvailable(object sender, CameraFrameEventArgs e)
        {
            var frameinfo = e.Value;
            if (frameinfo.View != View)
                return;

            PXCMImage.PixelFormat format;

            Dispatcher.Invoke(() =>
            {
                switch(View)
                {
                    //case CameraViews.Colour: format = PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32; break;
                    //case CameraViews.Depth: format = PXCMImage.PixelFormat.PIXEL_FORMAT_DEPTH; break;
                    //case CameraViews.Infrared: format = PXCMImage.PixelFormat.PIXEL_FORMAT_Y8; break;
                    default: format = PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32; break;
                }

                PXCMImage.ImageData data;
                var image = frameinfo.SourceFrame as PXCMImage;
                image.AcquireAccess(PXCMImage.Access.ACCESS_READ, format, out data);
                Source = data.ToWritableBitmap(image.info.width, image.info.height, 72, 72);
                image.ReleaseAccess(data);
            });
        }
        #endregion
    }
}
