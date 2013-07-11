#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using L2DatEncDec.Definitions;
using L2DatEncDec.Parsers;
using L2DatEncDec.Tools;

#endregion

namespace L2DatEncDec
{
	public partial class MainForm : Form
	{
		public L2DatParser DatInfo;
		public DatVersion selectedDatVersion;
		internal List<L2DatDefinition> DatDatas;
		internal DatFileType selectedFileType;
		internal string selectedComboName = "";

		#region Config

		public void Config_Load()
		{
			// form and controls
			//Program.config.LoadFormState(this, "main_form");
			this.selectedDatVersion = (DatVersion)Program.config.ChronicleSetting;
			this.DirectoryDialog.Description = Program.language.getMessage(MsgList.DirectoryDialog_Desc);

			// Check DatFile Directory
			if (!Directory.Exists(Program.config.LineAgeDirectory))
			{
				new MessageBox(Program.language.getMessage(MsgList.SystemDir_Error), true);

				Program.config.LoadLastFolder(this.DirectoryDialog, "DirectoryDialog");
				if (this.DirectoryDialog.ShowDialog() == DialogResult.OK)
				{
					Program.config.SaveLastFolder(this.DirectoryDialog, "DirectoryDialog");
					Program.config.LineAgeDirectory = this.DirectoryDialog.SelectedPath;
				}
			}

			if (!Directory.Exists(Program.config.LineAgeDirectory))
				this.MenuL2encdecTools.Enabled = false;
		}

		private void Config_Save()
		{
			// form and controls
			//Program.config.SaveFormState(this, "main_form");
			Program.config.ChronicleSetting = (int)this.selectedDatVersion;

			// config
			Program.config.Save();
		}

		#endregion

		#region Control Events

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			this.Visible = false;
			this.SuspendLayout();

			try
			{
				this.Config_Load();
				this.Forms_Init(true);
				this.Forms_Update();
			}
			catch (Exception ex)
			{
				Program.log.Add(ex, true);
			}
			finally
			{
				this.Visible = true;
				this.ResumeLayout();

				this.Show();
				this.BringToFront();
				this.Activate();
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.Enabled = false;
			this.Config_Save();
		}

		private void FileNameCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.selectedComboName = this.FileNameCombo.Items[this.FileNameCombo.SelectedIndex].ToString();
			String[] TmpStr = Enum.GetNames(typeof(DatFileType));
			for (int i = 0; i < TmpStr.Length; i++)
			{
				if (this.selectedComboName.StartsWith(TmpStr[i].ToLower()))
				{
					this.selectedFileType = (DatFileType)i;
					break;
				}
			}

			this.Forms_Init(false);
			this.LoadBtn.Enabled = true;
			this.ImpBtn.Enabled = true;
			this.Forms_Update();
		}

		private void Forms_Init(Boolean isReload)
		{
			// Set Form Title
			this.Text = Application.ProductName + " Ver." + Application.ProductVersion.Substring(0, 6) +
						   " - " + Program.config.LineAgeDirectory;

			if (isReload)
			{
				// Set FileNameCombo
				this.FileNameCombo.Items.Clear();
				if (Directory.Exists(Program.config.LineAgeDirectory))
				{
					DirectoryInfo current = new DirectoryInfo(Program.config.LineAgeDirectory);
					foreach (FileInfo info in current.GetFiles("*.dat"))
					{
						foreach (String name in Enum.GetNames(typeof(DatFileType)))
						{
							String FileName = Path.GetFileNameWithoutExtension(info.FullName);
							if (FileName.ToLower().StartsWith(name.ToLower()))
							{
								this.FileNameCombo.Items.Add(info.Name.ToLower());
								break;
							}
						}
					}
				}
				// Set Localization
				Program.language.setLocalization(this);
			}

			// Set Buttons
			this.SaveBtn.Enabled = false;
			this.ExpBtn.Enabled = false;
			this.LoadBtn.Enabled = false;
			this.ImpBtn.Enabled = false;

			// Set StatusBars
			this.StatusLabel.Text = Program.language.getMessage(MsgList.ChronicleSetting) + selectedDatVersion.ToString() + " " +
									Program.language.getMessage(MsgList.TextEncoding) + Program.config.TextEncoding.ToUpper();
			this.StatusProgress.Minimum = 0;
			this.StatusProgress.Maximum = 100;
			this.StatusProgress.Value = 0;
			this.StatusProgress.Visible = false;

			// Set Datas
			this.DatInfo_init();
		}

