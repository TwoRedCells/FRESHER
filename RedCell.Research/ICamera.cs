using System;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Interface ICamera
    /// </summary>
    public interface ICamera : ILoggable
    {
        #region Events
        /// <summary>
        /// Occurs when a frame is available.
        /// </summary>
        event EventHandler<CameraFrameEventArgs> FrameAvailable;

        /// <summary>
        /// Occurs when a face is found.
        /// </summary>
        event EventHandler<FaceEventArgs> FaceFound;

        /// <summary>
        /// Occurs when landmarks are found.
        /// </summary>
        event EventHandler<LandmarksEventArgs> LandmarksFound;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the stream setting.
        /// </summary>
        /// <value>The stream setting.</value>
        CameraStreamSetting StreamSetting { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the raw frame.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <returns>System.Byte[].</returns>
        byte[] GetRawFrame(object frame);
        #endregion
    }
}
