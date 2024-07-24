using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Camera_App
{
    public partial class Camera_App : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource1;
        private VideoCaptureDevice videoSource2;
        private bool isCamera1Running = false;
        private bool isCamera2Running = false;
        public Camera_App()
        {
            InitializeComponent();
            MaximizeForm();
            InitializeCamera();
        }
        private void MaximizeForm()
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
        }
        private void InitializeCamera()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video sources found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (FilterInfo device in videoDevices)
            {
                comboBox1.Items.Add(device.Name);
            }

            comboBox1.SelectedIndex = 0; // Set default selection to the first device
        }
        private void videoSource1_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Get the latest frame from the camera
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            // Display the frame on the PictureBox
            pictureBox1.Image = bitmap;
        }
        private void videoSource2_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Get the latest frame from the second camera
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            // Display the frame on another PictureBox
            pictureBox2.Image = bitmap; // Assuming pictureBox2 is for the second camera
        }
        private void StartCamera1()
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                videoSource1 = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);
                videoSource1.NewFrame += new NewFrameEventHandler(videoSource1_NewFrame);
                videoSource1.Start();
                isCamera1Running = true;
            }
            else
            {
                MessageBox.Show("No Camera Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void StartCamera2()
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                videoSource2 = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString); // Assume second camera is at index 1
                videoSource2.NewFrame += new NewFrameEventHandler(videoSource2_NewFrame);
                videoSource2.Start();
                isCamera2Running = true;
            }
            else
            {
                MessageBox.Show("No Second Camera Detected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StartCamera1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isCamera1Running)
            {
                videoSource1.SignalToStop();
                videoSource1.WaitForStop();
                isCamera1Running = false;
                pictureBox1.Image = null;
                MessageBox.Show("Camera 1 has been stopped!", "Camera Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartCamera2();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isCamera2Running)
            {
                videoSource2.SignalToStop();
                videoSource2.WaitForStop();
                isCamera2Running = false;
                pictureBox2.Image = null;
                MessageBox.Show("Camera 2 has been stopped!", "Camera Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button3.Enabled = true;
            }
        }
    }
}
