using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;

namespace QuanTriNhanSu
{
    public partial class RpTkThang : DevExpress.XtraReports.UI.XtraReport
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.FacesDatabaseConnectionString.ToString());
        public RpTkThang(DateTime d)
        {
            InitializeComponent();
            xrLabel9.Text = "DANH SÁCH NHÂN VIÊN TRONG THÁNG " + d.Month+"/"+d.Year;
            con.Open();
            string sql = "select MaNV, Ngay, GioVao, GioRa from ChamCong where Month(Ngay)='" + d.Month + "'and Year(Ngay)='" + d.Year + "'";
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            xrLabel5.DataBindings.Add("Text", dt, "MaNV");
            xrLabel6.Text = d.ToString("dd/MM/yyyy");
            xrLabel7.DataBindings.Add("Text", dt, "GioVao");
            xrLabel8.DataBindings.Add("Text", dt, "GioRa");


            // ReportPrintTool printtool = new DevExpress.XtraReports.UI.ReportPrintTool(this);
            //  printtool.ShowPreviewDialog();


        }
        public void PrintPre()
        {
            this.ShowPreview();
        }

    }
}
