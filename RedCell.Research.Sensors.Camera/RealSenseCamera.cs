using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RedCell.Research.Experiment;

namespace RedCell.Research.Sensors
{
    /// <summary>
    /// Class Camera. This class cannot be inherited.
    /// </summary>
    public sealed class RealSenseCamera : ICamera
    {
        #region Constants
        /// <summary>
        /// The real sense key
        /// </summary>
        public const string RealSenseKey = "Intel(R) RealSense(TM) 3D Camera";

        /// <summary>
        /// The default pixel format
        /// </summary>
        public const PXCMImage.PixelFormat DefaultPixelFormat = PXCMImage.PixelFormat.PIXEL_FORMAT_YUY2;

        /// <summary>
        /// The default width
        /// </summary>
        public const int DefaultWidth = 640;

        /// <summary>
        /// The default height
        /// </summary>
        public const int DefaultHeight = 480;
        #endregion

        #region Fields
        private PXCMSenseManager _sm = null;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="RealSenseCamera"/> class.
        /// </summary>
        public RealSenseCamera()
        {
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public bool Initialize()
        {
            if(Session == null)
                Session = PXCMSession.CreateInstance();

            var version = Session.QueryVersion();
            Console.WriteLine("SDK Version {0}.{1}", version.major, version.minor);

            EnumerateDevices();
            EnumerateResolutions();

            // Set default camera.
            Device = Devices.Where(d => d.Key == RealSenseKey).Select(d => d.Value).FirstOrDefault();

            if (Device == null)
                Device = Devices.Select(d => d.Value).FirstOrDefault();

            if (Device == null)
                return false;

            // Set default resolution.
            Resolution = ColorResolutions[Device.name].Where(r =>
                        r.Item1.format == DefaultPixelFormat && r.Item1.width == DefaultWidth &&
                        r.Item1.height == DefaultHeight).Select(r => r).FirstOrDefault();

            if (Resolution == null)
                return false;

            return true;
        }
        #endregion

        #region Properties
        public static PXCMSession Session { get; private set; }
        public Dictionary<string, PXCMCapture.DeviceInfo> Devices { get; private set; }
        public PXCMCapture.DeviceInfo Device { get; set; }
        public Dictionary<string, IEnumerable<Tuple<PXCMImage.ImageInfo, PXCMRangeF32>>> ColorResolutions { get; private set; }
        public Tuple<PXCMImage.ImageInfo, PXCMRangeF32> Resolution { get; set; }
        private readonly List<Tuple<int, int>> SupportedColorResolutions = new List<Tuple<int, int>>
        {
            Tuple.Create(1920, 1080),
            Tuple.Create(1280, 720),
            Tuple.Create(960, 540),
            Tuple.Create(640, 480),
            Tuple.Create(640, 360),
        };

        public bool EnableFace { get; set; }

        public bool EnableEmotion { get; set; }

        public bool EnableStreaming { get; set; }

        public bool EnableExpression { get; set; }

        public bool EnableLandmarks { get; set; }
        #endregion

        #region Events
        public event EventHandler<CameraFrameEventArgs> FrameAvailable;

        public event EventHandler<FaceEventArgs> FaceFound;

        private void OnFaceFound(object sender, FaceEventArgs e)
        {
            if (FaceFound != null)
                FaceFound(sender, e);
        }

        public event EventHandler<DataEventArgs> DataAvailable;

        private void OnDataAvailable(object sender, DataEventArgs e)
        {
            if (DataAvailable != null)
                DataAvailable(sender, e);
        }

        public event EventHandler<CameraFrameEventArgs> ColourImage;

        /// <summary>
        /// Called when a colour image is ready.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnColourImage(object sender, EventArgs<PXCMImage> e)
        {
            if (ColourImage != null)
                ColourImage(sender, new CameraFrameEventArgs(new CameraFrame(e.Value, CameraViews.Colour)));
            if (FrameAvailable != null)
                FrameAvailable(sender, new CameraFrameEventArgs(new CameraFrame(e.Value, CameraViews.Colour)));
        }

        public event EventHandler<CameraFrameEventArgs> DepthImage;

        /// <summary>
        /// Called when a depth image is available..
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnDepthImage(object sender, EventArgs<PXCMImage> e)
        {
            if (DepthImage != null)
                DepthImage(sender, new CameraFrameEventArgs(new CameraFrame(e.Value, CameraViews.Depth)));
            if (FrameAvailable != null)
                FrameAvailable(sender, new CameraFrameEventArgs(new CameraFrame(e.Value, CameraViews.Depth)));
        }

        public event EventHandler<CameraFrameEventArgs> InfraredImage;

        /// <summary>
        /// Called when and infrared image is available.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnInfraredImage(object sender, EventArgs<PXCMImage> e)
        {
            if (InfraredImage != null)
                InfraredImage(sender, new CameraFrameEventArgs(new CameraFrame(e.Value, CameraViews.Infrared)));
            if (FrameAvailable != null)
                FrameAvailable(sender, new CameraFrameEventArgs(new CameraFrame(e.Value, CameraViews.Infrared)));
        }

        /// <summary>
        /// Occurs when an emotion is found.
        /// </summary>
        public event EventHandler<EmotionEventArgs> EmotionFound;

        /// <summary>
        /// Handles the <see cref="E:EmotionFound" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EmotionEventArgs"/> instance containing the event data.</param>
        private void OnEmotionFound(object sender, EmotionEventArgs e)
        {
            if (EmotionFound != null)
                EmotionFound(sender, e);
        }

        /// <summary>
        /// Occurs when landmarks is found.
        /// </summary>
        public event EventHandler<LandmarksEventArgs> LandmarksFound;

        /// <summary>
        /// Handles the <see cref="E:LandmarksFound" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LandmarksEventArgs"/> instance containing the event data.</param>
        private void OnLandmarksFound(object sender, LandmarksEventArgs e)
        {
            if (LandmarksFound != null)
                LandmarksFound(sender, e);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (_sm != null)
                throw new ResearchException("Camera is already started.");

            _sm =  PXCMSenseManager.CreateInstance();

            // Configure face detection.
            if (EnableFace)
            {
                _sm.EnableFace();
                var faceModule = _sm.QueryFace();
                using (PXCMFaceConfiguration faceConfig = faceModule.CreateActiveConfiguration())
                {
                    faceConfig.EnableAllAlerts();
                    faceConfig.pose.isEnabled = true;
                    faceConfig.pose.maxTrackedFaces = 4;

                    if (EnableExpression)
                    {
                        PXCMFaceConfiguration.ExpressionsConfiguration expression = faceConfig.QueryExpressions();
                        expression.Enable();
                        expression.EnableAllExpressions();
                        faceConfig.ApplyChanges();
                    }
                }
            }

            if (EnableEmotion)
            {
                // Configure emotion detection.
                _sm.EnableEmotion();
            }

            if (EnableStreaming)
            {
                // Configure streaming.
                _sm.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 640, 480);
            //    _sm.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, 640, 480);
            //    _sm.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_IR, 640, 480);
            }

            // Event handler for data callbacks.
            var handler = new PXCMSenseManager.Handler {
                onModuleProcessedFrame=OnModuleProcessedFrame
            };

            _sm.Init(handler);

            // GO.
            Debug.WriteLine("{0} Starting streaming.", Time());
            _sm.StreamFrames(false);
            
            


            //Debug.WriteLine("{0} End streaming.", Time());
        }

