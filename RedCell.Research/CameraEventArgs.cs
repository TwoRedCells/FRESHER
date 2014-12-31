using System;
using System.Collections.Generic;

namespace RedCell.Research.Experiment
{
    public class CameraEventArgs : EventArgs
    {
        public CameraEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; private set; }

        public int Height { get; private set; }
    }
}
