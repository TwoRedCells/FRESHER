﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedCell.Research.Experiment
{
    public struct Point3d
    {
        public Point3d(double x, double y, double z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double Z { get; private set; }
    }
}
