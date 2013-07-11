namespace L2DatEncDec
{
    partial class OptionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionForm));
            this.Encodinglabel = new System.Windows.Forms.Label();
            this.EncodeingCombo = new System.Windows.Forms.ComboBox();
            this.ChronicleLabel = new System.Windows.Forms.Label();
            this.ChronicleCombo = new System.Windows.Forms.ComboBox();
            this.OkBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.LanguageLabel = new System.Windows.Forms.Label();
            this.LanguageCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Encodinglabel
            // 
            this.Encodinglabel.AutoSize = true;
            this.Encodinglabel.Location = new System.Drawing.Point(9, 46);
            this.Encodinglabel.Name = "Encodinglabel";
            this.Encodinglabel.Size = new System.Drawing.Size(74, 12);
            this.Encodinglabel.TabIndex = 2;
            this.Encodinglabel.Text = "TextEncoding";
            // 
            // EncodeingCombo
            // 
            this.EncodeingCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EncodeingCombo.FormattingEnabled = true;
            this.EncodeingCombo.Location = new System.Drawing.Point(121, 43);
            this.EncodeingCombo.Name = "EncodeingCombo";
            this.EncodeingCombo.Size = new System.Drawing.Size(158, 20);
            this.EncodeingCombo.TabIndex = 3;
            // 
            // ChronicleLabel
            // 
            this.ChronicleLabel.AutoSize = true;
            this.ChronicleLabel.Location = new System.Drawing.Point(10, 15);
            this.ChronicleLabel.Name = "ChronicleLabel";
            this.ChronicleLabel.Size = new System.Drawing.Size(89, 12);
            this.ChronicleLabel.TabIndex = 0;
            this.ChronicleLabel.Text = "ChronicleSetting";
            // 
            // ChronicleCombo
            // 
            this.ChronicleCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChronicleCombo.FormattingEnabled = true;
            this.ChronicleCombo.Location = new System.Drawing.Point(121, 12);
            this.ChronicleCombo.Name = "ChronicleCombo";
            this.ChronicleCombo.Size = new System.Drawing.Size(158, 20);
            this.ChronicleCombo.TabIndex = 1;
            // 
            // OkBtn
            // 
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkBtn.Location = new System.Drawing.Point(51, 108);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 6;
            this.OkBtn.Text = "O K";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(167, 108);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // LanguageLabel
            // 
            this.LanguageLabel.AutoSize = true;
            this.LanguageLabel.Location = new System.Drawing.Point(9, 77);
            this.LanguageLabel.Name = "LanguageLabel";
            this.LanguageLabel.Size = new System.Drawing.Size(53, 12);
            this.LanguageLabel.TabIndex = 4;
            this.LanguageLabel.Text = "Language";
            // 
            // LanguageCombo
            // 
            this.LanguageCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguageCombo.FormattingEnabled = true;
            this.LanguageCombo.Location = new System.Drawing.Point(121, 74);
            this.LanguageCombo.Name = "LanguageCombo";
            this.LanguageCombo.Size = new System.Drawing.Size(158, 20);
            this.LanguageCombo.TabIndex = 5;
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 146);
            this.Controls.Add(this.LanguageCombo);
            this.Controls.Add(this.LanguageLabel);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.ChronicleCombo);
            this.Controls.Add(this.ChronicleLabel);
            this.Controls.Add(this.EncodeingCombo);
            this.Controls.Add(this.Encodinglabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Option";
            this.Load += new System.EventHandler(this.OptionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label Encodinglabel;
        private System.Windows.Forms.ComboBox EncodeingCombo;
        public System.Windows.Forms.Label ChronicleLabel;
        private System.Windows.Forms.ComboBox ChronicleCombo;
        public System.Windows.Forms.Button OkBtn;
        public System.Windows.Forms.Button CancelBtn;
        public System.Windows.Forms.Label LanguageLabel;
        private System.Windows.Forms.ComboBox LanguageCombo;
    }
}