namespace RedCell.Research.Experiment
{
    public class CameraFrame
    {
        public CameraFrame(dynamic sourceFrame, CameraViews view)
        {
            SourceFrame = sourceFrame;
            View = view;
        }

        public dynamic SourceFrame { get; private set; }

        public CameraViews View { get; private set; }
    }
}