        private string Time()
        {
            return DateTime.Now.ToString("H:mm:ss");
        }

        public void EnumerateDevices()
        {
            Devices = new Dictionary<string, PXCMCapture.DeviceInfo>();
            var desc = new PXCMSession.ImplDesc
            {
                group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR,
                subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE
            };

            for (int i = 0; ; i++)
            {
                PXCMSession.ImplDesc desc1;
                if (Session.QueryImpl(desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                PXCMCapture capture;
                if (Session.CreateImpl(desc1, out capture) < pxcmStatus.PXCM_STATUS_NO_ERROR) continue;

                for (int j = 0; ; j++)
                {
                    PXCMCapture.DeviceInfo dinfo;
                    if (capture.QueryDeviceInfo(j, out dinfo) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                    if (!Devices.ContainsKey(dinfo.name))
                        Devices.Add(dinfo.name, dinfo);
                }

                capture.Dispose();
            }
        }


        pxcmStatus OnModuleProcessedFrame(int type, PXCMBase module, PXCMCapture.Sample sample) 
        {
            // Process the frame using the appropriate callback.
            switch(type)
            {
                case PXCMFaceModule.CUID:
                    OnFaceCallback(module as PXCMFaceModule);
                    break;

                case PXCMEmotion.CUID:
                    OnEmotionCallback(module as PXCMEmotion);
                    break;

                default:
                    throw new NotImplementedException("Unknown type.");
            }

            // Handle graphics.
            if (sample.color != null)
                OnColourImage(this, new EventArgs<PXCMImage>(sample.color));
            if (sample.depth != null)
                OnDepthImage(this, new EventArgs<PXCMImage>(sample.depth));
            if (sample.ir != null)
                OnInfraredImage(this, new EventArgs<PXCMImage>(sample.ir));

           // return NO_ERROR to continue, or any error to abort.
           return pxcmStatus.PXCM_STATUS_NO_ERROR;
        }

        private void OnFaceCallback(PXCMFaceModule module)
        {
            PXCMRectI32 bounds;
            using (var faceData = module.CreateOutput())
            {
                faceData.Update();
                var faces = faceData.QueryFaces();
                foreach (var face in faces)
                {
                    var detection = face.QueryDetection();
                    detection.QueryBoundingRect(out bounds);
//                    Debug.WriteLine("{0} Face detected: {1}", Time(), bounds);

                    var landmarkData = face.QueryLandmarks();
                    if (landmarkData != null)
                    {
                        PXCMFaceData.LandmarkPoint[] landmarks;
                        landmarkData.QueryPoints(out landmarks);

                        var landmarkDict = new Dictionary<string, Point>();
                        
                        foreach(PXCMFaceData.LandmarkPoint landmark in landmarks)
                        {
                            landmarkDict.Add("LANDMARK_" + landmark.source.index, new Point(landmark.image.x, landmark.image.y));
                            
                            /*Debug.WriteLine("{0}/{1} at {2},{3},{4}",
                                landmark.source.index,
                                landmark.source.alias,
                                landmark.image.x,
                                landmark.image.y,
                                landmark.confidenceImage);
                            */
                        }
                        
                        var landmarkArgs = new LandmarksEventArgs(landmarkDict, Resolution.Item1.width, Resolution.Item1.height);
                        OnLandmarksFound(this, landmarkArgs);
                    }

                    // Expression
                    var expressionValues = new Dictionary<string, double>();
                    var expressionData = face.QueryExpressions();
                    if (expressionData != null)
                    {
                        foreach (PXCMFaceData.ExpressionsData.FaceExpression expression in Enum.GetValues(typeof(PXCMFaceData.ExpressionsData.FaceExpression)))
                        {
                            PXCMFaceData.ExpressionsData.FaceExpressionResult score;
                            expressionData.QueryExpression(expression, out score);
                            expressionValues.Add(expression.ToString(), score.intensity / 100d);
//                            Debug.WriteLine("{0} Expression: {1} == {2}", Time(), expression, score.intensity / 100d);
                        }
                    }
                    OnFaceFound(this, new FaceEventArgs(new Rectangle(bounds.x, bounds.y, bounds.w, bounds.h), expressionValues, Resolution.Item1.width, Resolution.Item1.height));
                    OnDataAvailable(this, new DataEventArgs(expressionValues));
                }
            }
        }

        private void OnEmotionCallback(PXCMEmotion module)
        {
            PXCMEmotion.EmotionData[] emotions;
            int faces = module.QueryNumFaces();
            // Debug.WriteLine("{0} Faces detected: {1}", Time(), faces);

            for (int face = 0; face < faces; face++)
            {
                module.QueryAllEmotionData(face, out emotions);
                foreach (var emotion in emotions)
                {
                    if (emotion.evidence <= 0)
                        continue;
                    Debug.WriteLine("{0} Faces #{1} has {2} with evidence {3} and intensity {4} at rectangle {5},{6},{7},{8}",
                        Time(), emotion.fid, emotion.eid, emotion.evidence, emotion.intensity,
                        emotion.rectangle.x, emotion.rectangle.y, emotion.rectangle.w, emotion.rectangle.h);
                }
            }
        }

        public void Stop()
        {
            if (_sm != null)
            {
                _sm.Close();
                _sm.Dispose();
                _sm = null;
            }
        }


        /// <summary>
        /// Enumerates the resolutions.
        /// </summary>
        /// <exception cref="System.Exception">PXCMCapture.Device null</exception>
        public void EnumerateResolutions()
        {
            ColorResolutions = new Dictionary<string, IEnumerable<Tuple<PXCMImage.ImageInfo, PXCMRangeF32>>>();
            var desc = new PXCMSession.ImplDesc
            {
                group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR,
                subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE
            };

            for (int i = 0; ; i++)
            {
                PXCMSession.ImplDesc desc1;
                if (Session.QueryImpl(desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                PXCMCapture capture;
                if (Session.CreateImpl(desc1, out capture) < pxcmStatus.PXCM_STATUS_NO_ERROR) continue;

                for (int j = 0; ; j++)
                {
                    PXCMCapture.DeviceInfo info;
                    if (capture.QueryDeviceInfo(j, out info) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                    PXCMCapture.Device device = capture.CreateDevice(j);
                    if (device == null)
                    {
                        throw new Exception("PXCMCapture.Device null");
                    }
                    var deviceResolutions = new List<Tuple<PXCMImage.ImageInfo, PXCMRangeF32>>();

                    for (int k = 0; k < device.QueryStreamProfileSetNum(PXCMCapture.StreamType.STREAM_TYPE_COLOR); k++)
                    {
                        PXCMCapture.Device.StreamProfileSet profileSet;
                        device.QueryStreamProfileSet(PXCMCapture.StreamType.STREAM_TYPE_COLOR, k, out profileSet);
                        var currentRes = new Tuple<PXCMImage.ImageInfo, PXCMRangeF32>(profileSet.color.imageInfo,
                            profileSet.color.frameRate);

                        if (SupportedColorResolutions.Contains(new Tuple<int, int>(currentRes.Item1.width, currentRes.Item1.height)))
                        {
                            deviceResolutions.Add(currentRes);
                        }
                    }
                    ColorResolutions.Add(info.name, deviceResolutions);
                    device.Dispose();
                }

                capture.Dispose();
            }
        }


        #endregion

    }
}
