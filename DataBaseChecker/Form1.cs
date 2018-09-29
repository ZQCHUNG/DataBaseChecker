using DataBaseChecker.Class;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBaseChecker
{
    public partial class Form1 : Form
    {
        static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DataBaseManager DBManager = new DataBaseManager();

        string ComparedFilePath { get; set; }

        string ConnString { get; set; }

        string DataBaseName { get; set; } // 當檔名

        DataTable dt_TableName = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            log4net.Config.BasicConfigurator.Configure();

            Logger.Info("Init成功");
        }

        private void BtnCompare_Click(object sender, EventArgs e)
        {
            GetCompareDataBaseInfo();
        }

        //取得比對檔案
        private void GetCompareDataBaseInfo()
        {
            OpenFileDialog comparedFile = new OpenFileDialog();

            comparedFile.Filter = "txt files (*.txt)|*.txt";
            comparedFile.Title = "請選擇比對檔";

            if (comparedFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                ComparedFilePath = comparedFile.FileName;

                Logger.Info(string.Format("ComparedFilePath::{0}", ComparedFilePath));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnectDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                ConnString = txtConnString.Text;

                if (string.IsNullOrEmpty(ConnString))
                {
                    MessageBox.Show("尚未填入連線資訊");
                }

                dt_TableName = DBManager.ConnDB(ConnString, SQLManager.Select.GetAllTableName());

                Logger.Info("Table.length::" + dt_TableName.Rows.Count);

                for (int i = 0; i < dt_TableName.Rows.Count; i++)
                {
                    string tableName = dt_TableName.Rows[i]["TABLE_NAME"].ToString();

                    chkListTableDataChecker.Items.Add(tableName);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ex.ToString());

                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool check = !checkBox1.Checked ? false : true;

                for (int i = 0; i < chkListTableDataChecker.Items.Count; i++)
                {
                    chkListTableDataChecker.SetItemChecked(i, check);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ex.ToString());

                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 將目前資料庫狀態寫入文件內
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecordTableInfo_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseName = string.IsNullOrEmpty(txtDatabaseName.Text) ? DateTime.Now.Date.ToString() + "DB" : txtDatabaseName.Text;

                //存放目錄
                string CheckerDir = "Checker/" + DataBaseName + "/" + DateTime.Now.Date.ToString("yyyy/MM/dd");

                if (!Directory.Exists(CheckerDir))
                {
                    Directory.CreateDirectory(CheckerDir);
                }

                for (int i = 0; i < dt_TableName.Rows.Count; i++)
                {
                    DataTable dt_TablePK = new DataTable();

                    DataTable dt_TableSchema = new DataTable();

                    DataTable dt_data = new DataTable();

                    string tableName = dt_TableName.Rows[i]["TABLE_NAME"].ToString();

                    //存放檔案
                    string savePath = Path.Combine(CheckerDir, tableName + ".txt");

                    FileStream fileStream = new FileStream(savePath, FileMode.Create);

                    fileStream.Close();   //切記開了要關,不然會被佔用而無法修改喔!!!

                    Logger.Info(string.Format("savePath::{0},{1}", savePath, File.Exists(savePath)));

                    using (StreamWriter sw = new StreamWriter(savePath, true))
                    {
                        dt_TableSchema = DBManager.ConnDB(ConnString, SQLManager.Select.GetTableSchema(tableName));

                        sw.WriteLine("欄位數量::" + dt_TableSchema.Rows.Count);

                        dt_TablePK = DBManager.ConnDB(ConnString, SQLManager.Select.GetTablePK(tableName));

                        if (dt_TablePK.Rows.Count == 0)
                        {
                            sw.WriteLine(string.Format("Key:{0}", "null"));
                        }
                        else
                        {
                            for (int j = 0; j < dt_TablePK.Rows.Count; j++)
                            {
                                sw.WriteLine(string.Format("Key:{0}", dt_TablePK.Rows[j]["COLUMN_NAME"].ToString()));
                            }
                        }

                        for (int j = 0; j < dt_TableSchema.Rows.Count; j++)
                        {
                            for (int k = 0; k < dt_TableSchema.Columns.Count; k++)
                            {
                                string columnName = dt_TableSchema.Columns[k].ColumnName;

                                sw.WriteLine(string.Format("{0}:{1}", columnName, dt_TableSchema.Rows[j][k].ToString()));
                            }

                            sw.WriteLine();
                        }

                        //若要記錄資料
                        if (chkListTableDataChecker.GetItemChecked(i))
                        {
                            sw.WriteLine("==Data==");

                            dt_data = DBManager.ConnDB(ConnString, SQLManager.Select.GetTableData(tableName));

                            for (int j = 0; j < dt_data.Rows.Count; j++)
                            {
                                for (int k = 0; k < dt_data.Columns.Count; k++)
                                {
                                    string columnName = dt_data.Columns[k].ColumnName;

                                    sw.WriteLine(string.Format("{0}:{1}", columnName, dt_data.Rows[j][k].ToString()));
                                }

                                sw.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ex.ToString());

                MessageBox.Show(ex.Message);
            }
        }
    }
}
