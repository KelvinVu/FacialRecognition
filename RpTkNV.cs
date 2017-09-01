using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;

namespace QuanTriNhanSu
{
    public partial class RpTkNV : DevExpress.XtraReports.UI.XtraReport
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.FacesDatabaseConnectionString.ToString());
        public RpTkNV(string id, DateTime dt)
        {
            InitializeComponent();
            con.Open();
            string sql = "select MaNV, Ngay, GioVao, GioRa from ChamCong where Ngay='" + dt + "'and MaNV='" + id + "'";
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            con.Close();

            xrLabel5.DataBindings.Add("Text", dt1, "MaNV");
            xrLabel6.Text = dt.ToString("dd/MM/yyyy");
            xrLabel7.DataBindings.Add("Text", dt1, "GioVao");
            xrLabel8.DataBindings.Add("Text", dt1, "GioRa");
        }
        public void PrintPre()
        {
            this.ShowPreview();
        }
    }
}
