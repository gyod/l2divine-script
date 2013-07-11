#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using L2DatEncDec.Parsers;

#endregion

namespace L2DatEncDec.Tools
{
	#region L2EncDec

	public class L2EncDec
	{
		public static BinaryReader Decrypt (string fname, Encoding enc)
		{
			Process process_decoder = new Process();
			string decoder_output = "";

			string fname_encoded = Path.Combine(Program.config.LineAgeDirectory, fname);
			string fname_decoded = Path.GetTempFileName();

			try
			{
				process_decoder.StartInfo.FileName = Path.Combine(LmUtils.GlobalUtilities.GetStartDirectory(false), @"l2encdec\l2encdec.exe");
				process_decoder.StartInfo.UseShellExecute = false;
				process_decoder.StartInfo.CreateNoWindow = true;
				process_decoder.StartInfo.RedirectStandardOutput = true;
				process_decoder.StartInfo.Arguments = String.Format("-s \"{0}\" \"{1}\"", fname_encoded, fname_decoded);
				process_decoder.Start();
				process_decoder.WaitForExit();

				if (process_decoder.ExitCode < 0)
				{
					// lets try with -g switch
					process_decoder.StartInfo.Arguments = String.Format("-g \"{0}\" \"{1}\"", fname_encoded, fname_decoded);
					process_decoder.Start();
					process_decoder.WaitForExit();

					if (process_decoder.ExitCode < 0)
					{
						// guess this file is not encrypted
						fname_decoded = fname_encoded;
					}
				}

				try
				{
					decoder_output = process_decoder.StandardOutput.ReadToEnd();
				}
				catch
				{
				}

				return new BinaryReader(File.OpenRead (fname_decoded), enc);
			}
			catch (Exception ex)
			{
				throw new ApplicationException (
					String.Format("Error decoding '{0}':\n\n{1}", fname_encoded, decoder_output), ex);
			}
			finally
			{
				process_decoder.Dispose();
			}
		}

		public static void Encrypt (string fname)
		{
			if (String.IsNullOrEmpty(Program.config.LineAge_EncryptParameters))
				return;

			Process process_encoder = new Process();
			string encoder_output = "";

			string fname_encoded = Path.Combine(Program.config.LineAgeDirectory, fname);
			string fname_decoded = Path.ChangeExtension(fname_encoded, "tmp");

			try
			{
				File.Delete(fname_decoded);
				File.Move(fname_encoded, fname_decoded);

				process_encoder.StartInfo.FileName = Path.Combine(LmUtils.GlobalUtilities.GetStartDirectory(false), @"l2encdec\l2encdec.exe");
				process_encoder.StartInfo.UseShellExecute = false;
				process_encoder.StartInfo.CreateNoWindow = true;
				process_encoder.StartInfo.RedirectStandardOutput = true;
				process_encoder.StartInfo.Arguments = String.Format("{0} \"{1}\" \"{2}\"", Program.config.LineAge_EncryptParameters, fname_decoded, fname_encoded);
				process_encoder.Start();
				process_encoder.WaitForExit();

				try
				{
					encoder_output = process_encoder.StandardOutput.ReadToEnd();
				}
				catch
				{
				}

				File.Delete(fname_decoded);
			}
			catch (Exception ex)
			{
				try
				{
					File.Move(fname_decoded, fname_encoded);
				}
				catch
				{
				}

				throw new ApplicationException (
					String.Format("Error encoding '{0}':\n\n{1}", fname_decoded, encoder_output), ex);
			}
			finally
			{
				process_encoder.Dispose();
			}
		}

