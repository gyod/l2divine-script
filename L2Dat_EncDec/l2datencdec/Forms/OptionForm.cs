#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using L2DatEncDec.Parsers;

#endregion

namespace L2DatEncDec
{
    public partial class OptionForm : Form
    {
        List<String> EncodingList = new List<String>();

        public OptionForm()
        {
            InitializeComponent();
        }

        private void OptionForm_Load(object sender, EventArgs e)
        {
            // Set Localization
            Program.language.setLocalization(this);
            this.Text = Program.main_form.MenuOption.Text;

            // Set ChronicleCombo
            this.ChronicleCombo.Items.Clear();
            foreach (String name in Enum.GetNames(typeof(DatVersion)))
                this.ChronicleCombo.Items.Add(name);
            this.ChronicleCombo.SelectedIndex = Program.config.ChronicleSetting;

            // Set EncodeingCombo
            foreach (EncodingInfo info in Encoding.GetEncodings())
            {
                if (!EncodingList.Contains(info.Name.ToLower()))
                    EncodingList.Add(info.Name.ToLower());
            }
            EncodingList.Sort();
            int idx = 0;
            this.EncodeingCombo.Items.Clear();
            for (int i = 0; i < EncodingList.Count; i++)
            {
                this.EncodeingCombo.Items.Add(EncodingList[i].ToUpper());
                if (EncodingList[i] == Program.config.TextEncoding)
                    idx = i;
            }
            this.EncodeingCombo.SelectedIndex = idx;

            // Set LanguageCombo
            idx = 0;
            this.LanguageCombo.Items.Clear();
            if (Directory.Exists(Program.language.LangPath))
            {
                int i = 0;
                DirectoryInfo current = new DirectoryInfo(Program.language.LangPath);
                foreach (FileInfo info in current.GetFiles("*.xml"))
                {
                    String FileName = Path.GetFileNameWithoutExtension(info.FullName);
                    this.LanguageCombo.Items.Add(FileName.Substring(0, 1).ToUpper() + FileName.Substring(1));
                    if (Path.GetFileName(info.FullName).ToLower() == Program.config.LangFileName.ToLower())
                        idx = i;
                    i++;
                }
            }
            this.LanguageCombo.SelectedIndex = idx;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            Program.config.ChronicleSetting = this.ChronicleCombo.SelectedIndex;
            Program.config.TextEncoding = EncodingList[this.EncodeingCombo.SelectedIndex];
            Program.config.LangFileName = this.LanguageCombo.Items[this.LanguageCombo.SelectedIndex] + ".xml";
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}