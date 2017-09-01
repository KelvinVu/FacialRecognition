using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;

namespace QuanTriNhanSu
{
    public partial class RpTkNgay : DevExpress.XtraReports.UI.XtraReport
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.FacesDatabaseConnectionString.ToString());
        public RpTkNgay(DateTime d)
        {
            InitializeComponent();
            
            xrLabel9.Text = "DANH SÁCH NHÂN VIÊN TRONG NGÀY " + d.ToString("dd/MM/yyyy");
            con.Open();
            string sql = "select MaNV, Ngay, GioVao, GioRa from ChamCong where Ngay='" + d + "'";
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            da.Fill(dt);
            con.Close();
            xrTableCell5.DataBindings.Add("Text", dt, "MaNV");
            xrTableCell6.Text = d.ToString("dd/MM/yyyy");
            xrTableCell8.DataBindings.Add("Text", dt, "GioVao");
            xrTableCell9.DataBindings.Add("Text", dt, "GioRa");

        }
        public void PrintPre()
        {
            this.ShowPreview();
        }

    }
}
