using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace QuanTriNhanSu
{
    public partial class UcTkNV : UserControl
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.FacesDatabaseConnectionString.ToString());
        string sql;
        
        DataTable dt = new DataTable();
        public UcTkNV()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            dateEdit1.Text = DateTime.Now.ToShortDateString();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            RpTkNV TkNV = new RpTkNV(txtID.Text, dateEdit1.DateTime);

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx|Pdf File (.pdf)|*.pdf";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtension = new FileInfo(exportFilePath).Extension;

                    switch (fileExtension)
                    {
                        case ".xls":
                            gridControl1.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gridControl1.ExportToXlsx(exportFilePath);
                            break;
                        case ".pdf":
                            gridControl1.ExportToPdf(exportFilePath);
                            break;
                    }
                    MessageBox.Show("Đã xuất file vào " + saveDialog.FileName + " thành công!");
                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                        catch
                        {
                            MessageBox.Show("Không thể mở file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không thể xuất file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            RpTkNV TkNV = new RpTkNV(txtID.Text, dateEdit1.DateTime);
            gridControl1.ShowPrintPreview();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
                sql = "select MaNV, Ngay, GioVao, GioRa from ChamCong where MaNV='" + txtID.Text + "' and Ngay='" + dateEdit1.DateTime.Date + "'";
            else
            {
                sql = "select MaNV, Ngay, GioVao, GioRa from ChamCong where MaNV='" + txtID.Text + "' and Month(Ngay)='" + dateEdit1.DateTime.Month + "'";
            }

            try
            {
                con.Open();

                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
                gridControl1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
