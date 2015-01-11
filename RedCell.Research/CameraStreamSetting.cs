using System;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Camera Stream Setting.
    /// </summary>
    public class CameraStreamSetting
    {
        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraStreamSetting"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="framerate">The framerate.</param>
        public CameraStreamSetting(int width, int height, double framerate)
        {
            Width = width;
            Height = height;
            Framerate = framerate;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the framerate.
        /// </summary>
        /// <value>The framerate.</value>
        public double Framerate { get; private set; }
        #endregion
    }
}
