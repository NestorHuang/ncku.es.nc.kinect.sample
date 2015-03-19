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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ncku.es.nc.kinect.sample_2
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        List<KinectSensor> kinects = new List<KinectSensor>();
        Sensor Sensors = new Sensor();
        public MainWindow()
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

    }
}
