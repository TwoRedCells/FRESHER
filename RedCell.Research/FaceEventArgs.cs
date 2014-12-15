using System;
using System.Collections.Generic;

namespace RedCell.Research.Experiment
{
    public class FaceEventArgs : EventArgs
    {
        public FaceEventArgs(IRectangle bounds, Dictionary<string, double> expressions)
        {
            Bounds = bounds;
            Expressions = expressions;
        }

        public IRectangle Bounds { get; private set; }

        public Dictionary<string, double> Expressions { get; private set; }

    }
}
