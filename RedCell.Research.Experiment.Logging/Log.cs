using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedCell.Data.Charts;

namespace RedCell.Research.Experiment
{
    public class Log
    {
        IEnumerable<string> _names;

        public Log()
        {
            DataSet = new DataSet();
        }

        /// <summary>
        /// Datas the set.
        /// </summary>
        /// <returns>DataSet.</returns>
        public DataSet DataSet { get; set; }

        public void Monitor(ILoggable source, IEnumerable<string> names)
        {
            source.DataAvailable += source_DataAvailable;
            _names = names;

            foreach (string name in names)
                DataSet.Add(new Series(name){Maximum = 1, Minimum = -1});
        }

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
