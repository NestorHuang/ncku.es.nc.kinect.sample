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

namespace ncku.es.nc.kinect.sample_1
{
    /// <summary>
    /// Highper.xaml 的互動邏輯
    /// </summary>
    public partial class Highper : Window
    {
        List<KinectSensor> kinects = new List<KinectSensor>();
        Sensor Sensors = new Sensor();

        WriteableBitmap _ColorImageBitmap;
        Int32Rect _ColorImageBitmapRect;
        int _ColorImageStride;
        public Highper()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
            kinects = Sensors.ConnetedSensor();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (kinects.Count > 0)
            {
                ColorImageStream colorStream = kinects[0].ColorStream;
                colorStream.Enable();
                //kinects[0].ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);
                kinects[0].ColorFrameReady += Kinect_ColorFrameReady;

                _ColorImageBitmap = new WriteableBitmap(colorStream.FrameWidth, colorStream.FrameHeight, 96, 96,
                    PixelFormats.Bgr32, null);
                _ColorImageBitmapRect = new Int32Rect(0, 0, colorStream.FrameWidth, colorStream.FrameHeight);
                _ColorImageStride = colorStream.FrameWidth * colorStream.FrameBytesPerPixel;
                ColorData.Source = _ColorImageBitmap;
            }
        }        

        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (kinects.Count > 0)
            {
                kinects[0].ColorStream.Disable();
                kinects[0].ColorFrameReady -= Kinect_ColorFrameReady;
                Sensors.DisConnetedAllSensor(kinects);
            }
        }

        private void Kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
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
