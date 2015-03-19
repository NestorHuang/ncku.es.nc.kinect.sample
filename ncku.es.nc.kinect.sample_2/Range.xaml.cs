using Microsoft.Kinect;
using ncku.es.nc.kinect.sample.dll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ncku.es.nc.kinect.sample_2
{
    /// <summary>
    /// Range.xaml 的互動邏輯
    /// </summary>
    public partial class Range : Window
    {
        List<KinectSensor> kinects = new List<KinectSensor>();
        Sensor Sensors = new Sensor();
        public Range()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
            kinects = Sensors.ConnetedSensor();
        }

        private WriteableBitmap _ColorImageBitmap;
        private Int32Rect _ColorImageBitmapRect;
        private int _ColorImageStride;

        private WriteableBitmap _DepthImageBitmap;
        private Int32Rect _DepthImageBitmapRect;
        private int _DepthImageStride;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (kinects.Count > 0)
            {
                #region 處理彩色影像
                ColorImageStream colorStream = kinects[0].ColorStream;
                kinects[0].ColorStream.Enable();
                _ColorImageBitmap = new WriteableBitmap(colorStream.FrameWidth, colorStream.FrameHeight, 96, 96, PixelFormats.Bgr32, null);
                _ColorImageBitmapRect = new Int32Rect(0, 0, colorStream.FrameWidth, colorStream.FrameHeight);
                _ColorImageStride = colorStream.FrameWidth * colorStream.FrameBytesPerPixel;
                ColorData.Source = _ColorImageBitmap;
                kinects[0].ColorFrameReady += myKinect_ColorFrameReady;
                #endregion

                DepthImageStream depthStream = kinects[0].DepthStream;
                kinects[0].DepthStream.Enable();
                _DepthImageBitmap = new WriteableBitmap(depthStream.FrameWidth, depthStream.FrameHeight,
                    96, 96, PixelFormats.Gray16, null);
                _DepthImageBitmapRect = new Int32Rect(0, 0, depthStream.FrameWidth, depthStream.FrameHeight);
                _DepthImageStride = depthStream.FrameWidth * depthStream.FrameBytesPerPixel;
                DepthData.Source = _DepthImageBitmap;
                kinects[0].DepthFrameReady += mykinect_DepthFrameReady;
            }
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (kinects.Count > 0)
            {
                kinects[0].ColorStream.Disable();
                kinects[0].DepthStream.Disable();
                kinects[0].ColorFrameReady -= myKinect_ColorFrameReady;
                kinects[0].DepthFrameReady -= mykinect_DepthFrameReady;
                Sensors.DisConnetedAllSensor(kinects);
            }
        }

        short[] pixelData;
        void mykinect_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame frame = e.OpenDepthImageFrame())
            {
                if (frame == null)
                    return;

                pixelData = new short[frame.PixelDataLength];
                frame.CopyPixelDataTo(pixelData);
                _DepthImageBitmap.WritePixels(_DepthImageBitmapRect, pixelData, _DepthImageStride, 0);
            }
        }

        void myKinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame frame = e.OpenColorImageFrame())
            {
                if (frame == null)
                    return;

                byte[] pixelData = new byte[frame.PixelDataLength];
                frame.CopyPixelDataTo(pixelData);
                _ColorImageBitmap.WritePixels(_ColorImageBitmapRect, pixelData, _ColorImageStride, 0);
            }
        }

        private void ColorData_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(ColorData);

            if (pixelData != null && pixelData.Length > 0)
            {
                int pixelIndex = (int)(p.X + ((int)p.Y * kinects[0].DepthStream.FrameWidth));
                int depth = pixelData[pixelIndex] >> DepthImageFrame.PlayerIndexBitmaskWidth;
                Title = "影像深度 : " + depth + " 公厘(mm)" + DepthInfoMeaning(depth);
            }
        }

        string DepthInfoMeaning(int depth)
        {
            string info = "無效距離";
            if (depth >= kinects[0].DepthStream.MinDepth &&
                 depth <= kinects[0].DepthStream.MaxDepth)
                info = "有效距離內 ";
            else if (depth == kinects[0].DepthStream.UnknownDepth)
                info = "無法判定距離 ";
            else if (depth == kinects[0].DepthStream.TooFarDepth)
                info = "距離太遠 ";
            else if (depth == kinects[0].DepthStream.TooNearDepth)
                info = "距離太近 ";

            return info;
        }

        private void RangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (RangeButton.Content.ToString() == "預設模式")
                SwitchToNearMode();
            else
                SwitchToDefaultMode();
        }

        void SwitchToNearMode()
        {
            try
            {
                kinects[0].DepthStream.Range = DepthRange.Near;
                RangeButton.Content = "近距離模式";
            }
            catch
            {
                MessageBox.Show("您的裝置不支援近距離模式");
            }
        }

        void SwitchToDefaultMode()
        {
            kinects[0].DepthStream.Range = DepthRange.Default;
            RangeButton.Content = "預設模式";
        }      

    }
}
