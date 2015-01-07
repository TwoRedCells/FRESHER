using System;

namespace RedCell.Research.Experiment
{
    public interface ICamera : ILoggable
    {
        event EventHandler<CameraFrameEventArgs> FrameAvailable;

        event EventHandler<FaceEventArgs> FaceFound;

        event EventHandler<LandmarksEventArgs> LandmarksFound;
    }
}