		private void Forms_Update()
		{
			this.MenuFileLoad.Enabled = this.LoadBtn.Enabled;
			this.MenuFileSave.Enabled = this.SaveBtn.Enabled;
			this.MenuFileImp.Enabled = this.ImpBtn.Enabled;
			this.MenuFileExp.Enabled = this.ExpBtn.Enabled;
			if (this.selectedFileType == DatFileType.Chargrp)
			{
				// Not Support
				this.SaveBtn.Enabled = false;
				this.ImpBtn.Enabled = false;
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
				{
					this.ExpBtn.Enabled = false;
					this.LoadBtn.Enabled = false;
				}
			}
		}

		private void DatInfo_init()
		{
			Type t = Type.GetType("L2DatEncDec.Parsers." + this.selectedFileType.ToString());
			object instance = t.InvokeMember(null,
										BindingFlags.CreateInstance,
										null,
										null,
										new object[] { });
			if (instance != null)
				DatInfo = (L2DatParser)instance;
			else
				DatInfo = new L2DatParser();
			DatDatas = new List<L2DatDefinition>();
		}

		#endregion

		#region LoadBtn Action

		private void LoadBtn_Click(object sender, EventArgs e)
		{
			try
			{
				this.StatusLabel.Text = Program.language.getMessage(MsgList.PleaseWait);
				this.FileNameCombo.Enabled = false;
				this.Enabled = false;

				this.DatInfo_init();
				if (!File.Exists(Path.Combine(Program.config.LineAgeDirectory, this.selectedComboName)))
				{
					this.StatusLabel.Text = Program.language.getMessage(MsgList.ERROR) +
												String.Format(Program.language.getMessage(MsgList.FileNotFound));
					return;
				}
				BinaryReader f = L2EncDec.Decrypt(this.selectedComboName, Encoding.Default);
				if (f == null)
					return;
				this.StatusProgress.Minimum = 0;
				this.StatusProgress.Value = 0;
				this.StatusProgress.Visible = true;
				this.DatDatas = DatInfo.Parse(f);
				f.Close();

				this.SaveBtn.Enabled = true;
				this.ExpBtn.Enabled = true;
			}
			catch (Exception ex)
			{
				Program.log.Add(ex, true);
			}
			finally
			{
				this.Enabled = true;
				this.FileNameCombo.Enabled = true;
				this.StatusProgress.Visible = false;
				this.Forms_Update();
			}
			this.StatusLabel.Text = Program.language.getMessage(MsgList.COMPLETE) +
										String.Format(Program.language.getMessage(MsgList.CompleteLoad), this.DatDatas.Count);
		}

		#endregion

		#region SaveBtn Action

		private void SaveBtn_Click(object sender, EventArgs e)
		{
			try
			{
				this.StatusLabel.Text = Program.language.getMessage(MsgList.PleaseWait);
				this.FileNameCombo.Enabled = false;
				this.Enabled = false;

				if (this.DatDatas.Count == 0)
				{
					this.StatusLabel.Text = Program.language.getMessage(MsgList.ERROR) +
												String.Format(Program.language.getMessage(MsgList.DataIsEmpty));
					return;
				}
				string fname = Path.Combine(Program.config.LineAgeDirectory, this.selectedComboName);
				if (Program.config.LineAgeSaveBakFiles)
				{
					try
					{
						File.Move(fname, Path.ChangeExtension(fname, ".bak"));
					}
					catch
					{
					}
				}
				BinaryWriter f = new BinaryWriter(File.OpenWrite(fname), Encoding.Default);
				if (f == null)
					return;
				this.StatusProgress.Minimum = 0;
				this.StatusProgress.Value = 0;
				this.StatusProgress.Visible = true;
				DatInfo.Compile(f, this.DatDatas);
				f.Close();

				L2EncDec.Encrypt(this.selectedComboName);
			}
			catch (Exception ex)
			{
				Program.log.Add(ex, true);
			}
			finally
			{
				this.Enabled = true;
				this.FileNameCombo.Enabled = true;
				this.StatusProgress.Visible = false;
			}
			this.StatusLabel.Text = Program.language.getMessage(MsgList.COMPLETE) +
										String.Format(Program.language.getMessage(MsgList.CompleteSave), this.DatDatas.Count);
		}

