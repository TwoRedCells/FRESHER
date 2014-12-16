namespace RedCell.Research.Experiment
{
    public interface IExperimentUI
    {
        /// <summary>
        /// Messages the box.
        /// </summary>
        /// <param name="message">The message.</param>
        void MessageBox(string message);

        /// <summary>
        /// Adds a region.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        IRegion AddRegion(float x, float y, float w, float h);
    }
}
