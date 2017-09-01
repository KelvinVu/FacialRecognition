using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Data.SqlClient;

namespace QuanTriNhanSu
{
    public partial class UcCameraVao : UserControl
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.FacesDatabaseConnectionString.ToString());
        Image<Bgr, Byte> currentFrame;
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray_frame = null;
        string name;

        Capture grabber;

        public HaarCascade Face = new HaarCascade("haarcascade_frontalface_alt2.xml");//haarcascade_frontalface_alt_tree.xml");

        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.5, 0.5);

        int NumLabels;

        // Khởi tạo tập huấn luyện
        Classifier_Train Eigen_Recog = new Classifier_Train();

        public UcCameraVao()
        {
            InitializeComponent();
            //Load khuôn mặt được huấn luyện
            if (Eigen_Recog.IsTrained)
            {
                label10.Text = "load thành công";
            }
            else
            {
                label10.Text = "không tìm thấy dữ liệu";
            }
            initialise_capture();

        }
        public void initialise_capture()
        {
            grabber = new Capture();
            grabber.QueryFrame();
            //Khởi tạo sự kiện bắt frame
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
            //Lấy frame hiện tại
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Chuyển thành ảnh xám
            if (currentFrame != null)
            {
                gray_frame = currentFrame.Convert<Gray, Byte>();

                //Xác định khuôn mặt
                MCvAvgComp[][] facesDetected = gray_frame.DetectHaarCascade(Face, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(50, 50));

                //Xử lý khuôn mặt
                foreach (MCvAvgComp face_found in facesDetected[0])
                {
                    result = currentFrame.Copy(face_found.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    //vẽ khung cho khuôn mặt tìm thấy
                    currentFrame.Draw(face_found.rect, new Bgr(Color.Red), 2);

                    if (Eigen_Recog.IsTrained)
                    {
                        //định danh cho khuôn mặt tìm thấy
                        name = Eigen_Recog.Recognise(result);
                        int match_value = (int)Eigen_Recog.Get_Eigen_Distance;
                        currentFrame.Draw(name + " ", ref font, new Point(face_found.rect.X - 2, face_found.rect.Y - 2), new Bgr(Color.LightGreen));
                        if (name != "Unknown")
                            ShowInfo(name.Substring(name.IndexOf("_") + 1, name.Length - 1 - name.IndexOf("_")));
                    }
                }
                // đưa lên picturebox
                imageBox1.Image = currentFrame.ToBitmap();
            }
        }
        private void ShowInfo(string id)
        {
            con.Open();
            string sql = "select * from NhanVien where MaNV=" + id;
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            txtID.DataBindings.Clear();
            txtTen.DataBindings.Clear();
            txtChucvu.DataBindings.Clear();
            txtDiachi.DataBindings.Clear();
            txtGioitinh.DataBindings.Clear();
            txtNgaysinh.DataBindings.Clear();
            txtSoDT.DataBindings.Clear();

            txtID.DataBindings.Add("Text", dt, "MaNV");
            txtTen.DataBindings.Add("Text", dt, "TenNV");
            txtNgaysinh.DataBindings.Add("Text", dt, "NgaySinh");
            txtGioitinh.DataBindings.Add("Text", dt, "GioiTinh");
            txtSoDT.DataBindings.Add("Text", dt, "SoDienThoai");
            txtDiachi.DataBindings.Add("Text", dt, "DiaChi");
            txtChucvu.DataBindings.Add("Text", dt, "ChucVu");
            pictureBox1.ImageLocation = Application.StartupPath + "/TrainedFaces/face_" + name + ".jpg";
            label10.Text = DateTime.Now.ToString();

            string sql1 = "if not exists(select MaNV from ChamCong where MaNV='" + txtID.Text + "' and Ngay='" + DateTime.Now.ToShortDateString() + "') insert into ChamCong (MaNV,Ngay,GioVao) values ('" + txtID.Text + "','" + DateTime.Now + "','" + DateTime.Now.TimeOfDay + "')";
            com = new SqlCommand(sql1, con);
            com.ExecuteNonQuery();
            con.Close();
        }
    }
}