		public static void SystemPatcher()
		{
			Process process_patcher = new Process();
			string patcher_output = "";

			string fname_l2encdec = Path.Combine(LmUtils.GlobalUtilities.GetStartDirectory(false), @"l2encdec\l2encdec.exe");
			string fname_libdll = Path.Combine(LmUtils.GlobalUtilities.GetStartDirectory(false), @"l2encdec\libgmp-10.dll");
			string fname_libzdll = Path.Combine(LmUtils.GlobalUtilities.GetStartDirectory(false), @"l2encdec\libz-1.dll");
			string fname_patcher = Path.Combine(LmUtils.GlobalUtilities.GetStartDirectory(false), @"l2encdec\patcher.exe");
			string fname_loader = Path.Combine(LmUtils.GlobalUtilities.GetStartDirectory(false), @"l2encdec\loaderCT1.exe");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
				fname_loader = Path.Combine(LmUtils.GlobalUtilities.GetStartDirectory(false), @"l2encdec\loaderCT1++.exe");
			string l2exe_file = "";

			try
			{
				// Create Backup Folder
				Boolean back_flg = false;
				string backup_folder = Path.Combine(Program.config.LineAgeDirectory, "backup");
				if (Directory.Exists(backup_folder))
					back_flg = true;
				else
					Directory.CreateDirectory(backup_folder);
				
				// Create Temp Folder
				string temp_folder = Path.Combine(Program.config.LineAgeDirectory, "tmp");
				if (Directory.Exists(temp_folder))
					Directory.Delete(temp_folder, true);
				Directory.CreateDirectory(temp_folder);

				// Install l2encdec.exe
				File.Copy(fname_l2encdec, Path.Combine(Program.config.LineAgeDirectory, Path.GetFileName(fname_l2encdec)), true);
				File.Copy(fname_libdll, Path.Combine(Program.config.LineAgeDirectory, Path.GetFileName(fname_libdll)), true);
				File.Copy(fname_libzdll, Path.Combine(Program.config.LineAgeDirectory, Path.GetFileName(fname_libzdll)), true);
				process_patcher.StartInfo.WorkingDirectory = Program.config.LineAgeDirectory;
				process_patcher.StartInfo.FileName = Path.Combine(Program.config.LineAgeDirectory, Path.GetFileName(fname_l2encdec));
				process_patcher.StartInfo.UseShellExecute = false;
				process_patcher.StartInfo.CreateNoWindow = true;
				process_patcher.StartInfo.RedirectStandardOutput = true;

				// Decrypt & Encrypt by l2encdec.exe
				DirectoryInfo current = new DirectoryInfo(Program.config.LineAgeDirectory);
				Program.main_form.StatusProgress.Maximum = current.GetFiles().Length;
				foreach (FileInfo file in current.GetFiles())
				{
					string current_file = Path.GetFileName(file.FullName);
					string backup_file = Path.Combine(backup_folder, current_file);
					
					// Check Header
					BinaryReader f = new BinaryReader(File.OpenRead(file.FullName), Encoding.Default);
					string FileHeader = "";
					if (f.BaseStream.Length > 28)
					{
						byte[] buf = f.ReadBytes(28);
						for (int i = 0; i < 28; i++)
						{
							if (buf[i] == 0x00) continue;
							FileHeader += Convert.ToChar(buf[i]);
						}
					}
					f.Close();
					if (FileHeader == "Lineage2Ver413")
					{
						if (!back_flg)
							File.Copy(file.FullName, backup_file, true);

						// Decrypt
						process_patcher.StartInfo.Arguments = String.Format("-s \"{0}\" \"{1}\"",
												current_file, Path.ChangeExtension(current_file, ".dec"));
						process_patcher.Start();
						process_patcher.WaitForExit();
						if (process_patcher.ExitCode < 0)
						{
							// lets try with -g switch
							process_patcher.StartInfo.Arguments = String.Format("-g \"{0}\" \"{1}\"",
													current_file, Path.ChangeExtension(current_file, ".dec"));
							process_patcher.Start();
							process_patcher.WaitForExit();
							if (process_patcher.ExitCode < 0)
							{
								new MessageBox(String.Format("Error patching.\n\n{0}", patcher_output));
								break;
							}
						}

						// Encrypt
						process_patcher.StartInfo.Arguments = String.Format("{0} \"{1}\" \"{2}\"",
												Program.config.LineAge_EncryptParameters,
												Path.ChangeExtension(current_file, ".dec"), current_file);
						process_patcher.Start();
						process_patcher.WaitForExit();
						if (process_patcher.ExitCode < 0)
						{
							new MessageBox(String.Format("Error patching.\n\n{0}", patcher_output));
							break;
						}

						File.Delete(Path.Combine(Program.config.LineAgeDirectory, Path.ChangeExtension(current_file, ".dec")));
					}
					else
					{
						if (current_file.ToLower() == "l2.exe")
						{
							l2exe_file = current_file;
							File.Copy(file.FullName, Path.Combine(temp_folder, current_file));
							if (!back_flg)
								File.Copy(file.FullName, backup_file, true);
						}
					}
					Program.main_form.StatusProgress.Value += 1;
				}

				if (l2exe_file != "")
				{
					// Install patcher.exe 
					File.Copy(fname_patcher, Path.Combine(temp_folder, Path.GetFileName(fname_patcher)));
					process_patcher.StartInfo.WorkingDirectory = temp_folder;
					process_patcher.StartInfo.FileName = Path.Combine(temp_folder, Path.GetFileName(fname_patcher));
					process_patcher.StartInfo.Arguments = "-f";
					process_patcher.Start();
					process_patcher.WaitForExit();
					File.Copy(Path.Combine(temp_folder, l2exe_file), Path.Combine(Program.config.LineAgeDirectory, l2exe_file), true);

					// Install loaderCT1.exe
					if (Program.main_form.selectedDatVersion >= L2DatEncDec.Parsers.DatVersion.ChaoticThrone1_0_Kamael)
					{
						File.Copy(fname_loader, Path.Combine(Program.config.LineAgeDirectory, Path.GetFileName(fname_loader)), true);
					}
				}
				
				// Cleanup Files
				File.Delete(Path.Combine(Program.config.LineAgeDirectory, "l2encdec.exe"));
				File.Delete(Path.Combine(Program.config.LineAgeDirectory, "libgmp-10.dll"));
				File.Delete(Path.Combine(Program.config.LineAgeDirectory, "libz-1.dll"));
				if (Directory.Exists(temp_folder))
					Directory.Delete(temp_folder, true);
			}
			catch (Exception ex)
			{
				throw new ApplicationException(
					String.Format("Error patching.\n\n{0}", patcher_output), ex);
			}
			finally
			{
				process_patcher.Dispose();
			}
		}
	}

