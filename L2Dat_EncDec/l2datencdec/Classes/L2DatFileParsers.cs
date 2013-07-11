#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using L2DatEncDec.Definitions;
using L2DatEncDec.Tools;

#endregion

namespace L2DatEncDec.Parsers
{
	public enum DatVersion
	{
		Chronicle3,
		Chronicle4,
		Chronicle5,
		Interlude,
		ChaoticThrone1_0_Kamael,
		ChaoticThrone1_5_Hellbound,
		ChaoticThrone2_1_Gracia,
		ChaoticThrone2_2_Part2,
		ChaoticThrone2_3_Final,
		ChaoticThrone2_4_Epilogue,
		ChaoticThrone2_5_Freya,
		ChaoticThrone2_6_HighFive
	}

	public enum DatFileType
	{
		// DDF で RECCNT = OFF; 以外のデータはうまく解析できない
		// it cannot analyze that not defined RECCNT = OFF at the DDF
		ActionName,
		AdditionalEffect,
		Armorgrp,
		Britemgrp,
		CastleName,
//		Charcreategrp,	// RECCNT = 20;
		Chargrp,		// RECCNT = 17;
		ClassInfo,
		CommandName,
		Creditgrp,
		Entereventgrp,
		Etcitemgrp,
//		Eula,	// RECCNT = 1;
		GameTip,
		Hairaccessorylocgrp,
		Hennagrp,
		HuntingZone,
		InstantZoneData,
		ItemName,
		ItemPrime,
//		Logongrp,	// RECCNT = 8;
		Mobskillanimgrp,
		MusicInfo,
		Npcgrp,
		NpcName,
		NpcString,
		Obscene,
		Optiondata_Client,
		ProductName,
		QuestName,
		RaidData,
		Recipe,
		RideData,
		ScenePlayerData,
		ServerName,
		ShortcutAlias,
		Skillgrp,
		SkillName,
		Skillsoundgrp,
		SkillSoundSource,
		StaticObject,
		SymbolName,
		SysString,
		SystemMsg,
		TransformData,
		Variationeffectgrp,
		Vehiclepartsgrp,
		Weapongrp,
		ZoneName
	}

	#region L2DatParser

	public class L2DatParser
	{
		public L2DatParser()
		{
			DatDefFields = new List<String>();
		}

		static List<String> DatDefFields;
		public List<String> getFieldNames()
		{
			if (DatDefFields.Count == 0)
			{
				MemberInfo[] members = getDefinition().getMembers();
				foreach (MemberInfo m in members)
				{
					if (m.MemberType != MemberTypes.Field)
						continue;
					DatDefFields.Add(m.Name);
				}
			}
			return DatDefFields;
		}

		public virtual L2DatDefinition getDefinition() { return new L2DatDefinition(); }

		#region Parse

		public virtual List<L2DatDefinition> Parse(BinaryReader f)
		{
			List<L2DatDefinition> res = new List<L2DatDefinition>();

			int total_records = f.ReadInt32();
			Program.main_form.StatusProgress.Maximum = total_records;
			Console.Out.WriteLine("TOTAL:" + total_records);
			for (int i = 0; i < total_records; i++)
			{
				//Console.Out.WriteLine("----- RecordNo : " + (i + 1) + " -----");
				res.Add(ParseMain(f, i));
				Program.main_form.StatusProgress.Value = i;
			}

			return res;
		}

		public virtual L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			L2DatDefinition info = new L2DatDefinition();
			info = Program.main_form.DatInfo.getDefinition();
			List<String> TmpArr = Program.main_form.DatInfo.getFieldNames();
			for (int i = 0; i < TmpArr.Count; i++)
			{
				String FName = TmpArr[i];
				info = ReadFieldValue(f, info, FName);
			}
			return info;
		}

		public L2DatDefinition ReadFieldValue(BinaryReader f, L2DatDefinition info, String FromName, String ToName)
		{
			int startPos = 0, endPos = 0;
			List<String> TmpArr = Program.main_form.DatInfo.getFieldNames();
			for (int i = 0; i < TmpArr.Count; i++)
			{
				if (TmpArr[i] == FromName)
					startPos = i;
				if (TmpArr[i] == ToName)
					endPos = i;
			}
			for (int i = startPos; i <= endPos; i++)
				info = ReadFieldValue(f, info, i);
			return info;
		}

		public L2DatDefinition ReadFieldValue(BinaryReader f, L2DatDefinition info, int FNumber)
		{
			String FName = Program.main_form.DatInfo.getFieldNames()[FNumber];
			return ReadFieldValue(f, info, FName);
		}

