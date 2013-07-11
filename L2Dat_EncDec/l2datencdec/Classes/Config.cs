#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Xml;
using System.Windows.Forms.Design;
using System.Net;
using LmUtils;

#endregion

namespace L2DatEncDec
{
	public class Config : LmUtils.Config
	{
		#region Constructors, destructor...

		public Config ()
			: base ("Config.xml")
		{
		}

		#endregion
		private const string Category_Global = "Global parameters";
		private const string Category_LineAge = "LineAge";

        #region Save, load and defaults

        public override void Reload()
        {
            base.Reload();

            if (this.xml.Root.ChildNodes.Count == 0)
            {
                Program.log.Add("No configuration file was found. Default settings will be used", LmUtils.LogLevel.Warning);
                this.Defaults();
            }

            if (this.inited)
                Program.main_form.Config_Load();
        }

        public override void Save()
        {
            try
            {
                base.Save();
            }
            catch (Exception e)
            {
                Program.log.Add("Error saving configuration settings", e);
            }
        }

        #endregion

		#region Global

		[DisplayName ("Log history"), Description ("Amount of last logs to store"), Category (Category_Global), DefaultValue (30)]
		public int LogHistory
		{
			get
			{
				try
				{
					return Convert.ToInt32(this.xml["Global.LogHistory"]);
				}
				catch
				{
					return (int)this.GetDefault("LogHistory");
				}
			}
			set
			{
				this.xml["Global.LogHistory"] = value.ToString();

				if (this.inited)
				{
					Program.log.LogHistory = value;
				}
			}
		}

        [DisplayName("Text Encoding"), Description("Encoding of read and written text file"), Category(Category_Global), DefaultValue("utf-8")]
        public string TextEncoding
        {
            get
            {
                try
                {
                    return this.xml["Global.TextEncoding"];
                }
                catch
                {
                    return (string)this.GetDefault("TextEncoding");
                }
            }
            set
            {
                this.xml["Global.TextEncoding"] = value;
            }
        }

        [DisplayName("Language File Name"), Description("Definition of ControlText"), Category(Category_Global), DefaultValue("default.xml")]
        public string LangFileName
        {
            get
            {
                try
                {
                    return this.xml["Global.LangFileName"];
                }
                catch
                {
                    return (string)this.GetDefault("LangFileName");
                }
            }
            set
            {
                this.xml["Global.LangFileName"] = value;
            }
        }

        [DisplayName("Chronicle Setting"), Description("Setting of Chronicle Version"), Category(Category_Global), DefaultValue(6)]
        public int ChronicleSetting
		{
			get
			{
				try
				{
                    return Convert.ToInt32(this.xml["Global.ChronicleSetting"]);
				}
				catch
				{
                    return (int)this.GetDefault("ChronicleSetting");
				}
			}
			set
			{
                this.xml["Global.ChronicleSetting"] = value.ToString();
			}
		}

		#endregion

		#region LineAge

		[DisplayName ("LineAge directory"), Description ("Directory where LineAge installed"), Category (Category_LineAge), DefaultValue ("")]
		public string LineAgeDirectory
		{
			get
			{
				try
				{
					return this.xml["LineAge.Directory"];
				}
				catch
				{
					return (string) this.GetDefault ("LineAgeDirectory");
				}
			}
			set
			{
				this.xml["LineAge.Directory"] = value;
			}
		}

		[DisplayName ("Save BAK files"), Description ("Set to 'True' if you want program to save bak-files on each save"), Category (Category_LineAge), DefaultValue (true)]
		public bool LineAgeSaveBakFiles
		{
			get
			{
				try
				{
					return Convert.ToBoolean (this.xml["LineAge.SaveBakFiles"]);
				}
				catch
				{
					return (bool) this.GetDefault ("LineAgeSaveBakFiles");
				}
			}
			set
			{
				this.xml["LineAge.SaveBakFiles"] = value.ToString ();
			}
		}

        [DisplayName("Encrypt after save"), Description("Encrypt files with specified parameters after save (set to empty string to disable encryption after save). Se l2encdec documentation for more info. Example: -h 413"), Category(Category_LineAge), DefaultValue("-h 413")]
		public string LineAge_EncryptParameters
		{
			get
			{
				try
				{
					return this.xml["LineAge.EncryptParameters"];
				}
				catch
				{
					return (string) this.GetDefault ("LineAge_EncryptParameters");
				}
			}
			set
			{
				this.xml["LineAge.EncryptParameters"] = value;
			}
		}

		#endregion
	}
}