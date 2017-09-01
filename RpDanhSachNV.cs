using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QuanTriNhanSu
{
    public partial class RpDanhSachNV : DevExpress.XtraReports.UI.XtraReport
    {
        public RpDanhSachNV()
        {
            InitializeComponent();
        }
        public void PrintPre()
        {
            this.ShowPreview();
        }

    }
}