		public L2DatDefinition ReadFieldValue(BinaryReader f, L2DatDefinition info, String FName)
		{
			L2DatDefinition TmpInfo = info;
			long curPos = f.BaseStream.Position;
			try
			{
				FieldInfo FType = Program.main_form.DatInfo.getDefinition().GetType().GetField(FName);
				if (FType.FieldType.FullName.EndsWith("UInt32"))
					FType.SetValue(info, f.ReadUInt32());
				else if (FType.FieldType.FullName.EndsWith("Int32"))
					FType.SetValue(info, f.ReadInt32());
				else if (FType.FieldType.FullName.EndsWith("Single"))
					FType.SetValue(info, f.ReadSingle());
				else if (FType.FieldType.FullName.EndsWith("Color"))
					FType.SetValue(info, Color.FromArgb(f.ReadInt32()));
				else if (FType.FieldType.FullName.EndsWith("CNTINT_PAIR"))
					FType.SetValue(info, CNTINT_PAIR.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("CNTRINT_PAIR"))
					FType.SetValue(info, CNTRINT_PAIR.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR"))
					FType.SetValue(info, CNTTXT_PAIR.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR2"))
					FType.SetValue(info, CNTTXT_PAIR2.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("CNTASCF_PAIR"))
					FType.SetValue(info, CNTASCF_PAIR.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("MTX"))
					FType.SetValue(info, MTX.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("MTX2"))
					FType.SetValue(info, MTX2.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("MTX3"))
					FType.SetValue(info, MTX3.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("ASCF"))
					FType.SetValue(info, ASCF.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("HEX"))
					FType.SetValue(info, HEX.Parse(f));
				else if (FType.FieldType.FullName.EndsWith("UNICODE"))
					FType.SetValue(info, UNICODE.Parse(f));
				else
				{
					Console.Out.WriteLine("!!!!! [WARNING] !!!!!");
					Console.Out.WriteLine("UnknownFieldType: " + FType.FieldType.FullName);
				}
			}
			catch (Exception ex)
			{
				TmpInfo.DumpFieldValues();
				ex = new ApplicationException(
					String.Format("Error parsing string file (FieldName: {0} RecordOffset: 0x{1:X} DumpData: {2})",
								  FName, f.BaseStream.Position, L2DatTool.Debug_DumpString(f, curPos, 8)), ex);
			}
			return info;
		}

		#endregion

		#region Compile

		public virtual void Compile(BinaryWriter f, List<L2DatDefinition> infos)
		{
			f.Write(infos.Count);

			int total_records = infos.Count;
			Program.main_form.StatusProgress.Maximum = total_records;
			Console.Out.WriteLine("TOTAL:" + total_records);
			for (int i = 0; i < total_records; i++)
			{
				//Console.Out.WriteLine("----- RecordNo : " + (i + 1) + " -----");
				CompileMain(f, infos, i);
				Program.main_form.StatusProgress.Value = i;
			}

			L2DatTool.WriteString(f, "SafePackage");
		}

		public virtual void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			L2DatDefinition item = new L2DatDefinition();
			item = infos[RecNo];
			List<String> TmpArr = Program.main_form.DatInfo.getFieldNames();
			for (int i = 0; i < TmpArr.Count; i++)
			{
				String FName = TmpArr[i];
				WriteFieldValue(f, item, FName);
			}
		}

		public void WriteFieldValue(BinaryWriter f, L2DatDefinition info, String FromName, String ToName)
		{
			int startPos = 0, endPos = 0;
			List<String> TmpArr = Program.main_form.DatInfo.getFieldNames();
			for (int i = 0; i < TmpArr.Count; i++)
			{
				if (TmpArr[i] == FromName)
					startPos = i;
				if (TmpArr[i] == ToName)
					endPos = i;
			}
			for (int i = startPos; i <= endPos; i++)
				WriteFieldValue(f, info, i);
		}

		public void WriteFieldValue(BinaryWriter f, L2DatDefinition item, int FNumber)
		{
			String FName = Program.main_form.DatInfo.getFieldNames()[FNumber];
			WriteFieldValue(f, item, FName);
		}

		public void WriteFieldValue(BinaryWriter f, L2DatDefinition item, String FName)
		{
			try
			{
				FieldInfo FType = Program.main_form.DatInfo.getDefinition().GetType().GetField(FName);
				if (FType.FieldType.FullName.EndsWith("UInt32"))
					f.Write((UInt32)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("Int32"))
					f.Write((Int32)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("Single"))
					f.Write((Single)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("Color"))
					f.Write(((Color)FType.GetValue(item)).ToArgb());
				else if (FType.FieldType.FullName.EndsWith("CNTINT_PAIR"))
					CNTINT_PAIR.Compile(f, (CNTINT_PAIR)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("CNTRINT_PAIR"))
					CNTRINT_PAIR.Compile(f, (CNTRINT_PAIR)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR"))
					CNTTXT_PAIR.Compile(f, (CNTTXT_PAIR)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR2"))
					CNTTXT_PAIR2.Compile(f, (CNTTXT_PAIR2)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("CNTASCF_PAIR"))
                    CNTASCF_PAIR.Compile(f, (CNTASCF_PAIR)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("MTX"))
					MTX.Compile(f, (MTX)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("MTX2"))
					MTX2.Compile(f, (MTX2)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("MTX3"))
					MTX3.Compile(f, (MTX3)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("ASCF"))
					ASCF.Compile(f, (ASCF)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("HEX"))
					HEX.Compile(f, (HEX)FType.GetValue(item));
				else if (FType.FieldType.FullName.EndsWith("UNICODE"))
					UNICODE.Compile(f, (UNICODE)FType.GetValue(item));
				else
				{
					Console.Out.WriteLine("!!!!! [WARNING] !!!!!");
					Console.Out.WriteLine("UnknownFieldType: " + FType.FieldType.FullName);
				}
			}
			catch (Exception ex)
			{
				item.DumpFieldValues();
				ex = new ApplicationException(
					String.Format("Error compiling string file (FieldName: {0} RecordOffset: 0x{1:X})",
								  FName, f.BaseStream.Position), ex);
			}
		}

		#endregion
	}

	#endregion

	#region ActionName

	public class ActionName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new ActionNameInfo();
		}
	}

