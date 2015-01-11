using System;
using System.Collections.Generic;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class LandmarksEventArgs.
    /// </summary>
    public class LandmarksEventArgs : CameraEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LandmarksEventArgs"/> class.
        /// </summary>
        /// <param name="landmarks">The landmarks.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public LandmarksEventArgs(Dictionary<string, Point> landmarks, int width, int height)
            : base(width, height)
        {
            Landmarks = landmarks;
        }

        /// <summary>
        /// Gets the landmarks.
        /// </summary>
        /// <value>The landmarks.</value>
        public Dictionary<string, Point> Landmarks { get; private set; }
    }
}