		#endregion

		#region ImpBtn Action

		private void ImpBtn_Click(object sender, EventArgs e)
		{
			String FName = "";
			String FValue = "";
			long RecNo = 1;

			try
			{
				this.StatusLabel.Text = Program.language.getMessage(MsgList.PleaseWait);
				this.FileNameCombo.Enabled = false;
				this.Enabled = false;

				this.ImportDialog.InitialDirectory = Application.StartupPath;
				this.ImportDialog.FileName = this.selectedComboName.Substring(0, this.selectedComboName.LastIndexOf("."));
				this.ImportDialog.Filter = "Tab-SeparatedValues files (*.tsv)|*.tsv";
				this.ImportDialog.FilterIndex = 1;
				this.ImportDialog.RestoreDirectory = true;
				if (this.ImportDialog.ShowDialog() == DialogResult.OK)
				{
					this.DatInfo_init();
					string line = "";
					System.Text.Encoding enc = System.Text.Encoding.GetEncoding(Program.config.TextEncoding);
					System.IO.StreamReader sr = new System.IO.StreamReader(this.ImportDialog.FileName, enc);

					this.StatusProgress.Minimum = 0;
					this.StatusProgress.Maximum = (int)sr.BaseStream.Length;
					this.StatusProgress.Value = 0;
					this.StatusProgress.Visible = true;

					while ((line = sr.ReadLine()) != null)
					{
						if (line.StartsWith("#")) continue;

						String[] TmpStr = line.Split(new char[] { '\t' });
						for (int i = 0; i < TmpStr.Length; i++)
						{
							TmpStr[i] = TmpStr[i].Trim(new char[] { '"' });
						}

						L2DatDefinition item = this.DatInfo.getDefinition();
						for (int i = 0, j = 0; i < this.DatInfo.getFieldNames().Count; i++, j++)
						{
							FName = this.DatInfo.getFieldNames()[i];
							FValue = TmpStr[j];
							FieldInfo FType = this.DatInfo.getDefinition().GetType().GetField(FName);
							if (FType == null) continue;
							if (FType.FieldType.FullName.EndsWith("UInt32"))
								FType.SetValue(item, Convert.ToUInt32(TmpStr[j]));
							else if (FType.FieldType.FullName.EndsWith("Int32"))
								FType.SetValue(item, Convert.ToInt32(TmpStr[j]));
							else if (FType.FieldType.FullName.EndsWith("Single"))
								FType.SetValue(item, Convert.ToSingle(TmpStr[j]));
							else if (FType.FieldType.FullName.EndsWith("Color"))
								FType.SetValue(item, LmUtils.ConvertUtilities.HtmlColorToColor(TmpStr[j]));
							else if (FType.FieldType.FullName.EndsWith("CNTINT_PAIR"))
							{
								CNTINT_PAIR tmpMtx = new CNTINT_PAIR();
								string[] TmpStr2 = new string[tmpMtx.getFieldCount()];
								for (int k = 0; k < tmpMtx.getFieldCount(); k++)
								{
									TmpStr2[k] = TmpStr[j];
									if (k < tmpMtx.getFieldCount() - 1)
										j++;
								}
								tmpMtx.setText(TmpStr2);
								FType.SetValue(item, tmpMtx);
							}
							else if (FType.FieldType.FullName.EndsWith("CNTRINT_PAIR"))
							{
								CNTRINT_PAIR tmpMtx = new CNTRINT_PAIR();
								string[] TmpStr2 = new string[tmpMtx.getFieldCount()];
								for (int k = 0; k < tmpMtx.getFieldCount(); k++)
								{
									TmpStr2[k] = TmpStr[j];
									if (k < tmpMtx.getFieldCount() - 1)
										j++;
								}
								tmpMtx.setText(TmpStr2);
								FType.SetValue(item, tmpMtx);
							}
							else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR"))
							{
								CNTTXT_PAIR tmpMtx = new CNTTXT_PAIR();
								string[] TmpStr2 = new string[tmpMtx.getFieldCount()];
								for (int k = 0; k < tmpMtx.getFieldCount(); k++)
								{
									TmpStr2[k] = TmpStr[j];
									if (k < tmpMtx.getFieldCount() - 1)
										j++;
								}
								tmpMtx.setText(TmpStr2);
								FType.SetValue(item, tmpMtx);
							}
							else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR2"))
							{
								CNTTXT_PAIR2 tmpMtx = new CNTTXT_PAIR2();
								string[] TmpStr2 = new string[tmpMtx.getFieldCount()];
								for (int k = 0; k < tmpMtx.getFieldCount(); k++)
								{
									TmpStr2[k] = TmpStr[j];
									if (k < tmpMtx.getFieldCount() - 1)
										j++;
								}
								tmpMtx.setText(TmpStr2);
								FType.SetValue(item, tmpMtx);
							}
							else if (FType.FieldType.FullName.EndsWith("CNTASCF_PAIR"))
							{
								CNTASCF_PAIR tmpMtx = new CNTASCF_PAIR();
								string[] TmpStr2 = new string[tmpMtx.getFieldCount()];
								for (int k = 0; k < tmpMtx.getFieldCount(); k++)
								{
									TmpStr2[k] = TmpStr[j];
									if (k < tmpMtx.getFieldCount() - 1)
										j++;
								}
								tmpMtx.setText(TmpStr2);
								FType.SetValue(item, tmpMtx);
							}
							else if (FType.FieldType.FullName.EndsWith("MTX"))
							{
								MTX tmpMtx = new MTX();
								string[] TmpStr2 = new string[tmpMtx.getFieldCount()];
								for (int k = 0; k < tmpMtx.getFieldCount(); k++)
								{
									TmpStr2[k] = TmpStr[j];
									if (k < tmpMtx.getFieldCount() - 1)
										j++;
								}
								tmpMtx.setText(TmpStr2);
								FType.SetValue(item, tmpMtx);
							}
							else if (FType.FieldType.FullName.EndsWith("MTX2"))
							{
								MTX2 tmpMtx = new MTX2();
								string[] TmpStr2 = new string[tmpMtx.getFieldCount()];
								for (int k = 0; k < tmpMtx.getFieldCount(); k++)
								{
									TmpStr2[k] = TmpStr[j];
									if (k < tmpMtx.getFieldCount() - 1)
										j++;
								}
								tmpMtx.setText(TmpStr2);
								FType.SetValue(item, tmpMtx);
							}
							else if (FType.FieldType.FullName.EndsWith("MTX3"))
							{
								MTX3 tmpMtx = new MTX3();
								string[] TmpStr2 = new string[tmpMtx.getFieldCount()];
								for (int k = 0; k < tmpMtx.getFieldCount(); k++)
								{
									TmpStr2[k] = TmpStr[j];
									if (k < tmpMtx.getFieldCount() - 1)
										j++;
								}
								tmpMtx.setText(TmpStr2);
								FType.SetValue(item, tmpMtx);
							}
							else if (FType.FieldType.FullName.EndsWith("ASCF"))
							{
								ASCF tmpStr2 = new ASCF();
								tmpStr2.Text = TmpStr[j];
								FType.SetValue(item, tmpStr2);
							}
							else if (FType.FieldType.FullName.EndsWith("HEX"))
							{
								HEX tmpStr2 = new HEX();
								tmpStr2.Text = TmpStr[j];
								FType.SetValue(item, tmpStr2);
							}
							else if (FType.FieldType.FullName.EndsWith("UNICODE"))
							{
								UNICODE tmpStr2 = new UNICODE();
								tmpStr2.Text = TmpStr[j];
								FType.SetValue(item, tmpStr2);
							}
							else
								FType.SetValue(item, TmpStr[j]);
						}
						this.DatDatas.Add(item);
						this.StatusProgress.Value = (int)sr.BaseStream.Position;
						RecNo++;
					}
					sr.Close();

					this.SaveBtn.Enabled = true;
					this.ExpBtn.Enabled = true;
				}
				else
					return;
			}
			catch (Exception ex)
			{
				ex = new ApplicationException(
					String.Format("Error importing string file (RecNo: {0} FieldName: {1} FieldValue: {2})",
								  RecNo, FName, FValue), ex);
				Program.log.Add(ex, true);
			}
			finally
			{
				this.Enabled = true;
				this.FileNameCombo.Enabled = true;
				this.StatusProgress.Visible = false;
				this.Forms_Update();
			}
			this.StatusLabel.Text = Program.language.getMessage(MsgList.COMPLETE) +
										String.Format(Program.language.getMessage(MsgList.CompleteImp), this.DatDatas.Count);
		}

