using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace ncku.es.nc.kinect.sample.dll
{
    public class Sensor
    {
        List<KinectSensor> KinectSensorList;
        KinectSensorCollection potentialSensors = KinectSensor.KinectSensors;
        private EventHandler<DepthImageFrameReadyEventArgs> kinect_DepthFrameReady;
        private EventHandler<SkeletonFrameReadyEventArgs> kinect_SkeletonFrameReady;
        private EventHandler<ColorImageFrameReadyEventArgs> kinect_ColorFrameReady;
        private object interactionstream_InteractionFrameReady;

        #region 連線Kinect
        public List<KinectSensor> ConnetedSensor()
        {
            List<KinectSensor> kinects = new List<KinectSensor>();

            if (KinectSensor.KinectSensors.Count > 0)
            {
                for (int count = 0; count < KinectSensor.KinectSensors.Count; count++)
                {
                    kinects.Add(activeSensor(count, true, true, true));
                }
            }                

            return kinects;
        }

        /// <summary>
        /// 啟動Sensor
        /// 深度識別為預設(DepthRange.Default)
        /// 影像資訊類型為最大值(Resolution640x480Fps30)
        /// </summary>
        /// <param name="serial">Sersor序號</param>
        /// <param name="activeDepth">啟動深度追蹤</param>
        /// <param name="activeSkeleton">啟動骨架追蹤</param>
        /// <param name="activeColor">啟動色彩追蹤</param>
        private KinectSensor activeSensor(int serial, bool activeDepth, bool activeSkeleton, bool activeColor)
        {
            //手勢追蹤在近接模式使用所以在一般模式下直接不採用
            bool activeInteraction = false;
            return activeSensor(serial, activeDepth, DepthRange.Default, DepthImageFormat.Resolution640x480Fps30, activeSkeleton, SkeletonTrackingMode.Default, activeColor, activeInteraction);
        }

        /// <summary>
        /// 啟動Sensor
        /// </summary>
        /// <param name="serial">Sersor序號</param>
        /// <param name="activeDepth">啟動深度追蹤</param>
        /// <param name="depthRange">深度資訊類型：Default(一般模式)、Near(近接模式)</param>
        /// <param name="depthImageFormat">影像資訊類型：Resolution320x240Fps30、Resolution640x480Fps30、Resolution80x60Fps30</param>
        /// <param name="activeSkeleton">啟動骨架追蹤</param>
        /// <param name="skeletonTrackingMode">骨架追蹤模式：Default(全身)、Seated(坐姿)</param>
        /// <param name="activeColor">啟動色彩追蹤</param>
        /// <param name="activeInteraction">啟動手勢追蹤</param>
        private KinectSensor activeSensor(int serial, bool activeDepth, DepthRange depthRange, DepthImageFormat depthImageFormat, bool activeSkeleton, SkeletonTrackingMode skeletonTrackingMode, bool activeColor, bool activeInteraction)
        {
            KinectSensor kinectSensor = KinectSensor.KinectSensors[serial];

            //深度追蹤
            if (activeDepth)
                activeDepthFunction(kinectSensor, depthRange, depthImageFormat);

            //骨架追蹤
            bool inNearRange = false; //是否為近接模式
            if (depthRange == DepthRange.Near)
                inNearRange = true;

            if (activeSkeleton)
                activeSkeletonFunction(kinectSensor, skeletonTrackingMode, inNearRange);

            //色彩追蹤
            if (activeColor)
                activeColorFunction(kinectSensor);

            //手勢追蹤
            //if (activeInteraction)
            //    activeInteractionFunction(kinectSensor);

            kinectSensor.Start();

            return kinectSensor;
        }

        ///// <summary>
        ///// 啟動手勢追蹤
        ///// </summary>
        ///// <param name="kinectSensor">要啟動的KinectSensor</param>
        //private void activeInteractionFunction(KinectSensor kinectSensor)
        //{
        //    InteractionStream interactionstream;
        //    interactionstream = new InteractionStream(kinectSensor, new DummyInteractionClient());
        //    interactionstream.InteractionFrameReady += interactionstream_InteractionFrameReady;
        //}

        /// <summary>
        /// 啟動色彩追蹤
        /// </summary>
        /// <param name="kinectSensor">要啟動的KinectSensor</param>
        private void activeColorFunction(KinectSensor kinectSensor)
        {
            kinectSensor.ColorFrameReady += kinect_ColorFrameReady;
            kinectSensor.ColorStream.Enable();
        }

        /// <summary>
        /// 啟動骨架追蹤
        /// </summary>
        /// <param name="kinectSensor">要啟動的KinectSensor</param>
        private void activeSkeletonFunction(KinectSensor kinectSensor,SkeletonTrackingMode skeletonTrackingMode,bool inNearRange)
        {
            kinectSensor.SkeletonStream.TrackingMode = skeletonTrackingMode;
            kinectSensor.SkeletonStream.EnableTrackingInNearRange = inNearRange;
            kinectSensor.SkeletonFrameReady += kinect_SkeletonFrameReady;
            kinectSensor.SkeletonStream.Enable();
        }

        /// <summary>
        /// 啟動深度追蹤
        /// </summary>
        /// <param name="kinectSensor">要啟動的KinectSensor</param>
        /// <param name="depthRange">深度資訊類型：一般模式、近接模式</param>
        private void activeDepthFunction(KinectSensor kinectSensor, DepthRange depthRange,DepthImageFormat depthImageFormat)
        {
            kinectSensor.DepthStream.Range = depthRange;
            kinectSensor.DepthFrameReady += kinect_DepthFrameReady;
            kinectSensor.DepthStream.Enable(depthImageFormat);
        }

        /// <summary>
        /// 關閉Kinect
        /// </summary>
        /// <param name="kinectSensor">要關閉的KinectSensor</param>
        /// <returns></returns>
        public bool DisConnetedSensor(KinectSensor kinectSensor)
        {
            bool isDisConnected = false;
            try
            {
                kinectSensor.Stop();
                isDisConnected = true;
            }
            finally
            {
                
            }

            return isDisConnected;
        }

        /// <summary>
        /// 關閉所有Kinect
        /// </summary>
        /// <param name="KinectSensorList">要關閉的KinectSensor List</param>
        /// <returns></returns>
        public bool DisConnetedAllSensor(List<KinectSensor> KinectSensorList)
        {
            bool isDisConnectedAllSensor = false;
            foreach (KinectSensor kinectSensor in KinectSensorList)
            {
                isDisConnectedAllSensor = DisConnetedSensor(kinectSensor); ;               
            }
            return isDisConnectedAllSensor;
        }

        #endregion

        #region 改變Kinect狀態

        /// <summary>
        /// 改變Kinect角度
        /// </summary>
        /// <param name="Sensor">指定變更Sensor</param>
        /// <param name="Angle">角度值27~-27度</param>
        /// <returns></returns>
        public string ChangeAngle(KinectSensor Sensor,int Angle)
        {
            int angle = Angle;
            KinectSensor sensor=Sensor;
            string statusString = "";

            if (checkAngleInRange(angle))
                sensor.ElevationAngle = angle;
            else
                statusString = "角度輸入錯誤，僅可輸入27 ~ -27範圍";

            return statusString;
        }

        /// <summary>
        /// 檢查角度在範圍內
        /// </summary>
        /// <param name="angle">角度值</param>
        /// <returns></returns>
        private bool checkAngleInRange(int angle)
        {
            bool inRange=true;
            if (angle > 27)
                inRange = false;
            if (angle < -27)
                inRange = false;

            return inRange;
        }

        #endregion

        #region 檢查狀態

        public bool IsConnected()
        {
            bool isConnected=false;
            if (KinectSensor.KinectSensors.Count > 0)
            {
                for (int count = 0; count < KinectSensor.KinectSensors.Count; count++)
                {
                    if (KinectSensor.KinectSensors[count].IsRunning)
                        isConnected = true;
                }
            }

            return isConnected;
        }

        #endregion
    }
}
