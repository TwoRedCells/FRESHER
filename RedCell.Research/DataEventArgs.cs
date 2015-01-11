using System;
using System.Collections.Generic;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class DataEventArgs.
    /// </summary>
    public class DataEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataEventArgs"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public DataEventArgs(Dictionary<string, double> values)
        {
            Values = values;
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public Dictionary<string, double> Values { get; private set; }

    }
}
