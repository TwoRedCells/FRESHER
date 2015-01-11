using SharpAvi;
using SharpAvi.Output;
using SharpAvi.Codecs;
using System;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class AVLog.
    /// </summary>
    public class AVLog
    {
        #region Fields
        private AviWriter _writer;
        private IAviVideoStream _video;
        //private IAviAudioStream _audio;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="AVLog" /> class.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <exception cref="System.ArgumentNullException">camera</exception>
        public AVLog(ICamera camera)
        {
            if (camera == null)
                throw new ArgumentNullException("camera");

            Camera = camera;
            EncodingQuality = 75; // Default.
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets the camera.
        /// </summary>
        /// <value>The camera.</value>
        public ICamera Camera { get; private set; }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the stream setting.
        /// </summary>
        /// <value>The stream setting.</value>
        public CameraStreamSetting StreamSetting { get; set; }

        /// <summary>
        /// Gets or sets the encoding quality.
        /// </summary>
        /// <value>The encoding quality.</value>
        public int EncodingQuality { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Records this instance.
        /// </summary>
        /// <exception cref="RedCell.Research.ResearchException">
        /// Filename must be set before recording.
        /// or
        /// A recording has already started.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Filename must be set before recording.</exception>
        public void StartRecord()
        {
            // Automatic filename
            if (string.IsNullOrWhiteSpace(Filename))
                Filename = "Video_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            if(_writer != null)
                throw new ResearchException("A recording has already started.");

            // Get video details from camera.
            if (StreamSetting == null)
                StreamSetting = Camera.StreamSetting;

            var encoder = new Mpeg4VideoEncoderVcm(StreamSetting.Width, StreamSetting.Height, StreamSetting.Framerate, 0, 50, KnownFourCCs.Codecs.X264);
            _writer = new AviWriter(Filename);
            _video = _writer.AddEncodingVideoStream(encoder, true, StreamSetting.Width, StreamSetting.Height);
            

            Camera.FrameAvailable += Camera_FrameAvailable;
        }

        /// <summary>
        /// Stops the record.
        /// </summary>
        public void StopRecord()
        {
            Camera.FrameAvailable -= Camera_FrameAvailable;
            _writer.Close();
        }

        /// <summary>
        /// Handles the FrameAvailable event of the Camera control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CameraFrameEventArgs"/> instance containing the event data.</param>
        private void Camera_FrameAvailable(object sender, CameraFrameEventArgs e)
        {
            var frame = Camera.GetRawFrame(e.Value.SourceFrame);
            _video.WriteFrame(true, frame, 0, frame.Length);
        }

        #endregion

    }
}