		#endregion

		#region ExpBtn Action

		private void ExpBtn_Click(object sender, EventArgs e)
		{
			String FName = "";
			long RecNo = 1;

			try
			{
				this.StatusLabel.Text = Program.language.getMessage(MsgList.PleaseWait);
				this.FileNameCombo.Enabled = false;
				this.Enabled = false;

				if (this.DatDatas.Count == 0)
				{
					this.StatusLabel.Text = Program.language.getMessage(MsgList.ERROR) +
												String.Format(Program.language.getMessage(MsgList.DataIsEmpty));
					return;
				}
				this.ExportDialog.InitialDirectory = Application.StartupPath;
				this.ExportDialog.FileName = this.selectedComboName.Substring(0, this.selectedComboName.LastIndexOf("."));
				this.ExportDialog.Filter = "Tab-SeparatedValues files (*.tsv)|*.tsv";
				this.ExportDialog.FilterIndex = 1;
				this.ExportDialog.RestoreDirectory = true;
				if (this.ExportDialog.ShowDialog() == DialogResult.OK)
				{
					System.Text.Encoding enc = System.Text.Encoding.GetEncoding(Program.config.TextEncoding);
					System.IO.StreamWriter sr = new System.IO.StreamWriter(this.ExportDialog.FileName, false, enc);

					// Write Headers
					sr.Write("# ");
					for (int i = 0; i < this.DatInfo.getFieldNames().Count; i++)
					{
						L2DatDefinition info = this.DatDatas[0];
						FName = this.DatInfo.getFieldNames()[i];
						FieldInfo FType = this.DatInfo.getDefinition().GetType().GetField(FName);
						if (FType == null) continue;
						if (FType.FieldType.FullName.EndsWith("CNTINT_PAIR"))
						{
							String TmpStr = ((CNTINT_PAIR)FType.GetValue(info)).getHeaderText(FName);
							sr.Write(TmpStr);
							continue;
						}
						else if (FType.FieldType.FullName.EndsWith("CNTRINT_PAIR"))
						{
							String TmpStr = ((CNTRINT_PAIR)FType.GetValue(info)).getHeaderText(FName);
							sr.Write(TmpStr);
							continue;
						}
						else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR"))
						{
							String TmpStr = ((CNTTXT_PAIR)FType.GetValue(info)).getHeaderText(FName);
							sr.Write(TmpStr);
							continue;
						}
						else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR2"))
						{
							String TmpStr = ((CNTTXT_PAIR2)FType.GetValue(info)).getHeaderText(FName);
							sr.Write(TmpStr);
							continue;
						}
						else if (FType.FieldType.FullName.EndsWith("CNTASCF_PAIR"))
						{
							String TmpStr = ((CNTASCF_PAIR)FType.GetValue(info)).getHeaderText(FName);
							sr.Write(TmpStr);
							continue;
						}
						else if (FType.FieldType.FullName.EndsWith("MTX"))
						{
							String TmpStr = ((MTX)FType.GetValue(info)).getHeaderText(FName);
							sr.Write(TmpStr);
							continue;
						}
						else if (FType.FieldType.FullName.EndsWith("MTX2"))
						{
							String TmpStr = ((MTX2)FType.GetValue(info)).getHeaderText(FName);
							sr.Write(TmpStr);
							continue;
						}
						else if (FType.FieldType.FullName.EndsWith("MTX3"))
						{
							String TmpStr = ((MTX3)FType.GetValue(info)).getHeaderText(FName);
							sr.Write(TmpStr);
							continue;
						}
						sr.Write(FName);
						if (i < this.DatInfo.getFieldNames().Count - 1)
							sr.Write('\t');
					}
					sr.Write("\r\n");

					this.StatusProgress.Minimum = 0;
					this.StatusProgress.Maximum = this.DatDatas.Count;
					this.StatusProgress.Value = 0;
					this.StatusProgress.Visible = true;
					
					// Write Datas
					for (int i = 0; i < this.DatDatas.Count; i++)
					{
						L2DatDefinition info = this.DatDatas[i];
						for (int j = 0; j < this.DatInfo.getFieldNames().Count; j++)
						{
							FName = this.DatInfo.getFieldNames()[j];
							FieldInfo FType = this.DatInfo.getDefinition().GetType().GetField(FName);
							if (FType == null) continue;
							String TmpStr = "";
							if (FType.GetValue(info) != null)
							{
								if (FType.FieldType.FullName.EndsWith("Color"))
									TmpStr = LmUtils.ConvertUtilities.ColorToHtmlColor((Color)FType.GetValue(info));
								else if (FType.FieldType.FullName.EndsWith("CNTINT_PAIR"))
								{
									TmpStr = ((CNTINT_PAIR)FType.GetValue(info)).getText();
									sr.Write(TmpStr);
									continue;
								}
								else if (FType.FieldType.FullName.EndsWith("CNTRINT_PAIR"))
								{
									TmpStr = ((CNTRINT_PAIR)FType.GetValue(info)).getText();
									sr.Write(TmpStr);
									continue;
								}
								else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR"))
								{
									TmpStr = ((CNTTXT_PAIR)FType.GetValue(info)).getText();
									sr.Write(TmpStr);
									continue;
								}
								else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR2"))
								{
									TmpStr = ((CNTTXT_PAIR2)FType.GetValue(info)).getText();
									sr.Write(TmpStr);
									continue;
								}
								else if (FType.FieldType.FullName.EndsWith("CNTASCF_PAIR"))
								{
									TmpStr = ((CNTASCF_PAIR)FType.GetValue(info)).getText();
									sr.Write(TmpStr);
									continue;
								}
								else if (FType.FieldType.FullName.EndsWith("MTX"))
								{
									TmpStr = ((MTX)FType.GetValue(info)).getText();
									sr.Write(TmpStr);
									continue;
								}
								else if (FType.FieldType.FullName.EndsWith("MTX2"))
								{
									TmpStr = ((MTX2)FType.GetValue(info)).getText();
									sr.Write(TmpStr);
									continue;
								}
								else if (FType.FieldType.FullName.EndsWith("MTX3"))
								{
									TmpStr = ((MTX3)FType.GetValue(info)).getText();
									sr.Write(TmpStr);
									continue;
								}
								else if (FType.FieldType.FullName.EndsWith("ASCF"))
									TmpStr = ((ASCF)FType.GetValue(info)).Text;
								else if (FType.FieldType.FullName.EndsWith("HEX"))
									TmpStr = ((HEX)FType.GetValue(info)).Text;
								else if (FType.FieldType.FullName.EndsWith("UNICODE"))
									TmpStr = ((UNICODE)FType.GetValue(info)).Text;
								else
									TmpStr = FType.GetValue(info).ToString();
							}
							if (TmpStr.IndexOf('"') > -1 || TmpStr.IndexOf(',') > -1 ||
								TmpStr.IndexOf('\r') > -1 || TmpStr.IndexOf('\n') > -1 ||
								TmpStr.StartsWith(" ") || TmpStr.StartsWith("\t") ||
								TmpStr.EndsWith(" ") || TmpStr.EndsWith("\t"))
							{
								if (TmpStr.IndexOf('"') > -1)
								{
									TmpStr = TmpStr.Replace("\"", "\"\"");
								}
								if (TmpStr.IndexOf('\n') > -1)
								{
									TmpStr = TmpStr.Replace("\n", " ");
								}
								TmpStr = "\"" + TmpStr + "\"";
							}
							sr.Write(TmpStr);
							if (j < this.DatInfo.getFieldNames().Count - 1)
								sr.Write('\t');
						}
						sr.Write("\r\n");
						this.StatusProgress.Value = i;
						RecNo++;
					}
					sr.Close();
				}
				else
					return;
			}
			catch (Exception ex)
			{
				ex = new ApplicationException(
					String.Format("Error exporting string file (RecNo: {0} FieldName: {1})",
								  RecNo, FName), ex);
				Program.log.Add(ex, true);
			}
			finally
			{
				this.Enabled = true;
				this.FileNameCombo.Enabled = true;
				this.StatusProgress.Visible = false;
			}
			this.StatusLabel.Text = Program.language.getMessage(MsgList.COMPLETE) +
										String.Format(Program.language.getMessage(MsgList.CompleteExp), this.DatDatas.Count);
		}

