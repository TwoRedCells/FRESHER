namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Interface ICameraView
    /// </summary>
    public interface ICameraView : IControl
    {
        /// <summary>
        /// Gets the camera.
        /// </summary>
        /// <value>The camera.</value>
        ICamera Camera { get; }
    }
}
