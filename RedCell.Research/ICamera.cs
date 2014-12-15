using System;

namespace RedCell.Research.Experiment
{
    public interface ICamera
    {
        event EventHandler<CameraFrameEventArgs> FrameAvailable;

        event EventHandler<FaceEventArgs> FaceFound;
    }
}
