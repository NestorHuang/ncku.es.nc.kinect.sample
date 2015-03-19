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

namespace ncku.es.nc.kinect.sample_4
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        List<KinectSensor> kinects = new List<KinectSensor>();
        Sensor Sensors = new Sensor();
        KinectAudioSource kinectaudiosource; 

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
            kinects = Sensors.ConnetedSensor();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(kinects.Count>0)
            {
                kinectaudiosource = kinects[0].AudioSource;
                SoundTracking();
            }            
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if(kinects.Count>0)
            {
                Sensors.DisConnetedAllSensor(kinects);
            }            
        }

        void SoundTracking()
        {
            KinectAudioSource audioSource = AudioSourceSetup();

            audioSource.BeamAngleChanged += audioSource_BeamAngleChanged;
            audioSource.SoundSourceAngleChanged += audioSource_SoundSourceAngleChanged;

            audioSource.Start();
        }
        
        KinectAudioSource AudioSourceSetup()
        {
            KinectAudioSource source = kinectaudiosource;
            source.NoiseSuppression = true;
            source.AutomaticGainControlEnabled = true;
            source.BeamAngleMode = BeamAngleMode.Adaptive;
            return source;
        }

        void audioSource_BeamAngleChanged(object sender, BeamAngleChangedEventArgs e)
        {
            string maxmin = " ,最大Beam Angle :" + KinectAudioSource.MaxBeamAngle
                           + " , 最小Beam Angle :" + KinectAudioSource.MinBeamAngle;
            string output = "偵測到Beam Angle :" + e.Angle.ToString() + maxmin;
            SoundInfo.Text = output;
        }

        void audioSource_SoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
        {
            string maxmin = " ,最大Source Angle :" + KinectAudioSource.MaxSoundSourceAngle
                                        + " , 最小Sound Angle :" + KinectAudioSource.MinSoundSourceAngle;
            string output = "偵測到Source Angle :" + e.Angle.ToString()
                        + " , Source Confidence: " + e.ConfidenceLevel.ToString()
                        + maxmin;
            SoundInfo.Text = output;

            //主動取得
            //string output2 = "SoundSourceAngle :" + audiosource.SoundSourceAngle +
            //                                    " , SoundSourceAngleConfidence: " + audiosource.SoundSourceAngleConfidence;
            //Console.WriteLine(output2);
        }
    }
}
