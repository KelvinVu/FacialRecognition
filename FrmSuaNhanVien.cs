using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanTriNhanSu
{
    public partial class FrmSuaNhanVien : Form
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.FacesDatabaseConnectionString.ToString());
        public FrmSuaNhanVien(string idnv)
        {
            InitializeComponent();
            con.Open();
            string sql = "select * from NhanVien where MaNV=" + idnv;
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            txtID.DataBindings.Add("Text", dt, "MaNV");
            txtHo.DataBindings.Add("Text", dt, "HoNV");
            txtTen.DataBindings.Add("Text", dt, "TenNV");
            txtDate.DataBindings.Add("Text", dt, "NgaySinh");
            if (dt.Rows[0]["GioiTinh"].ToString() == "Nam")
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;          
            txtSDT.DataBindings.Add("Text", dt, "SoDienThoai");
            txtDC.DataBindings.Add("Text", dt, "DiaChi");
            txtCV.DataBindings.Add("Text", dt, "ChucVu");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string sex;
            if (radioButton1.Checked==true)
            sex ="Nam";
            else
                sex="Nữ";
            string sql1 = "update NhanVien set HoNV='"+txtHo.Text+"',TenNV='"+txtTen.Text+"',NgaySinh='"+txtDate.Text+"',GioiTinh='"+sex+"',ChucVu='"+txtCV.Text+"',SoDienThoai='"+txtSDT.Text+"',DiaChi='"+txtDC.Text+"' where MaNV='"+txtID.Text+"'";
            SqlCommand com = new SqlCommand(sql1, con);
            com.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Thay đổi thông tin nhân viên thành công!");
            this.Close();
        }

     
    }
}
