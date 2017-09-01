using Emgu.CV;
using Emgu.CV.Structure;
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
    public partial class FrmMain : Form
    {
        UcCameraVao cam1;
      
        public FrmMain()
        {
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");

            InitializeComponent();
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            cam1 = new UcCameraVao();
            panelControl1.Controls.Add(cam1);
        }
      
        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            UcCameraRa cam2 = new UcCameraRa();
            panelControl1.Controls.Add(cam2);
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            UcDanhSachNV listEmp = new UcDanhSachNV();
            panelControl1.Controls.Add(listEmp);
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            FrmThemNhanVien add = new FrmThemNhanVien();
            add.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            UcTkNgay empDay = new UcTkNgay();
            panelControl1.Controls.Add(empDay);
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            UcTkThang empMonth = new UcTkThang();
            panelControl1.Controls.Add(empMonth);
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            UcTkNV empSearch = new UcTkNV();
            panelControl1.Controls.Clear();
            panelControl1.Controls.Add(empSearch);
        }
        
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Ngày:  " + DateTime.Now.Date.ToShortDateString() + "  Giờ hiện tại:  " + DateTime.Now.ToLongTimeString() + "  Trạng thái: SẴN SÀNG";
        }

    }
}