	#endregion

	#region L2DatTool

	class L2DatTool
	{
		public const char DELIMITER = '|';

		public static int ReadByteCount(BinaryReader f)
		{
			byte tmp = f.ReadByte();
			int len = tmp & 0x3F;
			if ((tmp & 0x40) > 0)
			{
				tmp = f.ReadByte();
				len += tmp << 6;
			}
			return len;
		}

		public static string ReadString(BinaryReader f)
		{
			int len = f.ReadByte();
			if (len == 0)
				return "";
			if (len >= 192)
				f.BaseStream.Seek(1, SeekOrigin.Current);
			long start_pos = f.BaseStream.Position;
			long end_pos = start_pos;

			Encoding enc = Encoding.Default;
			byte one_ch_len = 1;

			if (len >= 128)
			{
				// unicode string
				enc = Encoding.Unicode;
				one_ch_len = 2;

				while (true)
				{
					short ch = f.ReadInt16();
					if (ch == 0)
					{
						end_pos = f.BaseStream.Position;
						break;
					}
				}
			}
			else
			{
				if (char.IsControl((char) f.PeekChar()))
				{
					f.BaseStream.Seek(1, SeekOrigin.Current);
					start_pos = f.BaseStream.Position;
				}

				// ansi string
				while (true)
				{
					byte ch = f.ReadByte();
					if (ch == 0)
					{
						end_pos = f.BaseStream.Position;
						break;
					}
				}
			}

			long real_len = end_pos - start_pos - one_ch_len;

			f.BaseStream.Seek (start_pos, SeekOrigin.Begin);

			byte[] buf = f.ReadBytes((int) real_len);
			string res = enc.GetString(buf);

			f.BaseStream.Seek(one_ch_len, SeekOrigin.Current);

			return res;
		}

		public static string ReadStringSimple_UnicodeInt32Length(BinaryReader f)
		{
			long SavePos = f.BaseStream.Position;
			int len = f.ReadInt32();
			if (len > 10000)
			{
				f.BaseStream.Seek(SavePos, SeekOrigin.Begin);
				Console.Out.WriteLine("!!!!! [WARNING] !!!!!");
				L2DatTool.Debug_ByteDump(f, 32);
			}
			if (len == 0)
				return "";

			byte[] buf = new byte[len];
			f.Read(buf, 0, (int) len);

			return Encoding.Unicode.GetString(buf);
		}

