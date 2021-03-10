using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AttendanceSystem
{
    public partial class PrintHistory : Form
    {
        public PrintHistory()
        {
            InitializeComponent();

            List<string> myList = new List<string>();

            // 執行檔路徑下的 MyDir 資料夾
            string folderName = System.Windows.Forms.Application.StartupPath + @"\save";

            // 取得資料夾內所有檔案
            foreach (string fname in System.IO.Directory.GetFiles(folderName))
            {
                string[] sArray = fname.Split('\\');
                
                myList.Add(sArray[sArray.Length-1]);
            }

            dataGridView2.ColumnCount = 1;
            dataGridView2.Columns[0].Name = "檔案名稱";
            dataGridView2.Columns["檔案名稱"].ReadOnly = true;
            for (int i = 0; i < myList.Count; i++)
            {
                dataGridView2.Rows.Add(myList[i]);
            }

        }
        public static List<string> ReadCsvToList(string FileName)
        {
            FileInfo fi = new FileInfo(FileName);
            if (fi.Exists)
            {
                List<string> result = new List<string>();
                int count = 0;
                using (StreamReader sr = new StreamReader(FileName))
                {
                    while (sr.Peek() >= 0)
                    {
                        if (count != 0)
                            result.Add(sr.ReadLine());
                        count++;
                    }

                }
                return result;
            }
            else return null;
        }
        public void ReadCsvToDataTable(DataGridView dataGridView1, string FileName)
        {
            List<string> Input = ReadCsvToList(FileName);
            if (Input != null)
            {
                string[] sep = new string[] { "," };

                for (int j = 1; j < Input.Count; j++)
                {
                    string[] valuetemp = Input[j].Split(sep, StringSplitOptions.None);
                    dataGridView1.Rows.Add(valuetemp);
                }
            }
            else
            {
                MessageBox.Show("檔案不存在！");
            }
        }
        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string year = TextBoxYear.Text;
            string month = TextBoxMonth.Text;
            string day = TextBoxDay.Text;
            if (year.Length < 4)
            {
                TextBoxYear.Text += btn.Text;
            }
            else if(month.Length < 2)
            {
                TextBoxMonth.Text += btn.Text;
            }
            else if (day.Length < 2)
            {
                TextBoxDay.Text += btn.Text;
            }
            
        }
        private void ClearButton_Click(object sender, EventArgs e)
        {
            TextBoxYear.Text = "";
            TextBoxMonth.Text = "";
            TextBoxDay.Text = "";
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            string year = TextBoxYear.Text;
            string month = TextBoxMonth.Text;
            string day = TextBoxDay.Text;

            string fileName = "save/" + year + "-" + month + "-" + day + ".csv";
            Debug.WriteLine(fileName);
            dataGridView1.ColumnCount = 3; // 定義所需要的行數

            ReadCsvToDataTable(dataGridView1, fileName);
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        Bitmap bitmap;

        private void PrintButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
            {
                string message = "尚未開啟檔案";
                string caption = "錯誤";

                var result = MessageBox.Show(message, caption,
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
            else
            {
                //Resize DataGridView to full height.
                int height = dataGridView1.Height;
                dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height;

                //Create a Bitmap and draw the DataGridView on it.
                bitmap = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
                dataGridView1.DrawToBitmap(bitmap, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));

                //Resize DataGridView back to original height.
                dataGridView1.Height = height;

                string message = "確定列印？";
                string caption = "列印";

                var result = MessageBox.Show(message, caption,
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    printDocument1.Print();
                }
            }
            

        }
        private void PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            //Print the contents.
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
        private void PrintHistory_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.Rows.Clear();
            string fileName = "save/" + dataGridView2.CurrentCell.Value;
            Debug.WriteLine(fileName);
            if (fileName != "save/")
            {
                dataGridView1.ColumnCount = 3; // 定義所需要的行數

                ReadCsvToDataTable(dataGridView1, fileName);
            }
            
        }
    }
}
