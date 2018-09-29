namespace DataBaseChecker
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConnectDataBase = new System.Windows.Forms.Button();
            this.txtConnString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnCompare = new System.Windows.Forms.Button();
            this.chkListTableDataChecker = new System.Windows.Forms.CheckedListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRecordTablenfo = new System.Windows.Forms.Button();
            this.txtDatabaseName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtDatabaseName);
            this.panel1.Controls.Add(this.btnRecordTablenfo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.chkListTableDataChecker);
            this.panel1.Controls.Add(this.btnConnectDataBase);
            this.panel1.Controls.Add(this.txtConnString);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1403, 570);
            this.panel1.TabIndex = 0;
            // 
            // btnConnectDataBase
            // 
            this.btnConnectDataBase.Location = new System.Drawing.Point(1159, 8);
            this.btnConnectDataBase.Name = "btnConnectDataBase";
            this.btnConnectDataBase.Size = new System.Drawing.Size(75, 23);
            this.btnConnectDataBase.TabIndex = 3;
            this.btnConnectDataBase.Text = "連結資料庫";
            this.btnConnectDataBase.UseVisualStyleBackColor = true;
            this.btnConnectDataBase.Click += new System.EventHandler(this.btnConnectDataBase_Click);
            // 
            // txtConnString
            // 
            this.txtConnString.Location = new System.Drawing.Point(73, 8);
            this.txtConnString.Name = "txtConnString";
            this.txtConnString.Size = new System.Drawing.Size(1079, 22);
            this.txtConnString.TabIndex = 1;
            this.txtConnString.Text = "data source=DESKTOP-39RK5BQ\\SQLEXPRESS;initial catalog=CathayDimsDB;integrated se" +
    "curity=True;MultipleActiveResultSets=True;";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "連線字串：";
            // 
            // BtnCompare
            // 
            this.BtnCompare.Location = new System.Drawing.Point(2, 588);
            this.BtnCompare.Name = "BtnCompare";
            this.BtnCompare.Size = new System.Drawing.Size(75, 23);
            this.BtnCompare.TabIndex = 2;
            this.BtnCompare.Text = "比對";
            this.BtnCompare.UseVisualStyleBackColor = true;
            this.BtnCompare.Click += new System.EventHandler(this.BtnCompare_Click);
            // 
            // chkListTableDataChecker
            // 
            this.chkListTableDataChecker.FormattingEnabled = true;
            this.chkListTableDataChecker.Location = new System.Drawing.Point(14, 109);
            this.chkListTableDataChecker.Name = "chkListTableDataChecker";
            this.chkListTableDataChecker.Size = new System.Drawing.Size(1220, 225);
            this.chkListTableDataChecker.TabIndex = 4;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(17, 84);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(93, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "全選 / 都不選";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "打勾資料也須控管的資料表";
            // 
            // btnRecordTablenfo
            // 
            this.btnRecordTablenfo.Location = new System.Drawing.Point(14, 340);
            this.btnRecordTablenfo.Name = "btnRecordTablenfo";
            this.btnRecordTablenfo.Size = new System.Drawing.Size(118, 23);
            this.btnRecordTablenfo.TabIndex = 8;
            this.btnRecordTablenfo.Text = "記錄資訊";
            this.btnRecordTablenfo.UseVisualStyleBackColor = true;
            this.btnRecordTablenfo.Click += new System.EventHandler(this.btnRecordTableInfo_Click);
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.Location = new System.Drawing.Point(260, 340);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(100, 22);
            this.txtDatabaseName.TabIndex = 9;
            this.txtDatabaseName.Text = "CathayDimsDB";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(153, 345);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "產生的檔案名稱：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1427, 623);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BtnCompare);
            this.Name = "Form1";
            this.Text = "DataBaseChecker";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnString;
        private System.Windows.Forms.Button BtnCompare;
        private System.Windows.Forms.Button btnConnectDataBase;
        private System.Windows.Forms.CheckedListBox chkListTableDataChecker;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRecordTablenfo;
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.Label label3;
    }
}

