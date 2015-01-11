namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Interface IRegion
    /// </summary>
    public interface IRegion : IControl, IRectangle
    {
        /// <summary>
        /// Adds the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        void Add(IControl content);

        /// <summary>
        /// Removes the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        void Remove(IControl content);
    }
}
