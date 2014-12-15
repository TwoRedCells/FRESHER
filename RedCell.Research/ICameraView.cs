using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCell.Research.Experiment
{
    public interface ICameraView : IControl, IRectangle
    {
        ICamera Camera { get; }
    }
}
