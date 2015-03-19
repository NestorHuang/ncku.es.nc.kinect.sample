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

namespace ncku.es.nc.kinect.sample_1
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

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (kinects.Count > 0)
            {
                kinects[0].ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);
                //kinects[0].ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);
                kinects[0].ColorFrameReady += Kinect_ColorFrameReady;               
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

        void Kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame frame = e.OpenColorImageFrame())
            {
                if (frame == null)
                    return;

                byte[] pixelData = new byte[frame.PixelDataLength];
                frame.CopyPixelDataTo(pixelData);
                ColorData.Source = BitmapImage.Create(frame.Width, frame.Height, 96, 96,
                    PixelFormats.Bgr32, null, pixelData,
                    frame.Width * frame.BytesPerPixel);
                //ColorData.Source = BitmapImage.Create(frame.Width, frame.Height, 96, 96,
                //    PixelFormats.Gray16, null, pixelData,
                //    frame.Width * frame.BytesPerPixel);
            }
        }
    }
}
