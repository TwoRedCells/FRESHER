using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCell.Research.Experiment
{
    public interface ILoggable
    {
        event EventHandler<DataEventArgs> DataAvailable;
    }
}
