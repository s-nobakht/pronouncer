using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;   // this line added by SN
using System.IO;                // this line added by SN
using NAudio;                   // this line added by SN
using NAudio.Wave;              // this line added by SN

namespace Pronouncer_v01
{
      
    public partial class Form1 : Form
    {
        public DataTable srcDT { get; set; }
        public DataTable dstDT { get; set; }
        public string soundsPath { get; set; }
        public string outputFileName { get; set; }

        public Form1()
        {
            InitializeComponent();
            dbSelection.Items.Add("504");
            dbSelection.Items.Add("TPO_01_10");
            dbSelection.Items.Add("TPO_11_20");
            dbSelection.Items.Add("TPO_21_30");
            dbSelection.Items.Add("TPO_31_40");
            dbSelection.Items.Add("TPO_41_50");            

            comboBox1.Items.Add("English");
            comboBox1.Items.Add("Persian");
            comboBox1.Items.Add("Gap");            
        }

        /// <summary>
        /// Gets a list of the DataRow objects that are the datasources for a DataGridViews selected rows
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static DataRow[] GetSelectedDataRows(DataGridView grid)
        {
            DataRow[] dRows = new DataRow[grid.SelectedRows.Count];
            for (int i = 0; i < grid.SelectedRows.Count; i++)
                dRows[i] = ((DataRowView)grid.SelectedRows[i].DataBoundItem).Row;

            return dRows;
        }

        /// <summary>
        /// move row from one grid to another
        /// </summary>
        public void MoveRows(DataTable src, DataTable dest, DataRow[] rows)
        {
            
            foreach (DataRow row in rows)
            {
                // add to dest                
                dest.Rows.Add(row.ItemArray);

                // remove from src
                src.Rows.Remove(row);
            }
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "datasource=localhost;port=3306;username=root;password=XXXXXX;";
                MySqlConnection myConn = new MySqlConnection(connectionString);
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter();

                

                if (dbSelection.Text == "504")
                {
                    myDataAdapter.SelectCommand = new MySqlCommand("SELECT * FROM pronouncer.terms", myConn);
                }
                else if (dbSelection.Text == "TPO_01_10")
                {
                    myDataAdapter.SelectCommand = new MySqlCommand("SELECT * FROM pronouncer.terms_tpo_1_10", myConn);
                }
                else if (dbSelection.Text == "TPO_11_20")
                {
                    myDataAdapter.SelectCommand = new MySqlCommand("SELECT * FROM pronouncer.terms_tpo_11_20", myConn);
                }
                else if (dbSelection.Text == "TPO_21_30")
                {
                    myDataAdapter.SelectCommand = new MySqlCommand("SELECT * FROM pronouncer.terms_tpo_21_30", myConn);
                }
                else if (dbSelection.Text == "TPO_31_40")
                {
                    myDataAdapter.SelectCommand = new MySqlCommand("SELECT * FROM pronouncer.terms_tpo_31_40", myConn);
                }
                else if (dbSelection.Text == "TPO_41_50")
                {
                    myDataAdapter.SelectCommand = new MySqlCommand("SELECT * FROM pronouncer.terms_tpo_31_40", myConn);
                }
                else
                {
                    MessageBox.Show("You Must Select a Vocabs Collection");
                    return;
                }

                MySqlCommandBuilder cb = new MySqlCommandBuilder(myDataAdapter);
                myConn.Open();
                MessageBox.Show("Connected");

                //DataTable dbDataTable = new DataTable();
                this.srcDT = new DataTable();
                this.dstDT = new DataTable();
                myDataAdapter.Fill(this.srcDT);
                BindingSource bSource = new BindingSource();
                bSource.DataSource = this.srcDT;
                dataGridView1.DataSource = bSource;
                myDataAdapter.Update(this.srcDT);

                this.dataGridView2.DataSource = null;
                this.dataGridView2.Rows.Clear();

                this.dstDT.Columns.Add("id");
                this.dstDT.Columns.Add("english");
                this.dstDT.Columns.Add("persian");
                myConn.Close();                

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["english"].Value.ToString();
                textBox2.Text = row.Cells["persian"].Value.ToString();
                textBox3.Text = row.Cells["id"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MoveRows(this.srcDT, this.dstDT, GetSelectedDataRows(dataGridView1));

            BindingSource srcSource = new BindingSource();
            BindingSource dstSource = new BindingSource();
            srcSource.DataSource = this.srcDT;
            dstSource.DataSource = this.dstDT;
            dataGridView1.DataSource = srcSource;
            dataGridView2.DataSource = dstSource;
            
            /*
            foreach (DataGridViewRow selRow in this.dataGridView1.SelectedRows.OfType<DataGridViewRow>().ToArray())
            {
                this.dataGridView1.Rows.Remove(selRow);
                this.dataGridView2.Rows.Add(selRow);
            }
             * */

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MoveRows(this.dstDT, this.srcDT, GetSelectedDataRows(dataGridView2));

            BindingSource srcSource = new BindingSource();
            BindingSource dstSource = new BindingSource();
            srcSource.DataSource = this.srcDT;
            dstSource.DataSource = this.dstDT;
            dataGridView1.DataSource = srcSource;
            dataGridView2.DataSource = dstSource;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "English")
            {
                this.dataGridView3.Rows.Add("English");
            }
            else if (comboBox1.Text == "Persian")
            {
                this.dataGridView3.Rows.Add("Persian");
            }
            else if (comboBox1.Text == "Gap")
            {
                this.dataGridView3.Rows.Add("Gap");
            }
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*
            DataRow[] dRows = new DataRow[dataGridView3.SelectedRows.Count];
            for (int i = 0; i < dataGridView3.SelectedRows.Count; i++)
                dRows[i] = ((DataRowView)dataGridView3.SelectedRows[i].DataBoundItem).Row;
            */

            Int32 selectedRowCount =  dataGridView3.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                for (int i = 0; i < selectedRowCount; i++)
                {
                    dataGridView3.Rows.RemoveAt(dataGridView3.SelectedRows[0].Index);  
                }
            }
            
