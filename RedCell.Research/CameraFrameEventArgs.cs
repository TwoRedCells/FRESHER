namespace RedCell.Research.Experiment
{
    public class CameraFrameEventArgs : EventArgs<CameraFrame>
    {
        public CameraFrameEventArgs(CameraFrame frame) : base(frame)
        { }
    }
}
