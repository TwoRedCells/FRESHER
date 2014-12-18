using System;
using System.Collections.Generic;

namespace RedCell.Research.Experiment
{
    public class DataEventArgs : EventArgs
    {
        public DataEventArgs(Dictionary<string, double> values)
        {
            Values = values;
        }

        public Dictionary<string, double> Values { get; private set; }

    }
}
