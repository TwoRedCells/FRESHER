namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class CameraFrame.
    /// </summary>
    public class CameraFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraFrame"/> class.
        /// </summary>
        /// <param name="sourceFrame">The source frame.</param>
        /// <param name="view">The view.</param>
        public CameraFrame(dynamic sourceFrame, CameraViews view)
        {
            SourceFrame = sourceFrame;
            View = view;
        }

        /// <summary>
        /// Gets the source frame.
        /// </summary>
        /// <value>The source frame.</value>
        public dynamic SourceFrame { get; private set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public CameraViews View { get; private set; }
    }
}
