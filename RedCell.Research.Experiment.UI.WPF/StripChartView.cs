using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RedCell.UI.WPF.Charts;

namespace RedCell.Research.Experiment.UI
{
    public class StripChartView : TimeGraph, IControl
    {
        Log _log;

        public StripChartView(Log log)
        {
            _log = log;
            this.Data = log.DataSet;
            ValuesPerUnit = 0.05f;
        }
    }
}
 