		#endregion

		#region Menu Events

		private void MenuFileOpen_Click(object sender, EventArgs e)
		{
			Program.config.LoadLastFolder(this.DirectoryDialog, "DirectoryDialog");
			if (this.DirectoryDialog.ShowDialog() == DialogResult.OK)
			{
				Program.config.SaveLastFolder(this.DirectoryDialog, "DirectoryDialog");
				Program.config.LineAgeDirectory = this.DirectoryDialog.SelectedPath;
				this.Forms_Init(true);
				this.Forms_Update();
			}
		}

		private void MenuFileExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void MenuOption_Click(object sender, EventArgs e)
		{
			OptionForm dlg = new OptionForm();
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				this.selectedDatVersion = (DatVersion)Program.config.ChronicleSetting;
				Program.language.Reload();
				this.Forms_Init(true);
				this.Forms_Update();
			}
		}

		private void MenuL2encdecItem1_Click(object sender, EventArgs e)
		{
			try
			{
				this.StatusLabel.Text = Program.language.getMessage(MsgList.PleaseWait);
				this.FileNameCombo.Enabled = false;
				this.Enabled = false;

				this.StatusProgress.Minimum = 0;
				this.StatusProgress.Value = 0;
				this.StatusProgress.Visible = true;

				L2EncDec.SystemPatcher();
			}
			catch (Exception ex)
			{
				Program.log.Add(ex, true);
			}
			finally
			{
				this.Enabled = true;
				this.FileNameCombo.Enabled = true;
				this.StatusProgress.Visible = false;
			}
			this.StatusLabel.Text = Program.language.getMessage(MsgList.COMPLETE) +
										String.Format(Program.language.getMessage(MsgList.CompletePatchSystem));
		}

		private void MenuIniItems_Click(object sender, EventArgs e)
		{
			try
			{
				this.FileNameCombo.Enabled = false;
				this.Enabled = false;

				BinaryReader f = L2EncDec.Decrypt(((ToolStripMenuItem)sender).Text, Encoding.Default);
				if (f == null)
					return;
				char[] IniText = f.ReadChars((int)f.BaseStream.Length);
				f.Close();

				IniEditBox dlg = new IniEditBox(((ToolStripMenuItem)sender).Text, new String(IniText));
				dlg.ShowDialog();
			}
			catch (Exception ex)
			{
				Program.log.Add(ex, true);
			}
			finally
			{
				this.Enabled = true;
				this.FileNameCombo.Enabled = true;
			}
		}

		private void MenuAbout_Click(object sender, EventArgs e)
		{
			AboutForm dlg = new AboutForm();
			dlg.ShowDialog();
		}

		#endregion
	}
}