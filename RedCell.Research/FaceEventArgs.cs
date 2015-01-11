using System;
using System.Collections.Generic;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class FaceEventArgs.
    /// </summary>
    public class FaceEventArgs : CameraEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceEventArgs"/> class.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="expressions">The expressions.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public FaceEventArgs(IRectangle bounds, Dictionary<string, double> expressions, int width, int height) : base(width, height)
        {
            Bounds = bounds;
            Expressions = expressions;
        }

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <value>The bounds.</value>
        public IRectangle Bounds { get; private set; }

        /// <summary>
        /// Gets the expressions.
        /// </summary>
        /// <value>The expressions.</value>
        public Dictionary<string, double> Expressions { get; private set; }

    }
}
