using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Threading;

namespace QuanTriNhanSu
{
    public partial class FrmThemNhanVien : Form
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.FacesDatabaseConnectionString.ToString());
        
        Capture grabber;
       
        Image<Bgr, Byte> currentFrame;
        Image<Gray, byte> result = null;
        Image<Gray, byte> gray_frame = null;

        //Classifier
        public HaarCascade Face = new HaarCascade("haarcascade_frontalface_alt2.xml");

        //Lưu hình ảnh
        List<Image<Gray, byte>> ImagestoWrite = new List<Image<Gray, byte>>();
        EncoderParameters ENC_Parameters = new EncoderParameters(1);
        EncoderParameter ENC = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
        ImageCodecInfo Image_Encoder_JPG;
        string facename;

        //Lưu file XAML
        List<string> NamestoWrite = new List<string>();
        List<string> NamesforFile = new List<string>();
        XmlDocument docu = new XmlDocument();
        

        public FrmThemNhanVien()
        {
            InitializeComponent();
            ENC_Parameters.Param[0] = ENC;
            Image_Encoder_JPG = GetEncoder(ImageFormat.Jpeg);
            initialise_capture();
        }
        private void Training_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop_capture();
        }
        
        public void initialise_capture()
        {
            grabber = new Capture();
            grabber.QueryFrame();
            //Khởi tạo sự kiện
            Application.Idle += new EventHandler(FrameGrabber);
        }
        private void stop_capture()
        {
            Application.Idle -= new EventHandler(FrameGrabber);
            if (grabber != null)
            {
                grabber.Dispose();
            }
        }

        //Xử lý frame
        void FrameGrabber(object sender, EventArgs e)
        {
            //lấy frame
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //chuyển thành ảnh xám
            if (currentFrame != null)
            {
                gray_frame = currentFrame.Convert<Gray, Byte>();

                //xác định khuôn mặt
                MCvAvgComp[][] facesDetected = gray_frame.DetectHaarCascade(Face, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

                foreach (MCvAvgComp face_found in facesDetected[0])
                {
                    result = currentFrame.Copy(face_found.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    result._EqualizeHist();
                    imgNV.Image = result.ToBitmap();
                    //vẽ khung vuông quanh khuôn mặt
                    currentFrame.Draw(face_found.rect, new Bgr(Color.Red), 2);

                }
                imgCam.Image = currentFrame.ToBitmap();
            }
        }

        //Lưu dữ liệu
        private bool save_training_data(Image face_data)
        {
            try
            {
                Random rand = new Random();
                facename = "face_" + XuLyDuLieu.LoaiBoDauTiengViet(txtTen.Text) + "_" + rand.Next().ToString() + ".jpg";
                bool file_create = true;                
                while (file_create)
                {

                    if (!File.Exists(Application.StartupPath + "/TrainedFaces/" + facename))
                    {
                        file_create = false;
                    }
                    else
                    {
                        facename = "face_" + XuLyDuLieu.LoaiBoDauTiengViet(txtTen.Text) + "_" + rand.Next().ToString() + ".jpg";
                    }
                }


                if (Directory.Exists(Application.StartupPath + "/TrainedFaces/"))
                {
                    face_data.Save(Application.StartupPath + "/TrainedFaces/" + facename, ImageFormat.Jpeg);
                }
                else
                {
                    Directory.CreateDirectory(Application.StartupPath + "/TrainedFaces/");
                    face_data.Save(Application.StartupPath + "/TrainedFaces/" + facename, ImageFormat.Jpeg);
                }
                if (File.Exists(Application.StartupPath + "/TrainedFaces/TrainedLabels.xml"))
                {
                    //File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", txtTen.Text + "\n\r");
                    bool loading = true;
                    while (loading)
                    {
                        try
                        {
                            docu.Load(Application.StartupPath + "/TrainedFaces/TrainedLabels.xml");
                            loading = false;
                        }
                        catch
                        {
                            docu = null;
                            docu = new XmlDocument();
                            Thread.Sleep(10);
                        }
                    }

                    //lấy root 
                    XmlElement root = docu.DocumentElement;

                    XmlElement face_D = docu.CreateElement("FACE");
                    XmlElement name_D = docu.CreateElement("NAME");
                    XmlElement file_D = docu.CreateElement("FILE");

                    //thêm giá trị mỗi node
                    //name.Value = textBoxName.Text;
                    //age.InnerText = textBoxAge.Text;
                    //gender.InnerText = textBoxGender.Text;
                    name_D.InnerText = facename.Substring(facename.IndexOf("_") + 1, facename.Length - 5 - facename.IndexOf("_"));
                    file_D.InnerText = facename;

                    //thêm tên và tên file
                    //person.Attributes.Append(name);
                    face_D.AppendChild(name_D);
                    face_D.AppendChild(file_D);

                    //thêm một định danh mới
                    root.AppendChild(face_D);

                    //Lưu
                    docu.Save(Application.StartupPath + "/TrainedFaces/TrainedLabels.xml");
                    //XmlElement child_element = docu.CreateElement("FACE");
                    //docu.AppendChild(child_element);
                    //docu.Save("TrainedLabels.xml");
                }
                else
                {
                    FileStream FS_Face = File.OpenWrite(Application.StartupPath + "/TrainedFaces/TrainedLabels.xml");
                    using (XmlWriter writer = XmlWriter.Create(FS_Face))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Faces_For_Training");

                        writer.WriteStartElement("FACE");
                        writer.WriteElementString("NAME", facename.Substring(facename.IndexOf("_") + 1, facename.Length - 5 - facename.IndexOf("_")));
                        writer.WriteElementString("FILE", facename);
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                    FS_Face.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
     
        private void FormAddEmployees_Load(object sender, EventArgs e)
        {
            rdbtnNam.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtDate.DateTime>=DateTime.Now)
            {
                MessageBox.Show("Ngày tháng không phù hợp!");
                return;
            }
            if (txtHo.Text == "" || txtChucvu.Text == "" || txtDate.Text == "" || txtDiachi.Text == "" || txtSDT.Text == "" || txtTen.Text == "")
            {
                MessageBox.Show("Thông tin chưa đầy đủ! Vui lòng nhập đầy đủ thông tin nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                save_training_data(imgNV.Image);
                Add();
                this.Close();
                MessageBox.Show("Thêm Nhân Viên Thành Công","Thông Báo",MessageBoxButtons.OK);

            }
        }

        private void Add()
        {
            string sex="Nam";
            if(rdbtnNu.Checked==true)
            {
                sex="Nữ";
            }
            con.Open();
            string sql = "insert into NhanVien(MaNV,HoNV,TenNV,NgaySinh,GioiTinh,ChucVu,SoDienThoai,DiaChi) values ('"+facename.Substring(facename.LastIndexOf("_")+1,facename.Length-5-facename.LastIndexOf("_")) 
                +"','"+txtHo.Text+"','"
                +txtTen.Text+"','"
                +txtDate.DateTime.ToShortDateString()
                +"','"+sex.ToString()+"','"
                +txtChucvu.Text+"','"
                +txtSDT.Text+"','"
                +txtDiachi.Text+"')";
            SqlCommand com = new SqlCommand(sql, con);
            com.ExecuteNonQuery();
            con.Close();
        }
    }
}
