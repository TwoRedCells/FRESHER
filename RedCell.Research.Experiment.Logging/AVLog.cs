using SharpAvi.Output;
using System;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class AVLog.
    /// </summary>
    public class AVLog
    {
        #region Fields
        /// <summary>
        /// The _writer
        /// </summary>
        private AviWriter _writer;
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
            if(string.IsNullOrWhiteSpace(Filename))
                throw new ResearchException("Filename must be set before recording.");

            if(_writer != null)
                throw new ResearchException("A recording has already started.");

            // Get video details from camera.
           // Camera.

            _writer = new AviWriter(Filename);
            //writer.AddMpeg4VideoStrea
        }

        /// <summary>
        /// Stops the record.
        /// </summary>
        public void StopRecord()
        {
            _writer.Close();

        }
        #endregion

    }
}