            /*
            DataRow[] dRows = new DataRow[dataGridView3.SelectedRows.Count];
            for (int i = 0; i < dataGridView3.SelectedRows.Count; i++)
                dRows[i] = ((DataRowView)dataGridView3.SelectedRows[i].DataBoundItem).Row;

            BindingSource srcSource = new BindingSource();
            srcSource.DataSource = dRows;
            dataGridView3.DataSource = srcSource;
            foreach (DataRow row in dRows)
            {
                // add to dest                
                //dest.Rows.Add(row.ItemArray);

                // remove from src
                srcSource Rows.Remove(row);
            }
             * */
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFdlg = new SaveFileDialog();
            saveFdlg.Filter = "Audio files (*.mp3)|*.mp3|All files (*.*)|*.*";
            string sfdname = saveFdlg.FileName;
            if (saveFdlg.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = saveFdlg.FileName;
                this.outputFileName = saveFdlg.FileName;
            }
        }

        private byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        public void Combine(string[] inputFiles, Stream output)
        {
            foreach (string file in inputFiles)
            {
                Mp3FileReader reader = new Mp3FileReader(file);
                if ((output.Position == 0) && (reader.Id3v2Tag != null))
                {
                    output.Write(reader.Id3v2Tag.RawData, 0, reader.Id3v2Tag.RawData.Length);
                }
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                {
                    output.Write(frame.RawData, 0, frame.RawData.Length);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

            button8.Enabled = false;

            if (string.IsNullOrEmpty(this.soundsPath))
            {
                MessageBox.Show("ERROR: Sound Path is Not Set !");
            }
            else if (string.IsNullOrEmpty(this.outputFileName))
            {
                MessageBox.Show("ERROR: Output File is Not Set !");
            }
            else if(dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show("ERROR: No Term Selected !");
            }
            else if (dataGridView3.Rows.Count == 0)
            {
                MessageBox.Show("ERROR: No Repeating Style Defined !");
            }
            else
            {
                /*
                List<string> files_list = new List<string>();
                System.IO.Stream outputResult = new System.IO.FileStream(this.outputFileName, FileMode.CreateNew);

                for (int rows = 0; rows < dataGridView2.Rows.Count; rows++) // reading term from datagridview
                {

                    if (checkBox1.Checked == true)
                    {
                        files_list.Clear();
                    }
                            
                    string term = dataGridView2.Rows[rows].Cells[1].Value.ToString();

                    for (int rows2 = 0; rows2 < dataGridView3.Rows.Count; rows2++) // reading repeatation mechanism from datagridview
                    {
                        string repeat_term = dataGridView3.Rows[rows2].Cells[0].Value.ToString();

                        if (repeat_term == "English")
                        {
                            files_list.Add(Path.Combine(this.soundsPath, term + ".mp3"));
                        }
                        else if (repeat_term == "Persian")
                        {
                            files_list.Add(Path.Combine(this.soundsPath, term + ".fa.mp3"));
                        }
                        else if (repeat_term == "Gap")
                        {
                            int inTermGap = int.Parse(textBox4.Text);

                            for (int i = 0; i < inTermGap; i++)       // repeating gap
                            {
                                files_list.Add(Path.Combine(this.soundsPath, "___.mp3"));
                            }
                        }
                    }

                    int outTermGap = int.Parse(textBox5.Text);
                    for (int i = 0; i < outTermGap; i++)       // repeating gap
                    {
                        files_list.Add(Path.Combine(this.soundsPath, "___.mp3"));
                    }

                    this.Combine(files_list.ToArray(), outputResult);
                }
                **/
                


                
                Byte[] result = new Byte[0];

                Array.Clear(result, 0, result.Length);

                for (int rows = 0; rows < dataGridView2.Rows.Count; rows++) // reading term from datagridview
                {

                    string term = dataGridView2.Rows[rows].Cells[1].Value.ToString();

                    var englishBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".mp3"));

                    var persianBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".fa.mp3"));

                    var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));

                    for (int rows2 = 0; rows2 < dataGridView3.Rows.Count; rows2++) // reading repeatation mechanism from datagridview
                    {
                        string repeat_term = dataGridView3.Rows[rows2].Cells[0].Value.ToString();

                        if (repeat_term == "English")
                        {
                            result = this.Combine(result, englishBuffer);

                        }
                        else if (repeat_term == "Persian")
                        {
                            result = this.Combine(result, persianBuffer);
                        }
                        else if (repeat_term == "Gap")
                        {
                            int inTermGap = int.Parse(textBox4.Text);
                                
                            for (int i = 0; i < inTermGap; i++)       // repeating gap
                            {
                                result = this.Combine(result, gapBuffer);   
                            }                                
                        }
                    }
                        
                    int outTermGap = int.Parse(textBox5.Text);
                    for (int i = 0; i < outTermGap; i++)       // repeating gap
                    {
                        result = this.Combine(result, gapBuffer);
                    }

                    File.WriteAllBytes(this.outputFileName, result);                 
                }
                 
                                    
                

                //var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));
                
                //string fileName = Path.GetFileNameWithoutExtension(this.outputFileName);              
                
                //using ( var fs = File.OpenWrite(this.outputFileName))
                //var fs = File.Create(this.outputFileName);
                //{

                /*
                using (var fs = new FileStream(this.outputFileName, FileMode.Append))
                {
                    //stream.Write(bytes, 0, bytes.Length);
                    

                    for (int rows = 0; rows < dataGridView2.Rows.Count; rows++) // reading term from datagridview
                    {

                        string term = dataGridView2.Rows[rows].Cells[1].Value.ToString();

                        var englishBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".mp3"));

                        var persianBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".fa.mp3"));

                        var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));

                        for (int rows2 = 0; rows2 < dataGridView3.Rows.Count; rows2++) // reading repeatation mechanism from datagridview
                        {
                            string repeat_term = dataGridView3.Rows[rows2].Cells[0].Value.ToString();

                            if (repeat_term == "English")
                            {
                                //var fs = File.OpenWrite(this.outputFileName);
                                //var englishBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".mp3"));
                                fs.Write(englishBuffer, 0, englishBuffer.Length);
                                fs.Flush();
                                //fs.Close();
                                fs.Write(gapBuffer, 0, gapBuffer.Length);
                                fs.Flush();
                                //var englishBuffer = File.OpenRead(Path.Combine(this.soundsPath, term + ".mp3"));
                                //englishBuffer.CopyTo(fs,(int)englishBuffer.Length);
                                //englishBuffer.Close();                                                                         

                            }
                            else if (repeat_term == "Persian")
                            {
                                //var fs = File.OpenWrite(this.outputFileName);
                                //var persianBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".fa.mp3"));
                                fs.Write(persianBuffer, 0, persianBuffer.Length);
                                fs.Flush();
                                //fs.Close();                                
                                //var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));

                                fs.Write(gapBuffer, 0, gapBuffer.Length);
                                fs.Flush();
                  
                            }
                            else if (repeat_term == "Gap")
                            {
                                int inTermGap = int.Parse(textBox4.Text);
                                //var gapBuffer = File.OpenRead(Path.Combine(this.soundsPath, "___.mp3"));
                                for (int i = 0; i < inTermGap; i++)       // repeating gap
                                {
                                    //fs.Write(gapBuffer, 0, gapBuffer.Length);
                                    //var fs = File.OpenWrite(this.outputFileName);
                                    //var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));
                                    fs.Write(gapBuffer, 0, gapBuffer.Length);
                                    fs.Flush();
                                    //fs.Close();
                                    //gapBuffer.CopyTo(fs, (int)gapBuffer.Length);
                                }
                                //gapBuffer.Close();
                                //fs.Flush();
                            }


                        }

                        //using(var gapBuffer = File.OpenRead(Path.Combine(this.soundsPath, "___.mp3")))
                        //{
                        int outTermGap = int.Parse(textBox5.Text);
                        for (int i = 0; i < outTermGap; i++)       // repeating gap
                        {
                            //fs.Write(gapBuffer, 0, gapBuffer.Length);
                            //gapBuffer.CopyTo(fs, (int)gapBuffer.Length);
                            //var fs = File.OpenWrite(this.outputFileName);
                            //var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));
                            fs.Write(gapBuffer, 0, gapBuffer.Length);
                            fs.Flush();
                            //fs.Close();
                        }
                        //gapBuffer.Close();
                        //fs.Flush();
                        //}


                    }
                    fs.Flush();
                    fs.Close();

                }
                 * */
                    
                /*
                    for (int rows = 0; rows < dataGridView2.Rows.Count; rows++) // reading term from datagridview
                    {

                        string term = dataGridView2.Rows[rows].Cells[1].Value.ToString();
                        
                        //var englishBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".mp3"));
                        
                        //var persianBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".fa.mp3"));

                        for (int rows2 = 0; rows2 < dataGridView3.Rows.Count; rows2++) // reading repeatation mechanism from datagridview
                        {
                            string repeat_term = dataGridView3.Rows[rows2].Cells[0].Value.ToString();

                            if (repeat_term == "English")
                            {
                                var fs = File.OpenWrite(this.outputFileName);
                                var englishBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".mp3"));
                                fs.Write(englishBuffer, 0, englishBuffer.Length);
                                fs.Flush();
                                fs.Close();
                                //fs.Write(gapBuffer, 0, gapBuffer.Length);
                                //var englishBuffer = File.OpenRead(Path.Combine(this.soundsPath, term + ".mp3"));
                                //englishBuffer.CopyTo(fs,(int)englishBuffer.Length);
                                //englishBuffer.Close();         
                       
                                /*
                                fs = File.OpenWrite(this.outputFileName);
                                var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));
                                fs.Write(gapBuffer, 0, gapBuffer.Length);
                                fs.Flush();
                                fs.Close();
                                 * */

                                //var gapBuffer = File.OpenRead(Path.Combine(this.soundsPath, "___.mp3"));
                                //gapBuffer.CopyTo(fs, (int)gapBuffer.Length);
                                //gapBuffer.Close();
                                //fs.Flush();
                /*
                            }
                            else if (repeat_term == "Persian")
                            {
                                var fs = File.OpenWrite(this.outputFileName);
                                var persianBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, term + ".fa.mp3"));
                                fs.Write(persianBuffer, 0, persianBuffer.Length);
                                fs.Flush();
                                fs.Close();     
                                /*
                                fs = File.OpenWrite(this.outputFileName);
                                var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));
                                fs.Write(gapBuffer, 0, gapBuffer.Length);
                                fs.Flush();
                                fs.Close();
                                 * **/
                                
                                /*
                                var persianBuffer = File.OpenRead(Path.Combine(this.soundsPath, term + ".fa.mp3"));
                                persianBuffer.CopyTo(fs, (int)persianBuffer.Length);
                                persianBuffer.Close();
                                var gapBuffer = File.OpenRead(Path.Combine(this.soundsPath, "___.mp3"));
                                gapBuffer.CopyTo(fs, (int)gapBuffer.Length);
                                gapBuffer.Close();
                                //fs.Write(persianBuffer, 0, persianBuffer.Length);
                                //fs.Write(gapBuffer, 0, gapBuffer.Length);
                                //fs.Flush();
                                 * */
                /*
                            }
                            else if (repeat_term == "Gap")
                            {
                                int inTermGap = int.Parse(textBox4.Text);
                                //var gapBuffer = File.OpenRead(Path.Combine(this.soundsPath, "___.mp3"));
                                for (int i = 0; i < inTermGap; i++)       // repeating gap
                                {
                                    //fs.Write(gapBuffer, 0, gapBuffer.Length);
                                    var fs = File.OpenWrite(this.outputFileName);
                                    var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));
                                    fs.Write(gapBuffer, 0, gapBuffer.Length);
                                    fs.Flush();
                                    fs.Close();
                                    //gapBuffer.CopyTo(fs, (int)gapBuffer.Length);
                                }
                                //gapBuffer.Close();
                                //fs.Flush();
                            }


                        }

                        //using(var gapBuffer = File.OpenRead(Path.Combine(this.soundsPath, "___.mp3")))
                        //{
                            int outTermGap = int.Parse(textBox5.Text);
                            for (int i = 0; i < outTermGap; i++)       // repeating gap
                            {
                                //fs.Write(gapBuffer, 0, gapBuffer.Length);
                                //gapBuffer.CopyTo(fs, (int)gapBuffer.Length);
                                var fs = File.OpenWrite(this.outputFileName);
                                var gapBuffer = File.ReadAllBytes(Path.Combine(this.soundsPath, "___.mp3"));
                                fs.Write(gapBuffer, 0, gapBuffer.Length);
                                fs.Flush();
                                fs.Close();
                            }
                            //gapBuffer.Close();
                            //fs.Flush();
                        //}
                        

                    }
                    //fs.Close();
                //}
                 * **/
                               
            }                        

            button8.Enabled = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string[] files = Directory.GetFiles(fbd.SelectedPath);

                textBox7.Text = fbd.SelectedPath;
                this.soundsPath = fbd.SelectedPath;
                MessageBox.Show("Files found: " + files.Length.ToString(), "Message");


            }
        }
    }
}
