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

        DataTable dt_TableSchema = new DataTable();

        DataTable dt_TablePK = new DataTable();

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

                //存放檔案
                string savePath = Path.Combine(CheckerDir, DataBaseName + ".txt");

                Logger.Info("savePath::" + savePath);

                FileStream fileStream = new FileStream(savePath, FileMode.Create);

                fileStream.Close();   //切記開了要關,不然會被佔用而無法修改喔!!!

                Logger.Info("savePath::" + File.Exists(savePath));

                for (int i = 0; i < dt_TableName.Rows.Count; i++)
                {
                    using (StreamWriter sw = new StreamWriter(savePath,true))
                    {
                        string tableName = dt_TableName.Rows[i]["TABLE_NAME"].ToString();

                        sw.WriteLine("Start:"+ tableName);
                        

                        sw.WriteLine("End");
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
