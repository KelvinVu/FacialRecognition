using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanTriNhanSu
{
    public partial class FrmChiTietNhanVien : Form
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.FacesDatabaseConnectionString.ToString());
        public FrmChiTietNhanVien(string idnv)
        {
            InitializeComponent();
            con.Open();
            string sql = "select * from NhanVien where MaNV="+idnv;
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            lbID.DataBindings.Add("Text", dt, "MaNV");
            lbHo.DataBindings.Add("Text", dt, "HoNV");
            lbTen.DataBindings.Add("Text", dt, "TenNV");
            lbNS.DataBindings.Add("Text", dt, "NgaySinh");
            lbGT.DataBindings.Add("Text", dt, "GioiTinh");
            lbSDT.DataBindings.Add("Text", dt, "SoDienThoai");
            lbDiachi.DataBindings.Add("Text", dt, "DiaChi");
            lbCV.DataBindings.Add("Text", dt, "ChucVu");

          //  FileStream f = new FileStream("imgnv.bmp", FileMode.Create);
          //  byte[] buff = (byte[]) dt.Rows[0]["Image"];
          //  f.Write(buff, 0, buff.Length);
           // f.Close();
            pictureBox1.ImageLocation = Application.StartupPath + "/PicNV/id" + idnv + ".bmp";
            
        }
    }
}
