using System;
using System.Collections.Generic;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Interface ILoggable
    /// </summary>
    public interface ILoggable
    {
        /// <summary>
        /// Occurs when data is available.
        /// </summary>
        event EventHandler<DataEventArgs> DataAvailable;
    }
}
