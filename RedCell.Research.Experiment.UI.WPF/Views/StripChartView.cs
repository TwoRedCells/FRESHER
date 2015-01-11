using RedCell.UI.WPF.Charts;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// A Strip Chart View.
    /// </summary>
    public class StripChartView : TimeGraph, IControl
    {
        Log _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="StripChartView"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public StripChartView(Log log)
        {
            _log = log;
            this.Data = log.DataSet;
            ValuesPerUnit = 0.05f;
        }
    }
}
 