		public static void WriteByteCount(BinaryWriter f, int len)
		{
			if (len > 0x3F)
			{
				int LByte = len & 0x3F;
				int HByte = len >> 6;
				f.Write((byte)(LByte | 0x40));
				f.Write((byte)HByte);
			}
			else
			{
				f.Write((byte)len);
			}
		}

		public static void WriteString(BinaryWriter f, string str)
		{
			if (!String.IsNullOrEmpty (str))
			{
				bool is_unicode = false;
				for (int k = 0; k < str.Length; k++)
				{
					int code = (int) str[k];
					if (code > 255 && code != 1081)
					{
						is_unicode = true;
						break;
					}
				}

				int len = str.Length + 1;
				if (len >= 64)
				{
					int len_part2 = len / 64;
					int len_part1 = len - ((len_part2 - 1) * 64);

					// set highest bit cause to indicate it is Unicode
					if (is_unicode)
						len_part1 += 128;

					f.Write((byte) len_part1);
					f.Write((byte) len_part2);
				}
				else
				{
					// set highest bit cause to indicate it is Unicode
					if (is_unicode)
						len += 128;

					f.Write((byte) len);
				}

				if (is_unicode)
				{
					f.Write(Encoding.Unicode.GetBytes(str));
					f.Write((byte) 0x00);
				}
				else
				{
					f.Write(Encoding.Default.GetBytes(str));
				}
			}

			f.Write((byte) 0x00);
		}

		public static void WriteStringSimple_UnicodeInt32Length(BinaryWriter f, string str)
		{

			f.Write((UInt32)(str.Length*2));

			if (str.Length > 0)
				f.Write(Encoding.Unicode.GetBytes(str));
		}

		public static void Debug_ByteDump(BinaryReader f, int cnt)
		{
			long SavePos = f.BaseStream.Position;
			Console.Out.WriteLine("+00 +01 +02 +03 +04 +05 +06 +07 +08 +09 +0A +0B +0C +0D +0E +0F");
			Console.Out.WriteLine("---------------------------------------------------------------");
			for (int i = 0, j = 1; i < cnt; i++, j++)
			{
				if (f.BaseStream.Position >= f.BaseStream.Length)
					break;
				byte temp = f.ReadByte();
				Console.Out.Write(String.Format(" {0:X2} ", temp));
				if (j % 16 == 0)
					Console.Out.Write("\n");
			}
			Console.Out.Write("\n");
			f.BaseStream.Seek(SavePos, SeekOrigin.Begin);
		}

		public static string Debug_DumpString(BinaryReader f, long pos, int cnt)
		{
			string ret = "";
			f.BaseStream.Seek(pos, SeekOrigin.Begin);
			for (int i = 0; i < cnt; i++)
			{
				if (f.BaseStream.Position >= f.BaseStream.Length)
					break;
				byte temp = f.ReadByte();
				ret += String.Format("{0:X2}:", temp);
			}
			return ret;
		}
	}

	#endregion

	#region L2DatFieldType

	public class CNTINT_PAIR
	{
		private int cnt;
		private string text; // [cnt]

		public string getHeaderText(string prefix)
		{
			return prefix + "_cnt\t" +
				   prefix + "_text\t";
		}

		public string getText()
		{
			string res = "";
			res += cnt + "\t";
			if (cnt > 1)
				res += "\"" + text + "\"\t";
			else
				res += text + "\t";
			return res;
		}

		public void setText(string[] value)
		{
			cnt = Convert.ToInt32(value[0]);
			text = value[1];
		}

		public int getFieldCount()
		{
			return 2;
		}

		public static CNTINT_PAIR Parse(BinaryReader f)
		{
			CNTINT_PAIR info = new CNTINT_PAIR();
			info.cnt = f.ReadInt32();
			info.text = "";
			for (int i = 0; i < info.cnt; i++)
			{
				info.text += f.ReadInt32().ToString();
				if (i < info.cnt - 1)
					info.text += L2DatTool.DELIMITER;
			}
			return info;
		}

