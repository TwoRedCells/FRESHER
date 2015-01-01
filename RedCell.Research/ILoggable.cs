using System;
using System.Collections.Generic;
namespace RedCell.Research.Experiment
{
    public interface ILoggable
    {
        event EventHandler<DataEventArgs> DataAvailable;
    }
}
