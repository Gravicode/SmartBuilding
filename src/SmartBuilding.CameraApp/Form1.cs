using System.Text.RegularExpressions;
using System;
using Emgu.CV;
using SmartBuilding.Models;

namespace SmartBuilding.CameraApp
{
    public partial class Form1 : Form
    {
        bool IsCapturing = false;
        Emgu.CV.VideoCapture capture;
        Mat mat;
        Bitmap image;
        CancellationTokenSource token;

        String faceFileName = "haarcascade_frontalface_default.xml";
        List<Rectangle> faces = new();
        CascadeClassifier face;
        public Form1()
        {
            InitializeComponent();
            capture = new();
            this.FormClosing += Form1_FormClosing;
            BtnStop.Click += (a, b) =>
            {
                if (IsCapturing)
                {
                    token.Cancel();
                    IsCapturing = false;
                    BtnStart.Enabled = true;
                    BtnStop.Enabled = false;
                }
            };
            this.BtnStart.Click += (a, b) =>
            {
                if (IsCapturing)
                {
                    MessageBox.Show("Is already started.");
                    return;
                }
                face = new CascadeClassifier(faceFileName);
                token = new();
                Thread thread = new Thread(new ThreadStart(() => Start(token.Token)));
                thread.Start();
                IsCapturing = true;
                BtnStart.Enabled = false;
                BtnStop.Enabled = true;
            };
        }

        HttpClient client = new();
        const string PushImageApiUrl = "https://localhost:44394/api/cctv/sendimage";
        async Task<bool> PushImageToCloud(string CctvName, byte[] ImageData)
        {
            try
            {
                var info = new CCTVImage() { CctvName = CctvName, ImageBytes = ImageData, CreatedDate = DateTime.Now };
                var json = System.Text.Json.JsonSerializer.Serialize(info);
                var hasil = await client.PostAsync(PushImageApiUrl, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                if (hasil.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{DateTime.Now} => push image dari {CctvName} sukses");
                    return true;
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now} => push image dari {CctvName} gagal, {await hasil.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }
        public static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        byte[] CloudImg;
        private async void Start(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested) break;
                try
                {
                    if (image != null) image.Dispose();
                    if (mat != null) mat.Dispose();

                    mat = capture.QueryFrame();
                    image = mat.ToBitmap();
                    CloudImg = ImageToByte2(image);

                    faces.Clear();
                    /*
                    //detect face
                    using (UMat ugray = new UMat())
                    {
                        CvInvoke.CvtColor(mat, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                        //normalizes brightness and increases contrast of the image
                        CvInvoke.EqualizeHist(ugray, ugray);

                        //Detect the faces  from the gray scale image and store the locations as rectangle                   
                        Rectangle[] facesDetected = face.DetectMultiScale(
                           ugray, 1.1, 10, new Size(20, 20));

                        faces.AddRange(facesDetected);
                    }
                    */
                    this.pictureBox1?.Invoke((MethodInvoker)delegate
                    {
                        // Running on the UI thread
                        pictureBox1.Image = image;
                    });

                    if (ChkPush.Checked)//&& faces.Count>0
                        await PushImageToCloud("cctv-1", CloudImg);
                    //image.Save(Application.StartupPath + "\\img.jpg");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //MessageBox.Show(ex.Message);
                }
                //finally
                //{
                //    if (image != null) image.Dispose();
                //    if (mat != null) mat.Dispose();
                //}
                Thread.Sleep(200);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsCapturing) token.Cancel();
            Thread.Sleep(500);
            if (capture != null) capture.Dispose();
        }
    }
}