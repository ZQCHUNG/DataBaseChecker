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

        string CheckerDir = "Checker/";

        string RecorderDir = "Record/";

        string ConnectStringInfoFileName = "ConnectionInfo.txt";

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

        #region 建立記錄

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

                chkListTableDataChecker.Items.Clear();

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
            checkedALL(chkListTableDataChecker, checkBox1);
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

                //建立檢查目錄
                string checkerDir = CheckerDir + DataBaseName;

                string connStringInfoPath = Path.Combine(checkerDir, "ConnectionInfo.txt");

                if (!Directory.Exists(checkerDir))
                {
                    Directory.CreateDirectory(checkerDir);
                }

                FileStream fileStreamConnectionInfo = new FileStream(connStringInfoPath, FileMode.Create);

                fileStreamConnectionInfo.Close();

                using (StreamWriter sw = new StreamWriter(connStringInfoPath))
                {
                    sw.WriteLine(ConnString);
                }

                //存放目錄
                string recordDir = RecorderDir + DataBaseName + "/" + DateTime.Now.Date.ToString("yyyy/MM/dd");

                if (Directory.Exists(recordDir))
                {
                    Directory.Delete(recordDir, true);
                }

                Directory.CreateDirectory(recordDir);

                for (int i = 0; i < dt_TableName.Rows.Count; i++)
                {
                    DataTable dt_TablePK = new DataTable();

                    DataTable dt_TableSchema = new DataTable();

                    DataTable dt_data = new DataTable();

                    string tableName = dt_TableName.Rows[i]["TABLE_NAME"].ToString();

                    //存放檔案
                    string savePath = Path.Combine(recordDir, tableName + ".txt");

                    CreateFile(savePath);

                    Logger.Info(string.Format("savePath::{0},{1}", savePath, File.Exists(savePath)));

                    using (StreamWriter sw = new StreamWriter(savePath, true))
                    {
                        dt_TableSchema = DBManager.ConnDB(ConnString, SQLManager.Select.GetTableSchema(tableName));

                        dt_TablePK = DBManager.ConnDB(ConnString, SQLManager.Select.GetTablePK(tableName));

                        sw.WriteLine("TableKey:");

                        if (dt_TablePK.Rows.Count != 0)
                        {
                            for (int j = 0; j < dt_TablePK.Rows.Count; j++)
                            {
                                sw.WriteLine(dt_TablePK.Rows[j]["COLUMN_NAME"].ToString());
                            }
                        }

                        sw.WriteLine("TableSchema:");

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
                            sw.WriteLine("==冷資料記錄，請買錸德(2349)==");

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

                label5.Text = "完成";
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ex.ToString());

                MessageBox.Show(ex.Message);

                label5.Text = "失敗";
            }
        }

        private static void CreateFile(string savePath)
        {
            Logger.Info(string.Format("CreateFile.savePath::{0}", savePath));

            FileStream fileStream = new FileStream(savePath, FileMode.Create);

            fileStream.Close();
        }

        #endregion

        #region 比對部分

        private void BtnGetCompareDir_Click(object sender, EventArgs e)
        {
            GetComparisonDir();
        }

        //取得目錄檔案
        private void GetComparisonDir()
        {
            try
            {
                chkListDir.Items.Clear();

                string[] dirs = Directory.GetDirectories(CheckerDir);

                Logger.Info("dirs.length::" + dirs.Length);

                for (int i = 0; i < dirs.Length; i++)
                {
                    string dirName = dirs[i].Replace(CheckerDir, "");

                    chkListDir.Items.Add(dirName);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ex.ToString());

                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkedALL(chkListDir, checkBox2);
        }

        private void btnCompared_Click(object sender, EventArgs e)
        {
            string dirName = "";

            //連線資訊
            string connStringInfoFilePath = "";

            string impactFilePath = "diffFile.txt";

            try
            {
                for (int i = 0; i < chkListDir.Items.Count; i++)
                {
                    if (chkListDir.GetItemChecked(i))
                    {
                        dirName = chkListDir.Items[i].ToString();

                        Logger.Info("準備比對::" + dirName);

                        connStringInfoFilePath = Path.Combine(CheckerDir, dirName, ConnectStringInfoFileName);

                        Logger.Info("連線資訊檔案位置::" + connStringInfoFilePath);

                        if (!File.Exists(connStringInfoFilePath))
                        {
                            Logger.Info("無連線資訊檔案");

                            continue;
                        }

                        using (StreamReader sr = new StreamReader(connStringInfoFilePath))
                        {
                            ConnString = sr.ReadLine();
                        }

                        RecorderDir = Path.Combine(RecorderDir, dirName);

                        var dir = (from d in new System.IO.DirectoryInfo(RecorderDir).GetDirectories()
                                   select d).ToList().OrderByDescending(d => d.CreationTime).Take(1).SingleOrDefault();

                        while (dir != null)
                        {
                            RecorderDir = Path.Combine(RecorderDir, dir.Name);

                            dir = (from d in new System.IO.DirectoryInfo(RecorderDir).GetDirectories()
                                   select d).ToList().OrderByDescending(d => d.CreationTime).Take(1).SingleOrDefault();
                        }

                        impactFilePath = Path.Combine(Path.Combine(CheckerDir, dirName, impactFilePath));

                        CreateFile(impactFilePath);

                        Logger.Info("比對位置::" + RecorderDir);

                        dt_TableName = DBManager.ConnDB(ConnString, SQLManager.Select.GetAllTableName());

                        using (StreamWriter sw = new StreamWriter(impactFilePath))
                        {
                            //比對Table是否一致

                            //舊檔案名稱
                            List<string> oldTableName = new List<string>();

                            //目前檔案名稱
                            List<string> currentTableName = new List<string>();

                            string[] tmp_files = System.IO.Directory.GetFiles(RecorderDir);

                            foreach (var o in tmp_files)
                            {
                                oldTableName.Add(o.Replace(RecorderDir + "\\", "").Replace(".txt", ""));
                            }

                            for (int j = 0; j < dt_TableName.Rows.Count; j++)
                            {
                                currentTableName.Add(dt_TableName.Rows[j]["TABLE_NAME"].ToString());
                            }

                            var diff_delete_list = oldTableName.Except(currentTableName);

                            foreach (var item in diff_delete_list)
                            {
                                sw.WriteLine("已刪除或變更Table:" + item);
                            }

                            var diff_add_list = currentTableName.Except(oldTableName);

                            foreach (var item in diff_add_list)
                            {
                                sw.WriteLine("新增Table:" + item);
                            }

                            var intersect_tablename = oldTableName.Intersect(currentTableName);

                            //比對PK是否一致

                            //比對Schema是否一致

                            //比對資料是否一致

                            foreach (var tableName in intersect_tablename)
                            {
                                DataTable dt_TablePK = new DataTable();

                                DataTable dt_TableSchema = new DataTable();

                                DataTable dt_data = new DataTable();


                                using (StreamReader sr = new StreamReader(Path.Combine(RecorderDir, tableName + ".txt")))
                                {
                                    sw.WriteLine();

                                    sw.WriteLine("開始處理:" + tableName);

                                    #region PK部分

                                    dt_TablePK = DBManager.ConnDB(ConnString, SQLManager.Select.GetTablePK(tableName));

                                    List<string> currentPK = new List<string>();

                                    for (int j = 0; j < dt_TablePK.Rows.Count; j++)
                                    {
                                        currentPK.Add(dt_TablePK.Rows[j]["COLUMN_NAME"].ToString());
                                    }

                                    string line = "";

                                    List<string> old_key_list = new List<string>();

                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        if (line == "TableKey:")
                                        {
                                            continue;
                                        }

                                        if (line == "TableSchema:")
                                        {
                                            break;
                                        }

                                        old_key_list.Add(line);
                                    }

                                    foreach (var item in old_key_list.Except(currentPK))
                                    {
                                        sw.WriteLine("已刪除PK:" + item);
                                    }

                                    foreach (var item in currentPK.Except(old_key_list))
                                    {
                                        sw.WriteLine("已新增PK:" + item);
                                    }

                                    #endregion

                                    #region Scahema部分

                                    dt_TableSchema = DBManager.ConnDB(ConnString, SQLManager.Select.GetTableSchema(tableName));

                                    List<string> currentScahema = new List<string>();

                                    for (int j = 0; j < dt_TableSchema.Rows.Count; j++)
                                    {
                                        currentScahema.Add(dt_TableSchema.Rows[j]["COLUMN_NAME"].ToString());
                                    }

                                    List<DataBaseSchema> oldSchema_List = new List<DataBaseSchema>();

                                    //當Key查詢用
                                    List<string> oldSchema_SearchKey = new List<string>();

                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        if (line == "==冷資料記錄，請買錸德(2349)==")
                                        {
                                            break;
                                        }

                                        if (line.StartsWith("COLUMN_NAME:"))
                                        {
                                            DataBaseSchema oldSchemaItem = new DataBaseSchema();

                                            line = line.Remove(0, "COLUMN_NAME:".Length);

                                            oldSchemaItem.COLUMN_NAME = line;

                                            oldSchema_SearchKey.Add(line);

                                            line = sr.ReadLine();

                                            line = line.Remove(0, "ORDINAL_POSITION:".Length);

                                            oldSchemaItem.ORDINAL_POSITION = line;

                                            line = sr.ReadLine();

                                            line = line.Remove(0, "DATA_TYPE:".Length);

                                            oldSchemaItem.DATA_TYPE = line;

                                            line = sr.ReadLine();

                                            line = line.Remove(0, "CHARACTER_MAXIMUM_LENGTH:".Length);

                                            oldSchemaItem.CHARACTER_MAXIMUM_LENGTH = line;

                                            oldSchema_List.Add(oldSchemaItem);
                                        }
                                    }

                                    foreach (var item in oldSchema_SearchKey.Except(currentScahema))
                                    {
                                        sw.WriteLine("已刪除Schema:" + item);
                                    }

                                    foreach (var item in currentScahema.Except(oldSchema_SearchKey))
                                    {
                                        sw.WriteLine("已新增Schema:" + item);
                                    }

                                    var intersectSchema = currentScahema.Intersect(oldSchema_SearchKey);

                                    for (int j = 0; j < dt_TableSchema.Rows.Count; j++)
                                    {
                                        if (intersectSchema.Contains(dt_TableSchema.Rows[j]["COLUMN_NAME"]))
                                        {
                                            sw.WriteLine("檢查Schema:" + dt_TableSchema.Rows[j]["COLUMN_NAME"]);

                                            var targetSchrma = oldSchema_List.Single(o => o.COLUMN_NAME == dt_TableSchema.Rows[j]["COLUMN_NAME"].ToString());

                                            if (targetSchrma.DATA_TYPE != dt_TableSchema.Rows[j]["DATA_TYPE"].ToString())
                                            {
                                                sw.WriteLine("DATA_TYPE已變更::" + targetSchrma.DATA_TYPE + "==>" + dt_TableSchema.Rows[j]["DATA_TYPE"].ToString());
                                            }

                                            if (targetSchrma.ORDINAL_POSITION != dt_TableSchema.Rows[j]["ORDINAL_POSITION"].ToString())
                                            {
                                                sw.WriteLine("ORDINAL_POSITION已變更::" + targetSchrma.ORDINAL_POSITION + "==>" + dt_TableSchema.Rows[j]["ORDINAL_POSITION"].ToString());
                                            }

                                            if (targetSchrma.CHARACTER_MAXIMUM_LENGTH != dt_TableSchema.Rows[j]["CHARACTER_MAXIMUM_LENGTH"].ToString())
                                            {
                                                sw.WriteLine("CHARACTER_MAXIMUM_LENGTH已變更::" + targetSchrma.CHARACTER_MAXIMUM_LENGTH + "==>" + dt_TableSchema.Rows[j]["CHARACTER_MAXIMUM_LENGTH"].ToString());
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }
                label6.Text = "比對完成";
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ex.ToString());

                MessageBox.Show(ex.Message);

                label6.Text = "失敗";
            }
        }

        class DataBaseSchema
        {
            public string COLUMN_NAME { get; set; }
            public string ORDINAL_POSITION { get; set; }
            public string DATA_TYPE { get; set; }
            public string CHARACTER_MAXIMUM_LENGTH { get; set; }
        }

        #endregion

        #region 共用部分

        private void checkedALL(CheckedListBox checkedListBox, CheckBox checkBox)
        {
            try
            {
                bool check = !checkBox.Checked ? false : true;

                for (int i = 0; i < checkedListBox.Items.Count; i++)
                {
                    checkedListBox.SetItemChecked(i, check);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ex.ToString());

                MessageBox.Show(ex.Message);
            }
        }


        #endregion
    }
}
