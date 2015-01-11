using RedCell.Data.Charts;
using System.Collections.Generic;
using System.Linq;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class Log.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// The _names
        /// </summary>
        IEnumerable<string> _names;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log()
        {
            DataSet = new DataSet();
        }

        /// <summary>
        /// Datas the set.
        /// </summary>
        /// <value>The data set.</value>
        public DataSet DataSet { get; set; }

        /// <summary>
        /// Monitors the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="names">The names.</param>
        public void Monitor(ILoggable source, IEnumerable<string> names)
        {
            source.DataAvailable += source_DataAvailable;
            _names = names;

            foreach (string name in names)
                DataSet.Add(new Series(name){Maximum = 1, Minimum = -1});
        }

        /// <summary>
        /// Handles the DataAvailable event of the source control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataEventArgs"/> instance containing the event data.</param>
        void source_DataAvailable(object sender, DataEventArgs e)
        {
            foreach(var set in e.Values)
            {
                if(_names.Contains(set.Key))
                    DataSet.AddValue(set.Key, new Datum<double>(set.Value));
            }
        }
    }
}
