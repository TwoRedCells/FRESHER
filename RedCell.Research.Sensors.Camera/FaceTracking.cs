using System;
using System.Diagnostics;

namespace face_tracking.cs
{
    internal class FaceTracking
    {
        private FPSTimer m_timer;
        private bool m_wasConnected;

        public FaceTracking()
        {
        }

        //private void DisplayDeviceConnection(bool isConnected)
        //{
        //    if (isConnected && !m_wasConnected) m_form.UpdateStatus("Device Reconnected", MainForm.Label.StatusLabel);
        //    else if (!isConnected && m_wasConnected)
        //        m_form.UpdateStatus("Device Disconnected", MainForm.Label.StatusLabel);
        //    m_wasConnected = isConnected;
        //}

        //private void DisplayPicture(PXCMImage image)
        //{
        //    PXCMImage.ImageData data;
        //    if (image.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out data) <
        //        pxcmStatus.PXCM_STATUS_NO_ERROR) return;
        //    m_form.DrawBitmap(data.ToBitmap(0, image.info.width, image.info.height));
        //    m_timer.Tick("");
        //    image.ReleaseAccess(data);
        //}

        private void CheckForDepthStream(PXCMCapture.Device.StreamProfileSet profiles, PXCMFaceModule faceModule)
        {
            PXCMFaceConfiguration faceConfiguration = faceModule.CreateActiveConfiguration();
            if (faceConfiguration == null)
            {
                Debug.Assert(faceConfiguration != null);
                return;
            }

            PXCMFaceConfiguration.TrackingModeType trackingMode = faceConfiguration.GetTrackingMode();
            faceConfiguration.Dispose();

            if (trackingMode != PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH) return;
            if (profiles.depth.imageInfo.format == PXCMImage.PixelFormat.PIXEL_FORMAT_DEPTH) return;
            PXCMCapture.DeviceInfo dinfo;
            m_form.Devices.TryGetValue(m_form.GetCheckedDevice(), out dinfo);

            if (dinfo != null)
                MessageBox.Show(
                    String.Format("Depth stream is not supported for device: {0}. \nUsing 2D tracking", dinfo.name),
                    @"Face Tracking",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FaceAlertHandler(PXCMFaceData.AlertData alert)
        {
            m_form.UpdateStatus(alert.label.ToString(), MainForm.Label.StatusLabel);
        }

        public void SimplePipeline()
        {
            PXCMSenseManager pp = m_form.Session.CreateSenseManager();

            if (pp == null)
            {
                throw new Exception("PXCMSenseManager null");
            }

            // Set Source & Landmark Profile Index 
            PXCMCapture.DeviceInfo info;
            if (m_form.GetRecordState())
            {
                pp.captureManager.SetFileName(m_form.GetFileName(), true);
                if (m_form.Devices.TryGetValue(m_form.GetCheckedDevice(), out info))
                {
                    pp.captureManager.FilterByDeviceInfo(info);
                }
            }
            else if (m_form.GetPlaybackState())
            {
                pp.captureManager.SetFileName(m_form.GetFileName(), false);
                PXCMCaptureManager cmanager = pp.QueryCaptureManager();
                if (cmanager == null)
                {
                    throw new Exception("PXCMCaptureManager null");
                }
                cmanager.SetRealtime(false);
            }
            else
            {
                if (m_form.Devices.TryGetValue(m_form.GetCheckedDevice(), out info))
                {
                    pp.captureManager.FilterByDeviceInfo(info);
                    Tuple<PXCMImage.ImageInfo, PXCMRangeF32> selectedRes = m_form.GetCheckedColorResolution();
                    var set = new PXCMCapture.Device.StreamProfileSet();
                    set.color.frameRate = selectedRes.Item2;
                    set.color.imageInfo.format = selectedRes.Item1.format;
                    set.color.imageInfo.width = selectedRes.Item1.width;
                    set.color.imageInfo.height= selectedRes.Item1.height;
                    pp.captureManager.FilterByStreamProfiles(set);
                }
            }

            // Set Module            
            pp.EnableFace();
            PXCMFaceModule faceModule = pp.QueryFace();
            if (faceModule == null)
            {
                Debug.Assert(faceModule != null);
                return;
            }
            
            PXCMFaceConfiguration moduleConfiguration = faceModule.CreateActiveConfiguration();

            if (moduleConfiguration == null)
            {
                Debug.Assert(moduleConfiguration != null);
                return;
            }
            
            PXCMFaceConfiguration.TrackingModeType mode = m_form.GetCheckedProfile().Contains("3D")
                ? PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH
                : PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR;

            moduleConfiguration.SetTrackingMode(mode);

            moduleConfiguration.strategy = PXCMFaceConfiguration.TrackingStrategyType.STRATEGY_RIGHT_TO_LEFT;

            moduleConfiguration.detection.maxTrackedFaces = m_form.NumDetection;
            moduleConfiguration.landmarks.maxTrackedFaces = m_form.NumLandmarks;
            moduleConfiguration.pose.maxTrackedFaces = m_form.NumPose;
            PXCMFaceConfiguration.ExpressionsConfiguration econfiguration = moduleConfiguration.QueryExpressions();
            if (econfiguration == null)
            {
                throw new Exception("ExpressionsConfiguration null");
            }
            econfiguration.properties.maxTrackedFaces = m_form.NumExpressions;

           econfiguration.EnableAllExpressions();
            moduleConfiguration.detection.isEnabled = m_form.IsDetectionEnabled();
            moduleConfiguration.landmarks.isEnabled = m_form.IsLandmarksEnabled();
            moduleConfiguration.pose.isEnabled = m_form.IsPoseEnabled();
            if (m_form.IsExpressionsEnabled())
            { 
                econfiguration.Enable();
            }

            PXCMFaceConfiguration.RecognitionConfiguration qrecognition = moduleConfiguration.QueryRecognition();
            if (qrecognition == null)
            {
                throw new Exception("PXCMFaceConfiguration.RecognitionConfiguration null");
            }
            if (m_form.IsRecognitionChecked())
            {
                qrecognition.Enable();
            }

            moduleConfiguration.EnableAllAlerts();
            moduleConfiguration.SubscribeAlert(FaceAlertHandler);

            pxcmStatus applyChangesStatus = moduleConfiguration.ApplyChanges();

            m_form.UpdateStatus("Init Started", MainForm.Label.StatusLabel);

            if (applyChangesStatus < pxcmStatus.PXCM_STATUS_NO_ERROR || pp.Init() < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                m_form.UpdateStatus("Init Failed", MainForm.Label.StatusLabel);
            }
            else
            {
                using (PXCMFaceData moduleOutput = faceModule.CreateOutput())
                {
                    Debug.Assert(moduleOutput != null);
                    PXCMCapture.Device.StreamProfileSet profiles;

                    PXCMCaptureManager cmanager = pp.QueryCaptureManager();
                    if (cmanager == null)
                    {
                        throw new Exception("capture manager null");
                    }
                    PXCMCapture.Device device  =  cmanager.QueryDevice();

                    if (device == null)
                    {
                        throw new Exception("device null");
                    }
                    
                    device.QueryStreamProfileSet(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, 0, out profiles);
                    CheckForDepthStream(profiles, faceModule);
                    
                    ushort threshold = device.QueryDepthConfidenceThreshold();
                    int filter_option = device.QueryIVCAMFilterOption();
                    int range_tradeoff = device.QueryIVCAMMotionRangeTradeOff();

                    device.SetDepthConfidenceThreshold(1);
                    device.SetIVCAMFilterOption(6);
                    device.SetIVCAMMotionRangeTradeOff(21);

                    if (m_form.IsMirrored())
                    {
                        device.SetMirrorMode(PXCMCapture.Device.MirrorMode.MIRROR_MODE_HORIZONTAL);
                    }
                    else
                    {
                        device.SetMirrorMode(PXCMCapture.Device.MirrorMode.MIRROR_MODE_DISABLED);
                    }

                    m_form.UpdateStatus("Streaming", MainForm.Label.StatusLabel);
                    m_timer = new FPSTimer(m_form);

                    while (!m_form.Stopped)
                    {
                        if (pp.AcquireFrame(true) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                        bool isConnected = pp.IsConnected();
                        DisplayDeviceConnection(isConnected);
                        if (isConnected)
                        {
                            PXCMCapture.Sample sample = pp.QueryFaceSample();
                            if (sample == null)
                            {
                                pp.ReleaseFrame();
                                continue;
                            }
                            DisplayPicture(sample.color);

                            moduleOutput.Update();
                            if (moduleConfiguration.QueryRecognition().properties.isEnabled)
                                UpdateRecognition(moduleOutput);

                            m_form.DrawGraphics(moduleOutput);
                            m_form.UpdatePanel();
                        }
                        pp.ReleaseFrame();
                    }

                    device.SetDepthConfidenceThreshold(threshold);
                    device.SetIVCAMFilterOption(filter_option);
                    device.SetIVCAMMotionRangeTradeOff(range_tradeoff);
                }

                moduleConfiguration.UnsubscribeAlert(FaceAlertHandler);
                moduleConfiguration.ApplyChanges();
                m_form.UpdateStatus("Stopped", MainForm.Label.StatusLabel);
            }
            moduleConfiguration.Dispose();
            pp.Close();
            pp.Dispose();
        }

        private void UpdateRecognition(PXCMFaceData faceOutput)
        {
            //TODO: add null checks
            if (m_form.Register) RegisterUser(faceOutput);
            if (m_form.Unregister) UnregisterUser(faceOutput);
        }

        private void RegisterUser(PXCMFaceData faceOutput)
        {
            m_form.Register = false;
            if (faceOutput.QueryNumberOfDetectedFaces() <= 0)
                return;

            PXCMFaceData.Face qface = faceOutput.QueryFaceByIndex(0);
            if (qface == null)
            {
                throw new Exception("PXCMFaceData.Face null");
            }
            PXCMFaceData.RecognitionData rdata = qface.QueryRecognition();
            if (rdata == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");
            }
            rdata.RegisterUser();
        }

        private void UnregisterUser(PXCMFaceData faceOutput)
        {
            m_form.Unregister = false;
            if (faceOutput.QueryNumberOfDetectedFaces() <= 0)
            {
                return;
            }

            PXCMFaceData.Face qface = faceOutput.QueryFaceByIndex(0);
            if (qface == null)
            {
                throw new Exception("PXCMFaceData.Face null");
            }

            PXCMFaceData.RecognitionData rdata = qface.QueryRecognition();
            if (rdata == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");
            }

            if (!rdata.IsRegistered())
            {
                return;
            }
            rdata.UnregisterUser();
        }
    }
}