﻿using System;
using System.Collections.Generic;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class CameraEventArgs.
    /// </summary>
    public class CameraEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraEventArgs"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public CameraEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }

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
    }
}
