namespace L2DatEncDec
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.LoadBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.DirectoryDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.FileNameCombo = new System.Windows.Forms.ComboBox();
            this.ExpBtn = new System.Windows.Forms.Button();
            this.ExportDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImpBtn = new System.Windows.Forms.Button();
            this.ImportDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuFileLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileImp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileExp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOption = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuL2encdecTools = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuL2encdecItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuL2encdecItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuIniItems1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuIniItems2 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoadBtn
            // 
            this.LoadBtn.Enabled = false;
            this.LoadBtn.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LoadBtn.Location = new System.Drawing.Point(168, 31);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(107, 37);
            this.LoadBtn.TabIndex = 0;
            this.LoadBtn.Text = "Load";
            this.LoadBtn.UseVisualStyleBackColor = true;
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Enabled = false;
            this.SaveBtn.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.SaveBtn.Location = new System.Drawing.Point(281, 31);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(104, 37);
            this.SaveBtn.TabIndex = 2;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // DirectoryDialog
            // 
            this.DirectoryDialog.Description = "Choose directory where LineageII System.";
            this.DirectoryDialog.ShowNewFolderButton = false;
            // 
            // FileNameCombo
            // 
            this.FileNameCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FileNameCombo.FormattingEnabled = true;
            this.FileNameCombo.Location = new System.Drawing.Point(13, 42);
            this.FileNameCombo.Name = "FileNameCombo";
            this.FileNameCombo.Size = new System.Drawing.Size(145, 20);
            this.FileNameCombo.TabIndex = 4;
            this.FileNameCombo.SelectedIndexChanged += new System.EventHandler(this.FileNameCombo_SelectedIndexChanged);
            // 
            // ExpBtn
            // 
            this.ExpBtn.Enabled = false;
            this.ExpBtn.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ExpBtn.Location = new System.Drawing.Point(499, 31);
            this.ExpBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ExpBtn.Name = "ExpBtn";
            this.ExpBtn.Size = new System.Drawing.Size(102, 37);
            this.ExpBtn.TabIndex = 5;
            this.ExpBtn.Text = "Export";
            this.ExpBtn.UseVisualStyleBackColor = true;
            this.ExpBtn.Click += new System.EventHandler(this.ExpBtn_Click);
            // 
            // ImpBtn
            // 
            this.ImpBtn.Enabled = false;
            this.ImpBtn.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ImpBtn.Location = new System.Drawing.Point(391, 31);
            this.ImpBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ImpBtn.Name = "ImpBtn";
            this.ImpBtn.Size = new System.Drawing.Size(102, 37);
            this.ImpBtn.TabIndex = 6;
            this.ImpBtn.Text = "Import";
            this.ImpBtn.UseVisualStyleBackColor = true;
            this.ImpBtn.Click += new System.EventHandler(this.ImpBtn_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuOption,
            this.MenuL2encdecTools,
            this.MenuAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(612, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFileOpen,
            this.toolStripSeparator1,
            this.MenuFileLoad,
            this.MenuFileSave,
            this.MenuFileImp,
            this.MenuFileExp,
            this.toolStripSeparator2,
            this.MenuFileExit});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(36, 20);
            this.MenuFile.Text = "File";
            // 
            // MenuFileOpen
            // 
            this.MenuFileOpen.Name = "MenuFileOpen";
            this.MenuFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MenuFileOpen.Size = new System.Drawing.Size(166, 22);
            this.MenuFileOpen.Text = "OpenFolder";
            this.MenuFileOpen.Click += new System.EventHandler(this.MenuFileOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // MenuFileLoad
            // 
            this.MenuFileLoad.Name = "MenuFileLoad";
            this.MenuFileLoad.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.MenuFileLoad.Size = new System.Drawing.Size(166, 22);
            this.MenuFileLoad.Text = "Load";
            this.MenuFileLoad.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // MenuFileSave
            // 
            this.MenuFileSave.Name = "MenuFileSave";
            this.MenuFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MenuFileSave.Size = new System.Drawing.Size(166, 22);
            this.MenuFileSave.Text = "Save";
            this.MenuFileSave.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // MenuFileImp
            // 
            this.MenuFileImp.Name = "MenuFileImp";
            this.MenuFileImp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.MenuFileImp.Size = new System.Drawing.Size(166, 22);
            this.MenuFileImp.Text = "Import";
            this.MenuFileImp.Click += new System.EventHandler(this.ImpBtn_Click);
            // 
            // MenuFileExp
            // 
            this.MenuFileExp.Name = "MenuFileExp";
            this.MenuFileExp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.MenuFileExp.Size = new System.Drawing.Size(166, 22);
            this.MenuFileExp.Text = "Export";
            this.MenuFileExp.Click += new System.EventHandler(this.ExpBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(163, 6);
            // 
            // MenuFileExit
            // 
            this.MenuFileExit.Name = "MenuFileExit";
            this.MenuFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.MenuFileExit.Size = new System.Drawing.Size(166, 22);
            this.MenuFileExit.Text = "Exit";
            this.MenuFileExit.Click += new System.EventHandler(this.MenuFileExit_Click);
            // 
            // MenuOption
            // 
            this.MenuOption.Name = "MenuOption";
            this.MenuOption.Size = new System.Drawing.Size(50, 20);
            this.MenuOption.Text = "Option";
            this.MenuOption.Click += new System.EventHandler(this.MenuOption_Click);
            // 
            // MenuL2encdecTools
            // 
            this.MenuL2encdecTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuL2encdecItem1,
            this.MenuL2encdecItem2});
            this.MenuL2encdecTools.Name = "MenuL2encdecTools";
            this.MenuL2encdecTools.Size = new System.Drawing.Size(93, 20);
            this.MenuL2encdecTools.Text = "L2encdecTools";
            // 
            // MenuL2encdecItem1
            // 
            this.MenuL2encdecItem1.Name = "MenuL2encdecItem1";
            this.MenuL2encdecItem1.Size = new System.Drawing.Size(141, 22);
            this.MenuL2encdecItem1.Text = "Patch System";
            this.MenuL2encdecItem1.Click += new System.EventHandler(this.MenuL2encdecItem1_Click);
            // 
            // MenuL2encdecItem2
            // 
            this.MenuL2encdecItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuIniItems1,
            this.MenuIniItems2});
            this.MenuL2encdecItem2.Name = "MenuL2encdecItem2";
            this.MenuL2encdecItem2.Size = new System.Drawing.Size(141, 22);
            this.MenuL2encdecItem2.Text = "Edit INI Files";
            // 
            // MenuIniItems1
            // 
            this.MenuIniItems1.Name = "MenuIniItems1";
            this.MenuIniItems1.Size = new System.Drawing.Size(106, 22);
            this.MenuIniItems1.Text = "l2.ini";
            this.MenuIniItems1.Click += new System.EventHandler(this.MenuIniItems_Click);
            // 
            // MenuIniItems2
            // 
            this.MenuIniItems2.Name = "MenuIniItems2";
            this.MenuIniItems2.Size = new System.Drawing.Size(106, 22);
            this.MenuIniItems2.Text = "user.ini";
            this.MenuIniItems2.Click += new System.EventHandler(this.MenuIniItems_Click);
            // 
            // MenuAbout
            // 
            this.MenuAbout.Name = "MenuAbout";
            this.MenuAbout.Size = new System.Drawing.Size(47, 20);
            this.MenuAbout.Text = "About";
            this.MenuAbout.Click += new System.EventHandler(this.MenuAbout_Click);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.StatusProgress});
            this.statusBar.Location = new System.Drawing.Point(0, 79);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(612, 22);
            this.statusBar.TabIndex = 8;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(597, 17);
            this.StatusLabel.Spring = true;
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusProgress
            // 
            this.StatusProgress.Name = "StatusProgress";
            this.StatusProgress.Size = new System.Drawing.Size(100, 16);
            this.StatusProgress.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 101);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.ImpBtn);
            this.Controls.Add(this.ExpBtn);
            this.Controls.Add(this.FileNameCombo);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.LoadBtn);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "L2Dat_EncDec";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog DirectoryDialog;
        private System.Windows.Forms.SaveFileDialog ExportDialog;
        private System.Windows.Forms.OpenFileDialog ImportDialog;
        private System.Windows.Forms.ComboBox FileNameCombo;
        public System.Windows.Forms.Button LoadBtn;
        public System.Windows.Forms.Button SaveBtn;
        public System.Windows.Forms.Button ImpBtn;
        public System.Windows.Forms.Button ExpBtn;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        public System.Windows.Forms.ToolStripProgressBar StatusProgress;
        private System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ToolStripMenuItem MenuFile;
        public System.Windows.Forms.ToolStripMenuItem MenuFileOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripMenuItem MenuFileLoad;
        public System.Windows.Forms.ToolStripMenuItem MenuFileSave;
        public System.Windows.Forms.ToolStripMenuItem MenuFileImp;
        public System.Windows.Forms.ToolStripMenuItem MenuFileExp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripMenuItem MenuFileExit;
        public System.Windows.Forms.ToolStripMenuItem MenuOption;
        public System.Windows.Forms.ToolStripMenuItem MenuL2encdecTools;
        public System.Windows.Forms.ToolStripMenuItem MenuL2encdecItem1;
        public System.Windows.Forms.ToolStripMenuItem MenuL2encdecItem2;
        private System.Windows.Forms.ToolStripMenuItem MenuIniItems1;
        private System.Windows.Forms.ToolStripMenuItem MenuIniItems2;
        public System.Windows.Forms.ToolStripMenuItem MenuAbout;
    }
}

