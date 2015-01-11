namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class CameraFrameEventArgs.
    /// </summary>
    public class CameraFrameEventArgs : EventArgs<CameraFrame>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraFrameEventArgs"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public CameraFrameEventArgs(CameraFrame frame) : base(frame)
        { }
    }
}
