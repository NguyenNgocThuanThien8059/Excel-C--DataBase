using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using Excel = Microsoft.Office.Interop.Excel;
using ImportDSKH.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace ImportDSKH
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        Model1 context = new Model1();
        private void ImportDuLieuExcel(string path)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[0];
                for(int i2 = excelWorksheet.Dimension.Start.Row + 1; i2 <=  excelWorksheet.Dimension.End.Row; i2++) 
                {
                    int index = dataGridView1.Rows.Add();
                    for (int j = excelWorksheet.Dimension.Start.Column; j < excelWorksheet.Dimension.End.Column; j++)
                    {
                        dataGridView1.Rows[index].Cells[j].Value = excelWorksheet.Cells[i2, j].Value.ToString();
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = " Import Excel ";
            openFileDialog.Filter = "Excel (*.xlsx) | *.xlsx";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImportDuLieuExcel(openFileDialog.FileName);
                textBox1.Text = openFileDialog.FileName;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        SqlConnection conn = new SqlConnection();
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["KHList"].ConnectionString;
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "ThemVaSua";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@MaKH", SqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[1].Value;
                        cmd.Parameters.Add("@Ten", SqlDbType.NVarChar).Value = dataGridView1.Rows[i].Cells[2].Value;
                        cmd.Parameters.Add("@NgaySinh", SqlDbType.DateTime).Value = dataGridView1.Rows[i].Cells[3].Value;
                        cmd.Parameters.Add("@SDT", SqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[4].Value;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[5].Value;
                        cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = dataGridView1.Rows[i].Cells[6].Value;
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    transaction.Commit();
                }
                catch 
                {
                    MessageBox.Show(" Something went wrong ");
                    transaction.Rollback();
                }
            }
            
        }
    }
}
