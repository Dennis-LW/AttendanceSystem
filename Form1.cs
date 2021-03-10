
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AttendanceSystem
{
    public partial class Form1 : Form
    {
        string today;
        public Form1()
        {
            InitializeComponent();
            string fileName = "save/" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

            today = DateTime.Now.ToString("yyyy-MM-dd");

            dataGridView1.ColumnCount = 3; // 定義所需要的行數

            ReadCsvToDataTable(dataGridView1, fileName);

            if (dataGridView1.Rows.Count < 3)
            {
                string[] top = new string[] { "空間名稱", "代號", DateTime.Now.ToString("yyyy/MM/dd") };
                dataGridView1.Rows.Add(top);
                string[] colName = new string[] { "姓名", "進入時間", "離開時間" };
                dataGridView1.Rows.Add(colName);
            }
            
            
            timer1.Interval = 3000;
            SaveTimer.Interval = 10000;
            SaveTimer.Enabled = true;

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
        public void ReadCsvToDataTable(DataGridView dataGridView1,string FileName)
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
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            
            Button btn = sender as Button;
            if(CheckIn.Enabled == false)
            {
                
                bool check = true;
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    string btnName = (string)dataGridView1.Rows[i].Cells[0].Value;
                    if (btnName == btn.Text)
                    {
                        check = false;
                        string leave = (string)dataGridView1.Rows[i].Cells[2].Value;
                        if (leave.Length < 1)
                        {
                            CheckIn.Enabled = true;
                            CheckOut.Enabled = false;
                            State.Text = "狀態：簽退中";

                            dataGridView1.Rows[i].Cells[2].Value = DateTime.Now.ToString("HH:mm");
                            leave = (string)dataGridView1.Rows[i].Cells[2].Value;
                            MsgLabel.Text = "Goodbye~ " + btn.Text + leave.Length;
                            timer1.Enabled = true;
                            
                        }
                        else if(leave.Length > 1)
                        {
                            string message = "已有簽退記錄，要重新簽退？";
                            string caption = "Warning";
                           
                            var result = MessageBox.Show(message, caption,
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

                            
                            if (result == DialogResult.Yes)
                            {
                                CheckIn.Enabled = true;
                                CheckOut.Enabled = false;
                                State.Text = "狀態：簽退中";

                                dataGridView1.Rows[i].Cells[2].Value = DateTime.Now.ToString("HH:mm");
                                MsgLabel.Text = "Goodbye~ " + btn.Text;
                                timer1.Enabled = true;
                            }
                        }
                        
                    }
                }
                if (check)
                {
                    string[] row = new string[] { btn.Text, DateTime.Now.ToString("HH:mm"), "" };
                    dataGridView1.Rows.Add(row);
                    MsgLabel.Text = "Hello! " + btn.Text;
                    timer1.Enabled = true;
                }
            }
            else if(CheckOut.Enabled == false)
            {
                bool NoExist = true;
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    string btnName = (string)dataGridView1.Rows[i].Cells[0].Value;
                    if (btnName == btn.Text)
                    {
                        NoExist = false;
                        string leave = (string)dataGridView1.Rows[i].Cells[2].Value;
                        if (leave.Length < 1)
                        {
                            CheckIn.Enabled = true;
                            CheckOut.Enabled = false;
                            State.Text = "狀態：簽退中";

                            dataGridView1.Rows[i].Cells[2].Value = DateTime.Now.ToString("HH:mm");
                            leave = (string)dataGridView1.Rows[i].Cells[2].Value;
                            MsgLabel.Text = "Goodbye~ " + btn.Text;
                            timer1.Enabled = true;

                        }
                        else if (leave.Length > 1)
                        {
                            string message = "已有簽退記錄，要重新簽退？";
                            string caption = "Warning";

                            var result = MessageBox.Show(message, caption,
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);


                            if (result == DialogResult.Yes)
                            {
                                CheckIn.Enabled = true;
                                CheckOut.Enabled = false;
                                State.Text = "狀態：簽退中";

                                dataGridView1.Rows[i].Cells[2].Value = DateTime.Now.ToString("HH:mm");
                                MsgLabel.Text = "Goodbye~ " + btn.Text;
                                timer1.Enabled = true;
                            }
                        }
                    }
                }
                if (NoExist)
                {
                    CheckOut.Enabled = true;
                    CheckIn.Enabled = false;
                    State.Text = "狀態：簽到中";

                    string[] row = new string[] { btn.Text, DateTime.Now.ToString("HH:mm"), "" };
                    dataGridView1.Rows.Add(row);
                    MsgLabel.Text = "Hello! " + btn.Text;
                    timer1.Enabled = true;
                }

            }
            else
            {
                CheckOut.Enabled = true;
                CheckIn.Enabled = false;
                State.Text = "狀態：簽到中";

                string[] row = new string[] { btn.Text, DateTime.Now.ToString("HH:mm"), "" };
                dataGridView1.Rows.Add(row);
                MsgLabel.Text = "Hello! " + btn.Text;
                timer1.Enabled = true;
            }
            
        }

        private void CheckIn_Click(object sender, EventArgs e)
        {
            if (CheckOut.Enabled == false)
            {
                CheckOut.Enabled = true;
                CheckIn.Enabled = false;
                State.Text = "狀態：簽到中";
            }
            else
            {
                CheckIn.Enabled = false;
                State.Text = "狀態：簽到中";
            }

        }

        private void CheckOut_Click(object sender, EventArgs e)
        {
            if (CheckIn.Enabled == false)
            {
                CheckIn.Enabled = true;
                CheckOut.Enabled = false;
                State.Text = "狀態：簽退中";
            }
            else
            {
                CheckOut.Enabled = false;
                State.Text = "狀態：簽退中";
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MsgLabel.Text = "";
            if (MsgLabel.Text == "")
            {
                timer1.Enabled = false;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (CheckIn.Enabled == false)
                {
                    string[] row = new string[] { textBox1.Text, DateTime.Now.ToString("HH:mm"), "" };
                    dataGridView1.Rows.Add(row);
                    MsgLabel.Text = "Hello! " + textBox1.Text;
                    timer1.Enabled = true;
                }
                else if (CheckOut.Enabled == false)
                {
                    for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                    {
                        string btnName = (string)dataGridView1.Rows[i].Cells[0].Value;
                        if (btnName == textBox1.Text)
                        {
                            dataGridView1.Rows[i].Cells[2].Value = DateTime.Now.ToString("HH:mm");
                            MsgLabel.Text = "Goodbye~ " + textBox1.Text;
                            timer1.Enabled = true;
                        }
                    }

                }
                textBox1.Text = "";
            }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button11.PerformClick();
                // these last two lines will stop the beep sound
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void DataDeleteButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                if(item.Index > 1)
                    dataGridView1.Rows.RemoveAt(item.Index);
            }
        }
        public class Data
        {
            public string Name { get; set; }
            public string EntryTime { get; set; }

        }
        private void DataSaveButton_Click(object sender, EventArgs e)
        {
            SaveCsv();
        }

        private void SaveCsv()
        {
            string[] col = new string[] { "姓名", "進入時間", "離開時間" };
            string fileName = "save/" + today + ".csv";
            ExportDatagridToCsv(dataGridView1, fileName, col, true);
        }
        public static void ExportDatagridToCsv(DataGridView dataGridView1, string FileName, string[] ColumnName, bool HasColumnName)
        {
            string strValue = string.Empty;
            //CSV 匯出的標題 要先塞一樣的格式字串 充當標題
            if (HasColumnName == true)
                strValue = string.Join(",", ColumnName);
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dataGridView1[j, i].Value.ToString()))
                    {
                        if (j > 0)
                            strValue = strValue + "," + dataGridView1[j, i].Value.ToString();
                        else
                        {
                            if (string.IsNullOrEmpty(strValue))
                                strValue = dataGridView1[j, i].Value.ToString();
                            else
                                strValue = strValue + Environment.NewLine + dataGridView1[j, i].Value.ToString();
                        }
                    }
                    else
                    {
                        if (j > 0)
                            strValue = strValue + ",";
                        else
                            strValue = strValue + Environment.NewLine;
                    }
                }

            }
            //存成檔案
            string strFile = FileName;
            if (!string.IsNullOrEmpty(strValue))
            {
                File.WriteAllText(strFile, strValue, Encoding.UTF8);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        Bitmap bitmap;

        private void PrintButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count < 4)
            {
                string message = "目前尚無資料";
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
        // The Print Preview Button
        private void PrintPreview_Click(object sender, EventArgs e)
        {
            PrintHistory printHistory = new PrintHistory();
            printHistory.ShowDialog();
        }
        private void SaveTimer_Tick(object sender, EventArgs e)
        {
            string now = DateTime.Now.ToString("HH");
            //Console.WriteLine(now);
            if (now == "06")
            {
                string[] col = new string[] { "姓名", "進入時間", "離開時間" };

                string fileName = "save/" + today + ".csv";
                ExportDatagridToCsv(dataGridView1, fileName, col, true);

                today = DateTime.Now.ToString("yyyy-MM-dd");

                dataGridView1.Rows.Clear();

                string[] top = new string[] { "空間名稱", "代號", today };
                dataGridView1.Rows.Add(top);
                string[] colName = new string[] { "姓名", "進入時間", "離開時間" };
                dataGridView1.Rows.Add(colName);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveCsv();
            string message = "是否儲存資料？";
            string caption = "程式即將關閉";

            var result = MessageBox.Show(message, caption,
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {
                SaveCsv();
            }
            else if(result == DialogResult.No)
            {
                string message2 = "直接關閉？";
                string caption2 = "程式即將關閉";
                var result2 = MessageBox.Show(message2, caption2,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (result2 == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
