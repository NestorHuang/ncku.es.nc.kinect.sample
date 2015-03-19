using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ncku.es.nc.kinect.sample.dll;
using Microsoft.Kinect;


namespace ncku.es.nc.kinect.sample
{
    public partial class Form1 : Form
    {
        List<KinectSensor> kinects = new List<KinectSensor>();
        Sensor Sensers = new Sensor();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnectedKinects_Click(object sender, EventArgs e)
        {            
            kinects = Sensers.ConnetedSensor();
            if (kinects.Count > 0)
            {
                MessageBox.Show(string.Format("連線{0}部Kinect，完成。", kinects.Count.ToString()));
            }
            else
            {
                MessageBox.Show("無可連線之Kinect");
            }
        }

        private void btnAngleChange_Click(object sender, EventArgs e)
        {
            int changeAngle = 0;
            try
            {
                changeAngle = int.Parse(this.txtAngle.Text);                
            }
            catch (Exception)
            {
                MessageBox.Show("請輸入數字");
                txtAngle.Text = "0";
                return;
            }

            if (Sensers.IsConnected())
            {
                string status = Sensers.ChangeAngle(kinects[0], changeAngle);
                if (!String.IsNullOrEmpty(status))
                    MessageBox.Show(status);
            }
            else
                MessageBox.Show("請先連線Kinect");
        }
    }
}