		public static void Compile(BinaryWriter f, CNTINT_PAIR info)
		{
			string[] TmpStr = info.text.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt);
			for (int i = 0; i < info.cnt; i++)
			{
				f.Write(Convert.ToInt32(TmpStr[i]));
			}
		}
	}

	public class CNTRINT_PAIR
	{
		private int cntr;
		private string text; // [cnt]

		public string getHeaderText(string prefix)
		{
			return prefix + "_cntr\t" +
				   prefix + "_text\t";
		}

		public string getText()
		{
			string res = "";
			res += cntr + "\t";
			if (cntr > 1)
				res += "\"" + text + "\"\t";
			else
				res += text + "\t";
			return res;
		}

		public void setText(string[] value)
		{
			cntr = Convert.ToInt32(value[0]);
			text = value[1];
		}

		public int getFieldCount()
		{
			return 2;
		}

		public static CNTRINT_PAIR Parse(BinaryReader f)
		{
			CNTRINT_PAIR info = new CNTRINT_PAIR();
			info.cntr = L2DatTool.ReadByteCount(f);
			info.text = "";
			for (int i = 0; i < info.cntr; i++)
			{
				info.text += f.ReadInt32().ToString();
				if (i < info.cntr - 1)
					info.text += L2DatTool.DELIMITER;
			}
			return info;
		}

		public static void Compile(BinaryWriter f, CNTRINT_PAIR info)
		{
			if (info.text.Length > 0)
			{
				string[] TmpStr = info.text.Split(new char[] { L2DatTool.DELIMITER });
				L2DatTool.WriteByteCount(f, info.cntr);
				for (int i = 0; i < info.cntr; i++)
				{
					f.Write(Convert.ToInt32(TmpStr[i]));
				}
			}
			else
				L2DatTool.WriteByteCount(f, 0);
		}
	}

	public class CNTTXT_PAIR
	{
		private int cnt;
		public int length
		{
			get
			{
				return cnt;
			}
		}
		private string text; // [cnt]

		public string getHeaderText(string prefix)
		{
			return prefix + "_cnt\t" +
				   prefix + "_text\t";
		}

		public string getText()
		{
			string res = "";
			res += cnt + "\t";
			if (cnt > 1)
				res += "\"" + text + "\"\t";
			else
				res += text + "\t";
			return res;
		}

		public void setText(string[] value)
		{
			cnt = Convert.ToInt32(value[0]);
			text = value[1];
		}

		public int getFieldCount()
		{
			return 2;
		}

		public static CNTTXT_PAIR Parse(BinaryReader f)
		{
			CNTTXT_PAIR info = new CNTTXT_PAIR();
			info.cnt = f.ReadInt32();
			info.text = "";
			for (int i = 0; i < info.cnt; i++)
			{
				info.text += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
				if (i < info.cnt - 1)
					info.text += L2DatTool.DELIMITER;
			}
			return info;
		}

		public static void Compile(BinaryWriter f, CNTTXT_PAIR info)
		{
			string[] TmpStr = info.text.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt);
			for (int i = 0; i < info.cnt; i++)
			{
				L2DatTool.WriteStringSimple_UnicodeInt32Length(f, TmpStr[i]);
			}
		}
	}

	public class CNTTXT_PAIR2
	{
		private int cnt;
		public int length
		{
			get
			{
				return cnt;
			}
		}
		private string text1;  // [cnt]
		private string value1; // [cnt]

		public string getHeaderText(string prefix)
		{
			return prefix + "_cnt\t" +
				   prefix + "_text1\t" +
				   prefix + "_value1\t";
		}

		public string getText()
		{
			string res = "";
			res += cnt + "\t";
			if (cnt > 1)
			{
				res += "\"" + text1 + "\"\t";
				res += "\"" + value1 + "\"\t";
			}
			else
			{
				res += text1 + "\t";
				res += value1 + "\t";
			}
			return res;
		}

		public void setText(string[] value)
		{
			cnt = Convert.ToInt32(value[0]);
			text1 = value[1];
			value1 = value[2];
		}

		public int getFieldCount()
		{
			return 3;
		}

		public static CNTTXT_PAIR2 Parse(BinaryReader f)
		{
			CNTTXT_PAIR2 info = new CNTTXT_PAIR2();
			info.cnt = f.ReadInt32();
			info.text1 = "";
			for (int i = 0; i < info.cnt; i++)
			{
				info.text1 += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
				if (i < info.cnt - 1)
					info.text1 += L2DatTool.DELIMITER;
			}
			info.value1 = "";
			for (int i = 0; i < info.cnt; i++)
			{
				info.value1 += f.ReadInt32().ToString();
				if (i < info.cnt - 1)
					info.value1 += L2DatTool.DELIMITER;
			}
			return info;
		}

		public static void Compile(BinaryWriter f, CNTTXT_PAIR2 info)
		{
			string[] TmpStr = info.text1.Split(new char[] { L2DatTool.DELIMITER });
			string[] TmpStr2 = info.value1.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt);
			for (int i = 0; i < info.cnt; i++)
			{
				L2DatTool.WriteStringSimple_UnicodeInt32Length(f, TmpStr[i]);
			}
			for (int i = 0; i < info.cnt; i++)
			{
				f.Write(Convert.ToInt32(TmpStr2[i]));
			}
		}
	}

	public class CNTASCF_PAIR
	{
		private int cnt;
		public int length
		{
			get
			{
				return cnt;
			}
		}
		private string text; // [cnt]

		public string getHeaderText(string prefix)
		{
			return prefix + "_cnt\t" +
				   prefix + "_text\t";
		}

		public string getText()
		{
			string res = "";
			res += cnt + "\t";
			if (cnt > 1)
				res += "\"" + text + "\"\t";
			else
				res += text + "\t";
			return res;
		}

		public void setText(string[] value)
		{
			cnt = Convert.ToInt32(value[0]);
			text = value[1];
		}

		public int getFieldCount()
		{
			return 2;
		}

		public static CNTASCF_PAIR Parse(BinaryReader f)
		{
			CNTASCF_PAIR info = new CNTASCF_PAIR();
			info.cnt = f.ReadInt32();
			info.text = "";
			for (int i = 0; i < info.cnt; i++)
			{
				info.text += L2DatTool.ReadString(f);
				if (i < info.cnt - 1)
					info.text += L2DatTool.DELIMITER;
			}
			return info;
		}

		public static void Compile(BinaryWriter f, CNTASCF_PAIR info)
		{
			string[] TmpStr = info.text.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt);
			for (int i = 0; i < info.cnt; i++)
			{
				L2DatTool.WriteString(f, TmpStr[i]);
			}
		}
	}

	public class MTX
	{
		private int cnt1;
		private string text1; // [cnt1]
		private int cnt2;
		private string text2; // [cnt2]

		public string getHeaderText(string prefix)
		{
			return prefix + "_cnt1\t" + 
				   prefix + "_text1\t" +
				   prefix + "_cnt2\t" +
				   prefix + "_text2\t";
		}

		public string getText()
		{
			string res = "";
			res += cnt1 + "\t";
			if (cnt1 > 1)
				res += "\"" + text1 + "\"\t";
			else
				res += text1 + "\t";
			res += cnt2 + "\t";
			if (cnt2 > 1)
				res += "\"" + text2 + "\"\t";
			else
				res += text2 + "\t";
			return res;
		}

		public void setText(string [] value)
		{
			cnt1 = Convert.ToInt32(value[0]);
			text1 = value[1];
			cnt2 = Convert.ToInt32(value[2]);
			text2 = value[3];
		}

		public int getFieldCount()
		{
			return 4;
		}

		public static MTX Parse(BinaryReader f)
		{
			MTX info = new MTX();
			info.cnt1 = f.ReadInt32();
			info.text1 = "";
			for (int i = 0; i < info.cnt1; i++)
			{
				info.text1 += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
				if (i < info.cnt1 - 1)
					info.text1 += L2DatTool.DELIMITER;
			}
			info.cnt2 = f.ReadInt32();
			info.text2 = "";
			for (int i = 0; i < info.cnt2; i++)
			{
				info.text2 += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
				if (i < info.cnt2 - 1)
					info.text2 += L2DatTool.DELIMITER;
			}
			return info;
		}

		public static void Compile(BinaryWriter f, MTX info)
		{
			string[] TmpStr = info.text1.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt1);
			for (int i = 0; i < info.cnt1; i++)
			{
				L2DatTool.WriteStringSimple_UnicodeInt32Length(f, TmpStr[i]);
			}
			TmpStr = info.text2.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt2);
			for (int i = 0; i < info.cnt2; i++)
			{
				L2DatTool.WriteStringSimple_UnicodeInt32Length(f, TmpStr[i]);
			}
		}
	}

	public class MTX2
	{
		private int cnt1;
		private string text1;	// [cnt1]
		private string value1;   // [cnt1]
		private string unknown1; // [cnt1]
		private int cnt2;
		private string text2;	// [cnt2]

		public string getHeaderText(string prefix)
		{
			return prefix + "_cnt1\t" +
				   prefix + "_text1\t" +
				   prefix + "_value1\t" +
				   prefix + "_unknown1\t" +
				   prefix + "_cnt2\t" +
				   prefix + "_text2\t";
		}

		public string getText()
		{
			string res = "";
			res += cnt1 + "\t";
			if (cnt1 > 1)
			{
				res += "\"" + text1 + "\"\t";
				res += "\"" + value1 + "\"\t";
				res += "\"" + unknown1 + "\"\t";
			}
			else
			{
				res += text1 + "\t";
				res += value1 + "\t";
				res += unknown1 + "\t";
			}
			res += cnt2 + "\t";
			if (cnt2 > 1)
				res += "\"" + text2 + "\"\t";
			else
				res += text2 + "\t";
			return res;
		}

		public void setText(string[] value)
		{
			cnt1 = Convert.ToInt32(value[0]);
			text1 = value[1];
			value1 = value[2];
			unknown1 = value[3];
			cnt2 = Convert.ToInt32(value[4]);
			text2 = value[5];
		}

		public int getFieldCount()
		{
			return 6;
		}

		public static MTX2 Parse(BinaryReader f)
		{

			MTX2 info = new MTX2();
			info.cnt1 = f.ReadInt32();
			info.text1 = "";
			info.unknown1 = "";
			for (int i = 0; i < info.cnt1; i++)
			{
				info.text1 += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
				if (i < info.cnt1 - 1)
					info.text1 += L2DatTool.DELIMITER;

				info.value1 += f.ReadInt32().ToString();
				if (i < info.cnt1 - 1)
					info.value1 += L2DatTool.DELIMITER;

				byte[] temp = f.ReadBytes(1);
				info.unknown1 += BitConverter.ToString(temp);
				if (i < info.cnt1 - 1)
					info.unknown1 += L2DatTool.DELIMITER;
			}
			info.cnt2 = f.ReadInt32();
			info.text2 = "";
			for (int i = 0; i < info.cnt2; i++)
			{
				info.text2 += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
				if (i < info.cnt2 - 1)
					info.text2 += L2DatTool.DELIMITER;
			}
			return info;
		}

		public static void Compile(BinaryWriter f, MTX2 info)
		{
			string[] TmpStr = info.text1.Split(new char[] { L2DatTool.DELIMITER });
			string[] TmpStr2 = info.value1.Split(new char[] { L2DatTool.DELIMITER });
			string[] TmpStr3 = info.unknown1.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt1);
			for (int i = 0; i < info.cnt1; i++)
			{
				L2DatTool.WriteStringSimple_UnicodeInt32Length(f, TmpStr[i]);
				f.Write(TmpStr2[i]);
				f.Write(Convert.ToByte(TmpStr3[i], 16));
			}
			TmpStr = info.text2.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt2);
			for (int i = 0; i < info.cnt2; i++)
			{
				L2DatTool.WriteStringSimple_UnicodeInt32Length(f, TmpStr[i]);
			}
		}
	}

	public class MTX3
	{
		private int cnt1;
		private string text1;	// [cnt1]
		private string unknown1; // [cnt1]
		private int cnt2;
		private string text2;	// [cnt2]
		private string text3;

		public string getHeaderText(string prefix)
		{
			return prefix + "_cnt1\t" +
				   prefix + "_text1\t" +
				   prefix + "_unknown1\t" +
				   prefix + "_cnt2\t" +
				   prefix + "_text2\t" +
				   prefix + "_text3\t";
		}

		public string getText()
		{
			string res = "";
			res += cnt1 + "\t";
			if (cnt1 > 1)
			{
				res += "\"" + text1 + "\"\t";
				res += "\"" + unknown1 + "\"\t";
			}
			else
			{
				res += text1 + "\t";
				res += unknown1 + "\t";
			}
			res += cnt2 + "\t";
			if (cnt2 > 1)
				res += "\"" + text2 + "\"\t";
			else
				res += text2 + "\t";
			res += text3 + "\t";
			return res;
		}

		public void setText(string[] value)
		{
			cnt1 = Convert.ToInt32(value[0]);
			text1 = value[1];
			unknown1 = value[2];
			cnt2 = Convert.ToInt32(value[3]);
			text2 = value[4];
			text3 = value[5];
		}

		public int getFieldCount()
		{
			return 6;
		}

		public static MTX3 Parse(BinaryReader f)
		{

			MTX3 info = new MTX3();
			info.cnt1 = f.ReadInt32();
			info.text1 = "";
			info.unknown1 = "";
			for (int i = 0; i < info.cnt1; i++)
			{
				info.text1 += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
				if (i < info.cnt1 - 1)
					info.text1 += L2DatTool.DELIMITER;

				byte[] temp = f.ReadBytes(2);
				info.unknown1 += BitConverter.ToString(temp);
				if (i < info.cnt1 - 1)
					info.unknown1 += L2DatTool.DELIMITER;
			}
			info.cnt2 = f.ReadInt32();
			info.text2 = "";
			for (int i = 0; i < info.cnt2; i++)
			{
				info.text2 += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
				if (i < info.cnt2 - 1)
					info.text2 += L2DatTool.DELIMITER;
			}
			info.text3 += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
			return info;
		}

		public static void Compile(BinaryWriter f, MTX3 info)
		{
			string[] TmpStr = info.text1.Split(new char[] { L2DatTool.DELIMITER });
			string[] TmpStr2 = info.unknown1.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt1);
			for (int i = 0; i < info.cnt1; i++)
			{
				L2DatTool.WriteStringSimple_UnicodeInt32Length(f, TmpStr[i]);
				string[] TmpStr3 = TmpStr2[i].Split(new char[] { '-' });
				for (int j = 0; j < TmpStr3.Length; j++)
				{
					f.Write(Convert.ToByte(TmpStr3[j], 16));
				}
			}
			TmpStr = info.text2.Split(new char[] { L2DatTool.DELIMITER });
			f.Write(info.cnt2);
			for (int i = 0; i < info.cnt2; i++)
			{
				L2DatTool.WriteStringSimple_UnicodeInt32Length(f, TmpStr[i]);
			}
			L2DatTool.WriteStringSimple_UnicodeInt32Length(f, info.text3);
		}
	}

	public class UNICODE
	{
		private string text = "";
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		public static UNICODE Parse(BinaryReader f)
		{
			UNICODE TmpStr = new UNICODE();
			TmpStr.Text = L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
			return TmpStr;
		}

		public static void Compile(BinaryWriter f, UNICODE info)
		{
			L2DatTool.WriteStringSimple_UnicodeInt32Length(f, info.Text);
		}
	}

	public class ASCF
	{
		private string text = "";
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		public static ASCF Parse(BinaryReader f)
		{
			ASCF TmpStr = new ASCF();
			TmpStr.Text = L2DatTool.ReadString(f);
			return TmpStr;
		}

		public static void Compile(BinaryWriter f, ASCF info)
		{
			L2DatTool.WriteString(f, info.Text);
		}
	}

	public class HEX
	{
		private string text = "";
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		public static HEX Parse(BinaryReader f)
		{
			HEX TmpStr = new HEX();
			byte[] temp = f.ReadBytes(4);
			TmpStr.Text = BitConverter.ToString(temp);
			return TmpStr;
		}

		public static void Compile(BinaryWriter f, HEX info)
		{
			string[] TmpStr = info.text.Split(new char[] { '-' });
			for (int i = 0; i < TmpStr.Length; i++)
			{
				f.Write(Convert.ToByte(TmpStr[i], 16));
			}
		}
	}

	#endregion
}
