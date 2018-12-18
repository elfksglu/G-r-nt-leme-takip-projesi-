using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO.Ports;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCapTureDevices;
        private VideoCaptureDevice Finalvideo;
        public Form1()
        {
            InitializeComponent();
        }
        int R;
        int G;
        int B;
        private void Form1_Load(object sender, EventArgs e)
        {
            VideoCapTureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in VideoCapTureDevices)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.DataSource = SerialPort.GetPortNames();
           serialPort1.PortName = comboBox2.SelectedItem.ToString();
           serialPort1.BaudRate = 9600;
           serialPort1.Open();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Finalvideo = new VideoCaptureDevice(VideoCapTureDevices[comboBox1.SelectedIndex].MonikerString);
            Finalvideo.NewFrame += Finalvideo_NewFrame;
            Finalvideo.Start();
        }

        private void Finalvideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Console.WriteLine(image.Width + " -- " + image.Height);
            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;

            if (radioButtonKırmızı.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(215, 0, 0));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                Nesnetespit(image1);
            }
            if (radioButtonMavi.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(30, 144, 215));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                Nesnetespit(image1);
            }
            if (radioButtonYeşil.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(0, 215, 0));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                Nesnetespit(image1);
            }
            if (radioButtonElleKontrol.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(R, G, B));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                Nesnetespit(image1);
            }
        }
        public void Nesnetespit(Bitmap image)
        {
            Grayscale grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinWidth = 5;
            blobCounter.MinHeight = 5;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;
            blobCounter.ProcessImage(image);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            Blob[] blobs = blobCounter.GetObjectsInformation();
            pictureBox2.Image = image;
            foreach (Rectangle recs in rects)
            {
                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];
                    int objectX = objectRect.X + (objectRect.Width / 2);
                    int objectY = objectRect.Y + (objectRect.Height / 2);

                    if (objectX < 213 && objectY < 160)
                    {
                        serialPort1.Write("1");
                    }
                    if ((213 < objectX && objectX < 426) && objectY < 160)

                    {
                        serialPort1.Write("2");
                    }
                    if (426 < objectX && objectX < 640 && objectY < 160)
                    {
                        serialPort1.Write("3");
                    }
                    if (objectX < 213 && (160 < objectY && objectY < 320))
                    {
                        serialPort1.Write("4");
                    }
                    if ((213 < objectX && objectX < 426) && (160 < objectY && objectY < 320))
                    {
                        serialPort1.Write("5");
                    }
                    if ((426 < objectX && objectX < 639) & (160 < objectY && objectY < 320))
                    {
                        serialPort1.Write("6");
                    }
                    if ( objectX <213 && (320 < objectY && objectY < 480))
                    {
                        serialPort1.Write("7");
                    }
                    if ((123 < objectX && objectX < 426) && (320 < objectY && objectY < 480))
                    {
                        serialPort1.Write("8");
                    }
                    if ((426 < objectX && objectX < 639) && (320 < objectY && objectY < 480))
                    {
                        serialPort1.Write("9");
                    }
                }



            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Finalvideo.IsRunning)
            {
                Finalvideo.Stop();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            R = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

            B = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            G = trackBar3.Value;
        }
    }
    }

            