	#endregion

	#region AdditionalEffect

	public class AdditionalEffect : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new AdditionalEffectInfo();
		}
	}

	#endregion

	#region Armorgrp

	public class Armorgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2)
				return new ArmorgrpInfo_CT3();
			else if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
				return new ArmorgrpInfo_CT1();
			else if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
				return new ArmorgrpInfo_C4();
			else
				return new ArmorgrpInfo();
		}
		
		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			L2DatDefinition ret = new L2DatDefinition();
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2)
			{
				ArmorgrpInfo_CT3 info = new ArmorgrpInfo_CT3();
				info.InitFieldValues();
				info = (ArmorgrpInfo_CT3)base.ReadFieldValue(f, info, "tag", "UNK_6");
				ret = info;
			}
			else if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
			{
				ArmorgrpInfo_CT1 info = new ArmorgrpInfo_CT1();
				info.InitFieldValues();
				info = (ArmorgrpInfo_CT1)base.ReadFieldValue(f, info, "tag", "UNK_1");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_5_Hellbound)
					info = (ArmorgrpInfo_CT1)base.ReadFieldValue(f, info, "UNK_2");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
					info = (ArmorgrpInfo_CT1)base.ReadFieldValue(f, info, "UNK_3");
				info = (ArmorgrpInfo_CT1)base.ReadFieldValue(f, info, "timetab", "UNK_6");
				ret = info;
			}
			else if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
			{
				ArmorgrpInfo_C4 info = new ArmorgrpInfo_C4();
				info = (ArmorgrpInfo_C4)base.ReadFieldValue(f, info, "tag", "mpbonus");
				ret = info;
			}
			else
			{
				ArmorgrpInfo info = new ArmorgrpInfo();
				info = (ArmorgrpInfo)base.ReadFieldValue(f, info, "tag", "mpbonus");
				ret = info;
			}
			return ret;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2)
			{
				ArmorgrpInfo_CT3 info = (ArmorgrpInfo_CT3)infos[RecNo];
				base.WriteFieldValue(f, info, "tag", "UNK_6");
			}
			else if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
			{
				ArmorgrpInfo_CT1 info = (ArmorgrpInfo_CT1)infos[RecNo];
				base.WriteFieldValue(f, info, "tag", "UNK_1");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_5_Hellbound)
					base.WriteFieldValue(f, info, "UNK_2");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
					base.WriteFieldValue(f, info, "UNK_3");
				base.WriteFieldValue(f, info, "timetab", "UNK_6");
			}
			else if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
			{
				ArmorgrpInfo_C4 info = (ArmorgrpInfo_C4)infos[RecNo];
				base.WriteFieldValue(f, info, "tag", "mpbonus");
			}
			else
			{
				ArmorgrpInfo info = (ArmorgrpInfo)infos[RecNo];
				base.WriteFieldValue(f, info, "tag", "mpbonus");
			}
		}
	}

	#endregion

	#region Britemgrp

	public class Britemgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new BritemgrpInfo();
		}
	}

	#endregion

	#region CastleName

	public class CastleName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new CastleNameInfo();
		}
 
		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			CastleNameInfo info = new CastleNameInfo();
			info = (CastleNameInfo)base.ReadFieldValue(f, info, "nbr", "desc");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				info = (CastleNameInfo)base.ReadFieldValue(f, info, "extra1", "extra4");
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			CastleNameInfo info = (CastleNameInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "nbr", "desc");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				base.WriteFieldValue(f, info, "extra1", "extra4");
		}
	}

	#endregion

	#region Charcreategrp

	public class Charcreategrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new CharcreategrpInfo();
		}
	}

	#endregion

	#region Chargrp

	public class Chargrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
				return new ChargrpInfo_CT1();
			else
				return new ChargrpInfo();
		}

		public override List<L2DatDefinition> Parse(BinaryReader f)
		{
			List<L2DatDefinition> res = new List<L2DatDefinition>();

			int total_records = 14;
			Program.main_form.StatusProgress.Maximum = total_records;
			Console.Out.WriteLine("TOTAL:" + total_records);
			for (int i = 0; i < total_records; i++)
			{
				//Console.Out.WriteLine("----- RecordNo : " + (i + 1) + " -----");
				res.Add(ParseMain(f, i));
				Program.main_form.StatusProgress.Value = i;
			}

			return res;
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			L2DatDefinition ret = new L2DatDefinition();
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
			{
				ChargrpInfo_CT1 info = new ChargrpInfo_CT1();
				//info = (ChargrpInfo_CT1)base.ReadFieldValue(f, info, "START", "END");
				ret = info;
			}
			else
			{
				ChargrpInfo info = new ChargrpInfo();
				info = (ChargrpInfo)base.ReadFieldValue(f, info, "face_icon", "cnt_ft");
				info.hair_mesh = new UNICODE();
				for (int i = 0; i < info.cnt_hm; i++)
				{
					info.hair_mesh.Text += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
					if (i < info.cnt_hm - 1)
						info.hair_mesh.Text += ",";
				}
				info.hair_tex = new UNICODE();
				for (int i = 0; i < info.cnt_ht; i++)
				{
					info.hair_tex.Text += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
					if (i < info.cnt_ht - 1)
						info.hair_tex.Text += ",";
				}
				info.face_mesh = new UNICODE();
				for (int i = 0; i < info.cnt_fm; i++)
				{
					info.face_mesh.Text += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
					if (i < info.cnt_fm - 1)
						info.face_mesh.Text += ",";
				}
				info.face_tex = new UNICODE();
				for (int i = 0; i < info.cnt_ft; i++)
				{
					info.face_tex.Text += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
					if (i < info.cnt_ft - 1)
						info.face_tex.Text += ",";
				}
				info = (ChargrpInfo)base.ReadFieldValue(f, info, "body_mesh1", "cnt_dmg");
				info.snd_att = new UNICODE();
				for (int i = 0; i < info.cnt_att; i++)
				{
					info.snd_att.Text += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
					if (i < info.cnt_att - 1)
						info.snd_att.Text += ",";
				}
				info.snd_def = new UNICODE();
				for (int i = 0; i < info.cnt_def; i++)
				{
					info.snd_def.Text += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
					if (i < info.cnt_def - 1)
						info.snd_def.Text += ",";
				}
				info.snd_dmg = new UNICODE();
				for (int i = 0; i < info.cnt_dmg; i++)
				{
					info.snd_dmg.Text += L2DatTool.ReadStringSimple_UnicodeInt32Length(f);
					if (i < info.cnt_dmg - 1)
						info.snd_dmg.Text += ",";
				}
				info = (ChargrpInfo)base.ReadFieldValue(f, info, "voice_snd_hand", "voice_snd_fist");
				ret = info;
			}
			return ret;
		}
	}

	#endregion

	#region ClassInfo

	public class ClassInfo : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new ClassInfoInfo();
		}
	}

	#endregion

	#region CommandName

	public class CommandName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new CommandNameInfo();
		}
	}

	#endregion

	#region Creditgrp

	public class Creditgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new CreditgrpInfo();
		}
	}

	#endregion
	
	#region Entereventgrp

	public class Entereventgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new EntereventgrpInfo();
		}
	}

	#endregion

	#region Etcitemgrp

	public class Etcitemgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
				return new EtcitemgrpInfo_CT1();
			else if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
				return new EtcitemgrpInfo_C4();
			else
				return new EtcitemgrpInfo();
		}
		
		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			L2DatDefinition ret = new L2DatDefinition();
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
			{
				EtcitemgrpInfo_CT1 info = new EtcitemgrpInfo_CT1();
				info.InitFieldValues();
				info = (EtcitemgrpInfo_CT1)base.ReadFieldValue(f, info, "tag", "drop_tex3");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
					info = (EtcitemgrpInfo_CT1)base.ReadFieldValue(f, info, "drop_extratex1", "newdata8");
				info = (EtcitemgrpInfo_CT1)base.ReadFieldValue(f, info, "icon1", "UNK_1");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_5_Hellbound)
					info = (EtcitemgrpInfo_CT1)base.ReadFieldValue(f, info, "UNK_2");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
					info = (EtcitemgrpInfo_CT1)base.ReadFieldValue(f, info, "UNK_3");
				info = (EtcitemgrpInfo_CT1)base.ReadFieldValue(f, info, "fort", "grade");
				ret = info;
			}
			else if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
			{
				EtcitemgrpInfo_C4 info = new EtcitemgrpInfo_C4();
				info = (EtcitemgrpInfo_C4)base.ReadFieldValue(f, info, "tag", "grade");
				ret = info;
			}
			else
			{
				EtcitemgrpInfo info = new EtcitemgrpInfo();
				info = (EtcitemgrpInfo)base.ReadFieldValue(f, info, "tag", "grade");
				ret = info;
			}
			return ret;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
			{
				EtcitemgrpInfo_CT1 info = (EtcitemgrpInfo_CT1)infos[RecNo];
				base.WriteFieldValue(f, info, "tag", "UNK_1");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_5_Hellbound)
					base.WriteFieldValue(f, info, "UNK_2");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
					base.WriteFieldValue(f, info, "UNK_3");
				base.WriteFieldValue(f, info, "fort", "grade");
			}
			else if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
			{
				EtcitemgrpInfo_C4 info = (EtcitemgrpInfo_C4)infos[RecNo];
				base.WriteFieldValue(f, info, "tag", "grade");
			}
			else
			{
				EtcitemgrpInfo info = (EtcitemgrpInfo)infos[RecNo];
				base.WriteFieldValue(f, info, "tag", "grade");
			}
		}
	}

	#endregion

	#region Eula

	public class Eula : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new EulaInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			EulaInfo info = new EulaInfo();
			info = (EulaInfo)base.ReadFieldValue(f, info, "eula", "fin1");
			if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
				info = (EulaInfo)base.ReadFieldValue(f, info, "fin2");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_5_Freya)
				info = (EulaInfo)base.ReadFieldValue(f, info, "fin3");
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			EulaInfo info = (EulaInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "eula", "fin1");
			if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
				base.WriteFieldValue(f, info, "fin2");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_5_Freya)
				base.WriteFieldValue(f, info, "fin3");
		}
	}

	#endregion

	#region GameTip

	public class GameTip : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new GameTipInfo();
		}
	}

	#endregion

	#region Hairaccessorylocgrp

	public class Hairaccessorylocgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new HairaccessorylocgrpInfo();
		}
	}

	#endregion

	#region Hennagrp

	public class Hennagrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new HennagrpInfo();
		}
	}

	#endregion

	#region HuntingZone

	public class HuntingZone : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new HuntingZoneInfo();
		}
	}

	#endregion

	#region InstantZoneData

	public class InstantZoneData : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new InstantZoneDataInfo();
		}
	}

	#endregion

	#region ItemName

	public class ItemName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2)
				return new ItemNameInfo_CT3();
			else
				return new ItemNameInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			L2DatDefinition ret = new L2DatDefinition();
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
			{
				ItemNameInfo_CT3 info = new ItemNameInfo_CT3();
				info = (ItemNameInfo_CT3)base.ReadFieldValue(f, info, "id", "UNK2");
				ret = info;
			}
			else
			{
				ItemNameInfo info = new ItemNameInfo();
				info = (ItemNameInfo)base.ReadFieldValue(f, info, "id", "popup");
				if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
					info = (ItemNameInfo)base.ReadFieldValue(f, info, "set_ids", "special_enchant_desc");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
					info = (ItemNameInfo)base.ReadFieldValue(f, info, "UNK2");
				ret = info;
			}
			return ret;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
			{
				ItemNameInfo_CT3 info = (ItemNameInfo_CT3)infos[RecNo];
				base.WriteFieldValue(f, info, "id", "UNK2");
			}
			else
			{
				ItemNameInfo info = (ItemNameInfo)infos[RecNo];
				base.WriteFieldValue(f, info, "id", "popup");
				if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
					base.WriteFieldValue(f, info, "set_ids", "special_enchant_desc");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
					base.WriteFieldValue(f, info, "UNK2");
			}
		}
	}

	#endregion

	#region ItemPrime

	public class ItemPrime : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new ItemPrimeInfo();
		}
	}

	#endregion

	#region Logongrp

	public class Logongrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new LogongrpInfo();
		}
	}

	#endregion

	#region Mobskillanimgrp

	public class Mobskillanimgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new MobskillanimgrpInfo();
		}
	}

	#endregion

	#region MusicInfo

	public class MusicInfo : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new MusicInfoInfo();
		}
	}

	#endregion

	#region Npcgrp

	public class Npcgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_4_Epilogue)
				return new NpcgrpInfo_CT2_4();
			else
				return new NpcgrpInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			L2DatDefinition ret = new L2DatDefinition();
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_4_Epilogue)
			{
				NpcgrpInfo_CT2_4 info = new NpcgrpInfo_CT2_4();
				info.InitFieldValues();
				info = (NpcgrpInfo_CT2_4)base.ReadFieldValue(f, info, "tag", "UNK_1");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_5_Freya)
					info = (NpcgrpInfo_CT2_4)base.ReadFieldValue(f, info, "UNK_2");
				info = (NpcgrpInfo_CT2_4)base.ReadFieldValue(f, info, "effect", "npcend");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_6_HighFive)
					info = (NpcgrpInfo_CT2_4)base.ReadFieldValue(f, info, "UNK_4");
				ret = info;
			}
			else
			{
				NpcgrpInfo info = new NpcgrpInfo();
				info = (NpcgrpInfo)base.ReadFieldValue(f, info, "tag", "npc_speed");
				if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
					info = (NpcgrpInfo)base.ReadFieldValue(f, info, "UNK_0_NEW");
				else
					info = (NpcgrpInfo)base.ReadFieldValue(f, info, "UNK_0_OLD");
				info = (NpcgrpInfo)base.ReadFieldValue(f, info, "snd1", "snd3");
				if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
				{
					info = (NpcgrpInfo)base.ReadFieldValue(f, info, "rb_effect_on");
					if (info.rb_effect_on == 1)
						info = (NpcgrpInfo)base.ReadFieldValue(f, info, "rb_effect", "rb_effect_fl");
				}
				if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
					info = (NpcgrpInfo)base.ReadFieldValue(f, info, "UNK_1_NEW");
				else
					info = (NpcgrpInfo)base.ReadFieldValue(f, info, "UNK_1_OLD", "level_lim_up");
				info = (NpcgrpInfo)base.ReadFieldValue(f, info, "effect", "class_lim");
				ret = info;
			}
			return ret;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_4_Epilogue)
			{
				NpcgrpInfo_CT2_4 info = (NpcgrpInfo_CT2_4)infos[RecNo];
				base.WriteFieldValue(f, info, "tag", "UNK_1");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_5_Freya)
					base.WriteFieldValue(f, info, "UNK_2");
				base.WriteFieldValue(f, info, "effect", "npcend");
                if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_6_HighFive)
                    base.WriteFieldValue(f, info, "UNK_4");
            }
			else
			{
				NpcgrpInfo info = (NpcgrpInfo)infos[RecNo];
				base.WriteFieldValue(f, info, "tag", "npc_speed");
				if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
					base.WriteFieldValue(f, info, "UNK_0_NEW");
				else
					base.WriteFieldValue(f, info, "UNK_0_OLD");
				base.WriteFieldValue(f, info, "snd1", "snd3");
				if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
				{
					base.WriteFieldValue(f, info, "rb_effect_on");
					if (info.rb_effect_on == 1)
						base.WriteFieldValue(f, info, "rb_effect", "rb_effect_fl");
				}
				if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
					base.WriteFieldValue(f, info, "UNK_1_NEW");
				else
					base.WriteFieldValue(f, info, "UNK_1_OLD", "level_lim_up");
				base.WriteFieldValue(f, info, "effect", "class_lim");
			}
		}
	}

	#endregion

	#region NpcName

	public class NpcName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new NpcNameInfo();
		}
	}

	#endregion

	#region NpcString

	public class NpcString : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new NpcStringInfo();
		}
	}

	#endregion

	#region Obscene

	public class Obscene : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new ObsceneInfo();
		}
	}

	#endregion

	#region Optiondata_Client

	public class Optiondata_Client : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new Optiondata_ClientInfo();
		}
	}

	#endregion

	#region ProductName

	public class ProductName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new ProductNameInfo();
		}
	}

	#endregion

	#region QuestName

	public class QuestName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new QuestNameInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			QuestNameInfo info = new QuestNameInfo();
			info = (QuestNameInfo)base.ReadFieldValue(f, info, "id", "get_item_in_quest");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
				info = (QuestNameInfo)base.ReadFieldValue(f, info, "UNK_1", "short_description");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
				info = (QuestNameInfo)base.ReadFieldValue(f, info, "req_class", "area_id");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				info = (QuestNameInfo)base.ReadFieldValue(f, info, "UNK_4", "tab6");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_5_Freya)
				info = (QuestNameInfo)base.ReadFieldValue(f, info, "tab7");
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			QuestNameInfo info = (QuestNameInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "id", "get_item_in_quest");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
				base.WriteFieldValue(f, info, "UNK_1", "short_description");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
				base.WriteFieldValue(f, info, "req_class", "area_id");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				base.WriteFieldValue(f, info, "UNK_4", "tab6");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_5_Freya)
				base.WriteFieldValue(f, info, "tab7");
		}
	}

	#endregion

	#region RaidData

	public class RaidData : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new RaidDataInfo();
		}
	}

	#endregion

	#region Recipe

	public class Recipe : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new RecipeInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			RecipeInfo info = new RecipeInfo();
			info = (RecipeInfo)base.ReadFieldValue(f, info, "name", "success_rate");
			int MatCnt = f.ReadInt32();
			info.materials = "";
			for (int i = 0; i < MatCnt; i++)
			{
				info.materials += "[" + f.ReadInt32().ToString();
				info.materials += "(" + f.ReadInt32().ToString() + ")]";
				if (i < MatCnt - 1)
					info.materials += ",";
			}
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
				info = (RecipeInfo)base.ReadFieldValue(f, info, "UNK0");
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			RecipeInfo info = (RecipeInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "name", "success_rate");
			string[] TmpStr = info.materials.Split(new char[] { ',' });
			f.Write(TmpStr.Length);
			for (int i = 0; i < TmpStr.Length; i++)
			{
				TmpStr[i] = TmpStr[i].Trim('[', ')', ']');
				String[] TmpStr2 = TmpStr[i].Split(new char[] { '(' });
				f.Write(Convert.ToInt32(TmpStr2[0]));
				f.Write(Convert.ToInt32(TmpStr2[1]));
			}
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
				base.WriteFieldValue(f, info, "UNK0");
		}
	}

	#endregion

	#region RideData

	public class RideData : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new RideDataInfo();
		}
	}

	#endregion

	#region ScenePlayerData

	public class ScenePlayerData : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new ScenePlayerDataInfo();
		}
	}

	#endregion

	#region ServerName

	public class ServerName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new ServerNameInfo();
		}
	}

	#endregion

	#region ShortcutAlias

	public class ShortcutAlias : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new ShortcutAliasInfo();
		}
	}

	#endregion

	#region Skillgrp

	public class Skillgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
            if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
				return new SkillgrpInfo_CT1();
			else
				return new SkillgrpInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			L2DatDefinition ret = new L2DatDefinition();
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
			{
				SkillgrpInfo_CT1 info = new SkillgrpInfo_CT1();
				info = (SkillgrpInfo_CT1)base.ReadFieldValue(f, info, "skill_id", "cast_style");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_5_Hellbound)
					info = (SkillgrpInfo_CT1)base.ReadFieldValue(f, info, "UNK_0");
				info = (SkillgrpInfo_CT1)base.ReadFieldValue(f, info, "hit_time", "hp_consume");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_4_Epilogue)
					info = (SkillgrpInfo_CT1)base.ReadFieldValue(f, info, "nonetext1");
				info = (SkillgrpInfo_CT1)base.ReadFieldValue(f, info, "UNK_1", "UNK_3");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_5_Freya)
					info = (SkillgrpInfo_CT1)base.ReadFieldValue(f, info, "UNK_4", "nonetext2");
				ret = info;
			}
			else
			{
				SkillgrpInfo info = new SkillgrpInfo();
				info = (SkillgrpInfo)base.ReadFieldValue(f, info, "skill_id", "extra_eff");
				if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
					info = (SkillgrpInfo)base.ReadFieldValue(f, info, "is_ench", "UNK_1");
				ret = info;
			}
			return ret;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
			{
				SkillgrpInfo_CT1 info = (SkillgrpInfo_CT1)infos[RecNo];
				base.WriteFieldValue(f, info, "skill_id", "cast_style");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_5_Hellbound)
					base.WriteFieldValue(f, info, "UNK_0");
				base.WriteFieldValue(f, info, "hit_time", "hp_consume");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_4_Epilogue)
					base.WriteFieldValue(f, info, "nonetext1");
				base.WriteFieldValue(f, info, "UNK_1", "UNK_3");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_5_Freya)
					base.WriteFieldValue(f, info, "UNK_4", "nonetext2");
			}
			else
			{
				SkillgrpInfo info = (SkillgrpInfo)infos[RecNo];
				base.WriteFieldValue(f, info, "skill_id", "extra_eff");
				if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
					base.WriteFieldValue(f, info, "is_ench", "UNK_1");
			}
		}
	}

	#endregion

	#region SkillName

	public class SkillName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new SkillNameInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			SkillNameInfo info = new SkillNameInfo();
			info = (SkillNameInfo)base.ReadFieldValue(f, info, "id", "description");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
				info = (SkillNameInfo)base.ReadFieldValue(f, info, "desc_add1", "desc_add2");
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			SkillNameInfo info = (SkillNameInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "id", "description");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle5)
				base.WriteFieldValue(f, info, "desc_add1", "desc_add2");
		}
	}

	#endregion

	#region Skillsoundgrp

	public class Skillsoundgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new SkillsoundgrpInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			SkillsoundgrpInfo info = new SkillsoundgrpInfo();
			info = (SkillsoundgrpInfo)base.ReadFieldValue(f, info, "skill_id", "fshaman_sub");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
				info = (SkillsoundgrpInfo)base.ReadFieldValue(f, info, "mkamael_sub", "fkamael_sub");
			info = (SkillsoundgrpInfo)base.ReadFieldValue(f, info, "RESERVED_sub", "fshaman_throw");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
				info = (SkillsoundgrpInfo)base.ReadFieldValue(f, info, "mkamael_throw", "fkamael_throw");
			info = (SkillsoundgrpInfo)base.ReadFieldValue(f, info, "RESERVED_throw", "sound_rad");
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			SkillsoundgrpInfo info = (SkillsoundgrpInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "skill_id", "fshaman_sub");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
				base.WriteFieldValue(f, info, "mkamael_sub", "fkamael_sub");
			base.WriteFieldValue(f, info, "RESERVED_sub", "fshaman_throw");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
				base.WriteFieldValue(f, info, "mkamael_throw", "fkamael_throw");
			base.WriteFieldValue(f, info, "RESERVED_throw", "sound_rad");
		}
	}

	#endregion

	#region SkillSoundSource

	public class SkillSoundSource : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new SkillSoundSourceInfo();
		}
	}

	#endregion

	#region StaticObject

	public class StaticObject : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new StaticObjectInfo();
		}
	}

	#endregion

	#region SymbolName

	public class SymbolName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new SymbolNameInfo();
		}
	}

	#endregion

	#region SysString

	public class SysString : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new SysStringInfo();
		}
	}

	#endregion

	#region SystemMsg

	public class SystemMsg : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new SystemMsgInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			SystemMsgInfo info = new SystemMsgInfo();
			info = (SystemMsgInfo)base.ReadFieldValue(f, info, "id", "sys_msg_ref");
			if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
				info = (SystemMsgInfo)base.ReadFieldValue(f, info, "UNK_1_1", "type");
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			SystemMsgInfo info = (SystemMsgInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "id", "sys_msg_ref");
			if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
				base.WriteFieldValue(f, info, "UNK_1_1", "type");
		}
	}

	#endregion

	#region TransformData

	public class TransformData : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new TransformDataInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			TransformDataInfo info = new TransformDataInfo();
			info = (TransformDataInfo)base.ReadFieldValue(f, info, "id", "transform_effect_b");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2)
				info = (TransformDataInfo)base.ReadFieldValue(f, info, "unk1");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				info = (TransformDataInfo)base.ReadFieldValue(f, info, "unk2", "unk4");
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			TransformDataInfo info = (TransformDataInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "id", "transform_effect_b");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2)
				base.WriteFieldValue(f, info, "unk1");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				base.WriteFieldValue(f, info, "unk2", "unk4");
		}
	}

	#endregion

	#region Variationeffectgrp

	public class Variationeffectgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new VariationeffectgrpInfo();
		}
	}

	#endregion

	#region Vehiclepartsgrp

	public class Vehiclepartsgrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new VehiclepartsgrpInfo();
		}
	}

	#endregion

	#region Weapongrp

	public class Weapongrp : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new WeapongrpInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			WeapongrpInfo info = new WeapongrpInfo();
			info.InitFieldValues();
			info = (WeapongrpInfo)base.ReadFieldValue(f, info, "tag", "UNK_0");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "drop_mesh1", "drop_tex3");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				{
					info = (WeapongrpInfo)base.ReadFieldValue(f, info, "drop_extratex1", "newdata8");
				}
			}
			else
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "drop_mesh1");
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "drop_tex1");
			}
			info = (WeapongrpInfo)base.ReadFieldValue(f, info, "icon1", "UNK_1");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_5_Hellbound)
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "UNK_2");
			}
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "UNK_3");
			}
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "timetab");
			}
			info = (WeapongrpInfo)base.ReadFieldValue(f, info, "body_part", "handness");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "wpn_mesh_new");
			}
			else
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "wpn_mesh");
			}
			info = (WeapongrpInfo)base.ReadFieldValue(f, info, "wpn_tex", "curvature");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "UNK_4", "UNK_5");
			}
			info = (WeapongrpInfo)base.ReadFieldValue(f, info, "effA");
			if ((Program.main_form.selectedDatVersion < DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh.length == 2) ||
					(Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh_new.length == 2))
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "effB");
			}
			info = (WeapongrpInfo)base.ReadFieldValue(f, info, "junk1A_1", "junk1A_5");
			if ((Program.main_form.selectedDatVersion < DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh.length == 2) ||
					(Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh_new.length == 2))
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "junk1B_1", "junk1B_5");
			}
			info = (WeapongrpInfo)base.ReadFieldValue(f, info, "rangeA");
			if ((Program.main_form.selectedDatVersion < DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh.length == 2) ||
					(Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh_new.length == 2))
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "rangeB");
			}
			info = (WeapongrpInfo)base.ReadFieldValue(f, info, "junk2A_1", "junk2A_6");
			if ((Program.main_form.selectedDatVersion < DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh.length == 2) ||
					(Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh_new.length == 2))
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "junk2B_1", "junk2B_6");
			}
			if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
			{
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "junk3_1", "junk3_4");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_4_Epilogue)
					info = (WeapongrpInfo)base.ReadFieldValue(f, info, "junk3_5", "junk3_6");
				info = (WeapongrpInfo)base.ReadFieldValue(f, info, "icons1", "icons4");
			}
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			WeapongrpInfo info = (WeapongrpInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "tag", "UNK_0");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
			{
				base.WriteFieldValue(f, info, "drop_mesh1", "drop_tex3");
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				{
					base.WriteFieldValue(f, info, "drop_extratex1", "newdata8");
				}
			}
			else
			{
				base.WriteFieldValue(f, info, "drop_mesh1");
				base.WriteFieldValue(f, info, "drop_tex1");
			}
			base.WriteFieldValue(f, info, "icon1", "UNK_1");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_5_Hellbound)
			{
				base.WriteFieldValue(f, info, "UNK_2");
			}
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_1_Gracia)
			{
				base.WriteFieldValue(f, info, "UNK_3");
			}
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone1_0_Kamael)
			{
				base.WriteFieldValue(f, info, "timetab");
			}
			base.WriteFieldValue(f, info, "body_part", "handness");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
			{
				base.WriteFieldValue(f, info, "wpn_mesh_new");
			}
			else
			{
				base.WriteFieldValue(f, info, "wpn_mesh");
			}
			base.WriteFieldValue(f, info, "wpn_tex", "curvature");
			if (Program.main_form.selectedDatVersion >= DatVersion.Chronicle4)
			{
				base.WriteFieldValue(f, info, "UNK_4", "UNK_5");
			}
			base.WriteFieldValue(f, info, "effA");
			if ((Program.main_form.selectedDatVersion < DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh.length == 2) ||
					(Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh_new.length == 2))
			{
				base.WriteFieldValue(f, info, "effB");
			}
			base.WriteFieldValue(f, info, "junk1A_1", "junk1A_5");
			if ((Program.main_form.selectedDatVersion < DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh.length == 2) ||
					(Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh_new.length == 2))
			{
				base.WriteFieldValue(f, info, "junk1B_1", "junk1B_5");
			}
			base.WriteFieldValue(f, info, "rangeA");
			if ((Program.main_form.selectedDatVersion < DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh.length == 2) ||
					(Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh_new.length == 2))
			{
				base.WriteFieldValue(f, info, "rangeB");
			}
			base.WriteFieldValue(f, info, "junk2A_1", "junk2A_6");
			if ((Program.main_form.selectedDatVersion < DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh.length == 2) ||
					(Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_2_Part2 && info.wpn_mesh_new.length == 2))
			{
				base.WriteFieldValue(f, info, "junk2B_1", "junk2B_6");
			}
			if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
			{
				if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				{
					base.WriteFieldValue(f, info, "junk3_1", "icons4");
				}
				else
				{
					base.WriteFieldValue(f, info, "junk3_1", "junk3_4");
					base.WriteFieldValue(f, info, "icons1", "icons4");
				}
			}
		}
	}

	#endregion

	#region ZoneName

	public class ZoneName : L2DatParser
	{
		public override L2DatDefinition getDefinition()
		{
			return new ZoneNameInfo();
		}

		public override L2DatDefinition ParseMain(BinaryReader f, int RecNo)
		{
			ZoneNameInfo info = new ZoneNameInfo();
			info = (ZoneNameInfo)base.ReadFieldValue(f, info, "nbr", "zone_name");
			if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
				info = (ZoneNameInfo)base.ReadFieldValue(f, info, "coords1", "map");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				info = (ZoneNameInfo)base.ReadFieldValue(f, info, "dupa");
			return info;
		}

		public override void CompileMain(BinaryWriter f, List<L2DatDefinition> infos, int RecNo)
		{
			ZoneNameInfo info = (ZoneNameInfo)infos[RecNo];
			base.WriteFieldValue(f, info, "nbr", "zone_name");
			if (Program.main_form.selectedDatVersion >= DatVersion.Interlude)
				base.WriteFieldValue(f, info, "coords1", "map");
			if (Program.main_form.selectedDatVersion >= DatVersion.ChaoticThrone2_3_Final)
				base.WriteFieldValue(f, info, "dupa");
		}
	}

	#endregion
}
