using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedCell.Research.Experiment
{
    public struct Point
    {
        public Point(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public double X { get; private set; }

        public double Y { get; private set; }
    }
}
