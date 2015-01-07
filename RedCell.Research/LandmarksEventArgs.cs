using System;
using System.Collections.Generic;

namespace RedCell.Research.Experiment
{
    public class LandmarksEventArgs : CameraEventArgs
    {
        public LandmarksEventArgs(Dictionary<string, Point> landmarks, int width, int height)
            : base(width, height)
        {
        }

        public Dictionary<string, Point> Landmarks { get; private set; }
    }
}
