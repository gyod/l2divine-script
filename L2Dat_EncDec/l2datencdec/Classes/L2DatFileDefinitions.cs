#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using L2DatEncDec.Tools;
using INT = System.Int32;
using UINT = System.UInt32;
using FLOAT = System.Single;
using CHEX = L2DatEncDec.Tools.ASCF;

#endregion

namespace L2DatEncDec.Definitions
{
	#region L2DatDefinition

	public class L2DatDefinition
	{
		static MemberInfo[] members;

		public L2DatDefinition()
		{
			members = this.GetType().GetMembers(
						   BindingFlags.Public | BindingFlags.Instance);
		}

		public MemberInfo[] getMembers()
		{
			return members;
		}

		public void InitFieldValues()
		{
			foreach (MemberInfo m in members)
			{
				if (m.MemberType == MemberTypes.Field)
				{
					FieldInfo FType = this.GetType().GetField(m.Name);
					if (FType == null) continue;
					if (FType.FieldType.FullName.StartsWith("L2DatEncDec.Tools."))
					{
						Type t = Type.GetType(FType.FieldType.FullName);
						object instance = t.InvokeMember(null,
													BindingFlags.CreateInstance,
													null,
													null,
													new object[] { });
						if (instance != null)
							FType.SetValue(this, instance);
					}
				}
			}
		}

		public void DumpFieldValues()
		{
			foreach (MemberInfo m in members)
			{
				if (m.MemberType == MemberTypes.Field)
				{
					FieldInfo FType = this.GetType().GetField(m.Name);
					if (FType == null) continue;
					String TmpStr = "";
					if (FType.GetValue(this) != null)
					{
						if (FType.FieldType.FullName.EndsWith("Color"))
							TmpStr = LmUtils.ConvertUtilities.ColorToHtmlColor((Color)FType.GetValue(this));
						else if (FType.FieldType.FullName.EndsWith("CNTINT_PAIR"))
							TmpStr = ((CNTINT_PAIR)FType.GetValue(this)).getText();
						else if (FType.FieldType.FullName.EndsWith("CNTRINT_PAIR"))
							TmpStr = ((CNTRINT_PAIR)FType.GetValue(this)).getText();
						else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR"))
							TmpStr = ((CNTTXT_PAIR)FType.GetValue(this)).getText();
						else if (FType.FieldType.FullName.EndsWith("CNTTXT_PAIR2"))
							TmpStr = ((CNTTXT_PAIR2)FType.GetValue(this)).getText();
						else if (FType.FieldType.FullName.EndsWith("CNTASCF_PAIR"))
							TmpStr = ((CNTASCF_PAIR)FType.GetValue(this)).getText();
						else if (FType.FieldType.FullName.EndsWith("MTX"))
							TmpStr = ((MTX)FType.GetValue(this)).getText();
						else if (FType.FieldType.FullName.EndsWith("MTX2"))
							TmpStr = ((MTX2)FType.GetValue(this)).getText();
						else if (FType.FieldType.FullName.EndsWith("MTX3"))
							TmpStr = ((MTX3)FType.GetValue(this)).getText();
						else if (FType.FieldType.FullName.EndsWith("UNICODE"))
							TmpStr = ((UNICODE)FType.GetValue(this)).Text;
						else if (FType.FieldType.FullName.EndsWith("ASCF"))
							TmpStr = ((ASCF)FType.GetValue(this)).Text;
						else if (FType.FieldType.FullName.EndsWith("HEX"))
							TmpStr = ((HEX)FType.GetValue(this)).Text;
						else
							TmpStr = FType.GetValue(this).ToString();
					}
					Console.Out.WriteLine(m.Name + ": " + TmpStr);
				}
			}
		}
	}

	#endregion

	#region ActionName

	public class ActionNameInfo : L2DatDefinition
	{
	   /*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT id;
		public INT type;
		public UINT category;
		public CNTRINT_PAIR cat2;
		public ASCF cmd;
		public ASCF icon;
		public ASCF name;
		public UNICODE desc;
	}

	#endregion

	#region AdditionalEffect

	public class AdditionalEffectInfo : L2DatDefinition
	{
		/*
		 Info from l2asm-disasm
		 */
		public UINT val;
		public ASCF name;
	}

	#endregion

	#region Armorgrp

	public class ArmorgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT id;
		public UINT drop_type;
		public UINT drop_anim_type;
		public UINT drop_radius;
		public UINT drop_height;
		public UINT UNK_0;
		public UNICODE drop_mesh;
		public UNICODE drop_tex;
		public UNICODE icon1;
		public UNICODE icon2;
		public UNICODE icon3;
		public UNICODE icon4;
		public UNICODE icon5;
		public UINT durability;
		public UINT weight;
		public UINT material;
		public UINT crystallizable;
		public UINT UNK_1;
		public UINT body_part;
		public MTX m_HumnFigh;
		public MTX f_HumnFigh;
		public MTX m_DarkElf;
		public MTX f_DarkElf;
		public MTX m_Dorf;
		public MTX f_Dorf;
		public MTX m_Elf;
		public MTX f_Elf;
		public MTX m_HumnMyst;
		public MTX f_HumnMyst;
		public MTX m_OrcFigh;
		public MTX f_OrcFigh;
		public MTX m_OrcMage;
		public MTX f_OrcMage;
		public MTX Unknown_MT;
		public MTX NPC_MT;
		public UNICODE att_eff;
		public CNTTXT_PAIR item_sound;
		public UNICODE drop_sound;
		public UNICODE equip_sound;
		public UINT armor_type;
		public UINT crystal_type;
		public UINT avoid_mod;
		public UINT pdef;
		public UINT mdef;
		public UINT mpbonus;
	}

	public class ArmorgrpInfo_C4 : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT id;
		public UINT drop_type;
		public UINT drop_anim_type;
		public UINT drop_radius;
		public UINT drop_height;
		public UINT UNK_0;
		public UNICODE drop_mesh1;
		public UNICODE drop_mesh2;
		public UNICODE drop_mesh3;
		public UNICODE drop_tex1;
		public UNICODE drop_tex2;
		public UNICODE drop_tex3;
		public UNICODE icon1;
		public UNICODE icon2;
		public UNICODE icon3;
		public UNICODE icon4;
		public UNICODE icon5;
		public UINT durability;
		public UINT weight;
		public UINT material;
		public UINT crystallizable;
		public UINT UNK_1;
		public UINT body_part;
		public MTX m_HumnFigh;
		public MTX m_HumnFigh_add;
		public MTX f_HumnFigh;
		public MTX f_HumnFigh_add;
		public MTX m_DarkElf;
		public MTX m_DarkElf_add;
		public MTX f_DarkElf;
		public MTX f_DarkElf_add;
		public MTX m_Dorf;
		public MTX m_Dorf_add;
		public MTX f_Dorf;
		public MTX f_Dorf_add;
		public MTX m_Elf;
		public MTX m_Elf_add;
		public MTX f_Elf;
		public MTX f_Elf_add;
		public MTX m_HumnMyst;
		public MTX m_HumnMyst_add;
		public MTX f_HumnMyst;
		public MTX f_HumnMyst_add;
		public MTX m_OrcFigh;
		public MTX m_OrcFigh_add;
		public MTX f_OrcFigh;
		public MTX f_OrcFigh_add;
		public MTX m_OrcMage;
		public MTX m_OrcMage_add;
		public MTX f_OrcMage;
		public MTX f_OrcMage_add;
		public MTX Unknown_MT;
		public MTX NPC_MT;
		public MTX ACC_MT;
		public UNICODE att_eff;
		public CNTTXT_PAIR item_sound;
		public UNICODE drop_sound;
		public UNICODE equip_sound;
		public UINT UNK_2;
		public UINT UNK_3;
		public UINT armor_type;
		public UINT crystal_type;
		public UINT avoid_mod;
		public UINT pdef;
		public UINT mdef;
		public UINT mpbonus;
	}

	public class ArmorgrpInfo_CT1 : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT id;
		public UINT drop_type;
		public UINT drop_anim_type;
		public UINT drop_radius;
		public UINT drop_height;
		public UINT UNK_0;
		public UNICODE drop_mesh1;
		public UNICODE drop_mesh2;
		public UNICODE drop_mesh3;
		public UNICODE drop_tex1;
		public UNICODE drop_tex2;
		public UNICODE drop_tex3;
		public UNICODE icon1;
		public UNICODE icon2;
		public UNICODE icon3;
		public UNICODE icon4;
		public UNICODE icon5;
		public UINT durability;
		public UINT weight;
		public UINT material;
		public UINT crystallizable;
		public UINT UNK_1;
		public CNTINT_PAIR UNK_2;
		public UINT UNK_3;
		public UNICODE timetab;
		public UINT body_part;
		public MTX m_HumnFigh;
		public MTX2 m_HumnFigh_add;
		public MTX f_HumnFigh;
		public MTX2 f_HumnFigh_add;
		public MTX m_DarkElf;
		public MTX2 m_DarkElf_add;
		public MTX f_DarkElf;
		public MTX2 f_DarkElf_add;
		public MTX m_Dorf;
		public MTX2 m_Dorf_add;
		public MTX f_Dorf;
		public MTX2 f_Dorf_add;
		public MTX m_Elf;
		public MTX2 m_Elf_add;
		public MTX f_Elf;
		public MTX2 f_Elf_add;
		public MTX m_HumnMyst;
		public MTX2 m_HumnMyst_add;
		public MTX f_HumnMyst;
		public MTX2 f_HumnMyst_add;
		public MTX m_OrcFigh;
		public MTX2 m_OrcFigh_add;
		public MTX f_OrcFigh;
		public MTX2 f_OrcFigh_add;
		public MTX m_OrcMage;
		public MTX2 m_OrcMage_add;
		public MTX f_OrcMage;
		public MTX2 f_OrcMage_add;
		public MTX m_Kamael;
		public MTX2 m_Kamael_add;
		public MTX f_Kamael;
		public MTX2 f_Kamael_add;
		public MTX NPC;
		public MTX2 NPC_add;
		public UNICODE att_eff;
		public CNTTXT_PAIR item_sound;
		public UNICODE drop_sound;
		public UNICODE equip_sound;
		public UINT UNK_4;
		public UINT UNK_5;
		public UINT armor_type;
		public UINT crystal_type;
		public UINT avoid_mod;
		public UINT pdef;
		public UINT mdef;
		public UINT mpbonus;
		public UINT UNK_6;
	}

	public class ArmorgrpInfo_CT3 : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT id;
		public UINT drop_type;
		public UINT drop_anim_type;
		public UINT drop_radius;
		public UINT drop_height;
		public UINT UNK_0;
		public UNICODE drop_mesh1;
		public UNICODE drop_mesh2;
		public UNICODE drop_mesh3;
		public UNICODE drop_tex1;
		public UNICODE drop_tex2;
		public UNICODE drop_tex3;
		public UNICODE drop_extratex1;
		public UINT newdata1;
		public UINT newdata2;
		public UINT newdata3;
		public UINT newdata4;
		public UINT newdata5;
		public UINT newdata6;
		public UINT newdata7;
		public UINT newdata8;
		public UNICODE icon1;
		public UNICODE icon2;
		public UNICODE icon3;
		public UNICODE icon4;
		public UNICODE icon5;
		public UINT durability;
		public UINT weight;
		public UINT material;
		public UINT crystallizable;
		public UINT UNK_1;
		public CNTINT_PAIR UNK_2;
		public UINT UNK_3;
		public UNICODE timetab;
		public UINT body_part;
		public MTX m_HumnFigh;
		public MTX3 m_HumnFigh_add;
		public MTX f_HumnFigh;
		public MTX3 f_HumnFigh_add;
		public MTX m_DarkElf;
		public MTX3 m_DarkElf_add;
		public MTX f_DarkElf;
		public MTX3 f_DarkElf_add;
		public MTX m_Dorf;
		public MTX3 m_Dorf_add;
		public MTX f_Dorf;
		public MTX3 f_Dorf_add;
		public MTX m_Elf;
		public MTX3 m_Elf_add;
		public MTX f_Elf;
		public MTX3 f_Elf_add;
		public MTX m_HumnMyst;
		public MTX3 m_HumnMyst_add;
		public MTX f_HumnMyst;
		public MTX3 f_HumnMyst_add;
		public MTX m_OrcFigh;
		public MTX3 m_OrcFigh_add;
		public MTX f_OrcFigh;
		public MTX3 f_OrcFigh_add;
		public MTX m_OrcMage;
		public MTX3 m_OrcMage_add;
		public MTX f_OrcMage;
		public MTX3 f_OrcMage_add;
		public MTX m_Kamael;
		public MTX3 m_Kamael_add;
		public MTX f_Kamael;
		public MTX3 f_Kamael_add;
		public MTX NPC;
		public MTX3 NPC_add;
		public UNICODE att_eff;
		public CNTTXT_PAIR item_sound;
		public UNICODE drop_sound;
		public UNICODE equip_sound;
		public UINT UNK_4;
		public UINT UNK_5;
		public UINT armor_type;
		public UINT crystal_type;
		public UINT avoid_mod;
		public UINT pdef;
		public UINT mdef;
		public UINT mpbonus;
		public UINT UNK_6;
	}

	#endregion

	#region Britemgrp

	public class BritemgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT ID;
		public UINT int1;
		public UINT int2;
		public UNICODE name;
		public UINT IDconn;
		public UINT vals1;
		public UINT vals2;
		public UINT vals3;
		public UINT vals4;
		public UINT vals5;
		public UINT vals6;
		public UINT vals7;
		public UINT vals8;
		public UINT vals9;
		public INT tail;
	}

	#endregion

	#region CastleName

	public class CastleNameInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT nbr;
		public UINT tag;
		public UINT id;
		public ASCF castle_name;
		public ASCF location;
		public ASCF desc;
		public ASCF extra1;
		public ASCF extra2;
		public ASCF extra3;
		public ASCF extra4;
	}

	#endregion

	#region Charcreategrp

	public class CharcreategrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public FLOAT flts1;
		public FLOAT flts2;
		public FLOAT flts3;
		public FLOAT flts4;
		public INT ints1;
		public INT ints2;
		public INT ints3;
		public INT ints4;
		public INT ints5;
		public INT ints6;
	}

	#endregion

	#region Chargrp

	public class ChargrpInfo_CT1 : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
	}

	public class ChargrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UNICODE face_icon;
		public UINT cnt_hm;
		public UINT cnt_ht;
		public UINT cnt_fm;
		public UINT cnt_ft;
		public UNICODE hair_mesh; //[cnt_hm]
		public UNICODE hair_tex;  //[cnt_ht]
		public UNICODE face_mesh; //[cnt_fm]
		public UNICODE face_tex;  //[cnt_ft]
		public UNICODE body_mesh1;
		public UNICODE body_mesh2;
		public UNICODE body_mesh3;
		public UNICODE body_mesh4;
		public UNICODE body_tex1;
		public UNICODE body_tex2;
		public UNICODE body_tex3;
		public UNICODE body_tex4;
		public UNICODE attack_eff;
		public UINT walkanimframe;
		public UINT cnt_att;
		public UINT cnt_def;
		public UINT cnt_dmg;
		public UNICODE snd_att; //[cnt_att]
		public UNICODE snd_def; //[cnt_def]
		public UNICODE snd_dmg; //[cnt_dmg]
		public CNTTXT_PAIR voice_snd_hand;
		public CNTTXT_PAIR voice_snd_1hs;
		public CNTTXT_PAIR voice_snd_2hs;
		public CNTTXT_PAIR voice_snd_dual;
		public CNTTXT_PAIR voice_snd_pole;
		public CNTTXT_PAIR voice_snd_bow;
		public CNTTXT_PAIR voice_snd_unknown;
		public CNTTXT_PAIR voice_snd_fist;
	}

	#endregion

	#region ClassInfo

	public class ClassInfoInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF name;
	}

	#endregion

	#region CommandName

	public class CommandNameInfo : L2DatDefinition
	{
	   /*
		Info from l2asm-disasm
		*/
		public UINT nbr;
		public UINT id;
		public ASCF name;
	}

	#endregion

	#region Creditgrp

	public class CreditgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF html;
		public ASCF image;
		public UINT time;
		public UINT align;
	}

	#endregion

	#region Entereventgrp

	public class EntereventgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF UNK_0;
		public ASCF skill_sound;
		public FLOAT sound_vol;
		public FLOAT sound_rad;
		public UINT isrise;
		public UINT spawn_type;
		public UNICODE effect_name;
		public UNICODE anim_name;
	}

	#endregion

	#region Etcitemgrp

	public class EtcitemgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT id;
		public UINT drop_type;
		public UINT drop_anim_type;
		public UINT drop_radius;
		public UINT drop_height;
		public UINT UNK_0;
		public UNICODE drop_mesh;
		public UNICODE drop_tex;
		public UNICODE icon1;
		public UNICODE icon2;
		public UNICODE icon3;
		public UNICODE icon4;
		public UNICODE icon5;
		public UINT durability;
		public UINT weight;
		public UINT material;
		public UINT crystallizable;
		public UINT UNK_1;
		public MTX mesh_tex_pair;
		public UNICODE item_sound;
		public UNICODE equip_sound;
		public UINT stackable;
		public UINT family;
		public UINT grade;
	}

	public class EtcitemgrpInfo_C4 : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT id;
		public UINT drop_type;
		public UINT drop_anim_type;
		public UINT drop_radius;
		public UINT drop_height;
		public UINT UNK_0;
		public UNICODE drop_mesh1;
		public UNICODE drop_mesh2;
		public UNICODE drop_mesh3;
		public UNICODE drop_tex1;
		public UNICODE drop_tex2;
		public UNICODE drop_tex3;
		public UNICODE icon1;
		public UNICODE icon2;
		public UNICODE icon3;
		public UNICODE icon4;
		public UNICODE icon5;
		public UINT durability;
		public UINT weight;
		public UINT material;
		public UINT crystallizable;
		public UINT UNK_1;
		public MTX mesh_tex_pair;
		public UNICODE item_sound;
		public UNICODE equip_sound;
		public UINT stackable;
		public UINT family;
		public UINT grade;
	}

	public class EtcitemgrpInfo_CT1 : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT id;
		public UINT drop_type;
		public UINT drop_anim_type;
		public UINT drop_radius;
		public UINT drop_height;
		public UINT UNK_0;
		public UNICODE drop_mesh1;
		public UNICODE drop_mesh2;
		public UNICODE drop_mesh3;
		public UNICODE drop_tex1;
		public UNICODE drop_tex2;
		public UNICODE drop_tex3;
		public UNICODE drop_extratex1;
		public UINT newdata1;
		public UINT newdata2;
		public UINT newdata3;
		public UINT newdata4;
		public UINT newdata5;
		public UINT newdata6;
		public UINT newdata7;
		public UINT newdata8;
		public UNICODE icon1;
		public UNICODE icon2;
		public UNICODE icon3;
		public UNICODE icon4;
		public UNICODE icon5;
		public INT durability;
		public UINT weight;
		public UINT material;
		public UINT crystallizable;
		public UINT UNK_1;
		public CNTINT_PAIR UNK_2;
		public UINT UNK_3;
		public UNICODE fort;
		public MTX mesh_tex_pair;
		public UNICODE item_sound;
		public UNICODE equip_sound;
		public UINT stackable;
		public UINT family;
		public UINT grade;
	}

	#endregion

	#region Eula

	public class EulaInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UNICODE eula;
		public UNICODE fin1;
		public UNICODE fin2;
		public UNICODE fin3;
	}

	#endregion

	#region GameTip

	public class GameTipInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT int1;
		public UINT int2;
		public UINT enable;
		public ASCF tip;
	}

	#endregion

	#region Hairaccessorylocgrp

	public class HairaccessorylocgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		}
		*/
		public UNICODE name;
		public FLOAT floats_1_1;
		public FLOAT floats_1_2;
		public FLOAT floats_1_3;
		public INT ints_1_1;
		public INT ints_1_2;
		public INT ints_1_3;
		public FLOAT floats_2_1;
		public FLOAT floats_2_2;
		public FLOAT floats_2_3;
		public INT ints_2_1;
		public INT ints_2_2;
		public INT ints_2_3;
		public FLOAT floats_3_1;
		public FLOAT floats_3_2;
		public FLOAT floats_3_3;
		public INT ints_3_1;
		public INT ints_3_2;
		public INT ints_3_3;
		public FLOAT floats_4_1;
		public FLOAT floats_4_2;
		public FLOAT floats_4_3;
		public INT ints_4_1;
		public INT ints_4_2;
		public INT ints_4_3;
		public FLOAT floats_5_1;
		public FLOAT floats_5_2;
		public FLOAT floats_5_3;
		public INT ints_5_1;
		public INT ints_5_2;
		public INT ints_5_3;
		public FLOAT floats_6_1;
		public FLOAT floats_6_2;
		public FLOAT floats_6_3;
		public INT ints_6_1;
		public INT ints_6_2;
		public INT ints_6_3;
		public FLOAT floats_7_1;
		public FLOAT floats_7_2;
		public FLOAT floats_7_3;
		public INT ints_7_1;
		public INT ints_7_2;
		public INT ints_7_3;
		public FLOAT floats_8_1;
		public FLOAT floats_8_2;
		public FLOAT floats_8_3;
		public INT ints_8_1;
		public INT ints_8_2;
		public INT ints_8_3;
		public FLOAT floats_9_1;
		public FLOAT floats_9_2;
		public FLOAT floats_9_3;
		public INT ints_9_1;
		public INT ints_9_2;
		public INT ints_9_3;
		public FLOAT floats_A_1;
		public FLOAT floats_A_2;
		public FLOAT floats_A_3;
		public INT ints_A_1;
		public INT ints_A_2;
		public INT ints_A_3;
		public FLOAT floats_B_1;
		public FLOAT floats_B_2;
		public FLOAT floats_B_3;
		public INT ints_B_1;
		public INT ints_B_2;
		public INT ints_B_3;
		public FLOAT floats_C_1;
		public FLOAT floats_C_2;
		public FLOAT floats_C_3;
		public INT ints_C_1;
		public INT ints_C_2;
		public INT ints_C_3;
		public FLOAT floats_D_1;
		public FLOAT floats_D_2;
		public FLOAT floats_D_3;
		public INT ints_D_1;
		public INT ints_D_2;
		public INT ints_D_3;
		public FLOAT floats_E_1;
		public FLOAT floats_E_2;
		public FLOAT floats_E_3;
		public INT ints_E_1;
		public INT ints_E_2;
		public INT ints_E_3;
		public FLOAT floats_F_1;
		public FLOAT floats_F_2;
		public FLOAT floats_F_3;
		public INT ints_F_1;
		public INT ints_F_2;
		public INT ints_F_3;
	}

	#endregion

	#region Hennagrp

	public class HennagrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT dye_id;
		public ASCF name;
		public ASCF icon;
		public ASCF symbol_add_name;
		public ASCF symbol_add_desc;
	}

	#endregion

	#region HuntingZone

	public class HuntingZoneInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT hunting_type;
		public UINT level;
		public UINT UNK_1;
		public FLOAT loc_x;
		public FLOAT loc_y;
		public FLOAT loc_z;
		public CHEX UNK_2;
		public UINT affiliated_area_id;
		public ASCF name;
	}

	#endregion

	#region InstantZoneData

	public class InstantZoneDataInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF name;
	}

	#endregion

	#region ItemName

	public class ItemNameInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UNICODE name;
		public UNICODE additionalname;
		public ASCF description;
		public INT popup;
		public ASCF set_ids;
		public ASCF set_bonus_desc;
		public ASCF set_extra_id;
		public ASCF set_extra_desc;
		public CHEX UNK1_1;
		public CHEX UNK1_2;
		public UINT special_enchant_amount;
		public ASCF special_enchant_desc;
		public UINT UNK2;
	}

	public class ItemNameInfo_CT3 : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UNICODE name;
		public UNICODE additionalname;
		public ASCF description;
		public INT popup;
		public UINT supercnt0;
		public CNTTXT_PAIR set_ids;
		public ASCF set_bonus_desc;
		public UINT supercnt1;
		public CNTTXT_PAIR set_extra_ids;
		public ASCF set_extra_desc;
		public CHEX UNK1_1;
		public CHEX UNK1_2;
		public CHEX UNK1_3;
		public CHEX UNK1_4;
		public CHEX UNK1_5;
		public CHEX UNK1_6;
		public CHEX UNK1_7;
		public CHEX UNK1_8;
		public CHEX UNK1_9;
		public UINT special_enchant_amount;
		public ASCF special_enchant_desc;
		public UINT UNK2;
	}

	#endregion

	#region ItemPrime

	public class ItemPrimeInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT val;
	}

	#endregion

	#region Logongrp

	public class LogongrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public INT x;
		public INT y;
		public INT z;
		public INT yaw;
	}

	#endregion

	#region Mobskillanimgrp

	public class MobskillanimgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT npc_id;
		public UINT skill_id;
		public UNICODE seq_name;
		public ASCF skill_name;
		public ASCF npc_name;
		public ASCF npc_class;
	}

	#endregion

	#region MusicInfo

	public class MusicInfoInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public CNTTXT_PAIR name;
	}

	#endregion

	#region Npcgrp

	public class NpcgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UNICODE npc_class;
		public UNICODE mesh;
		public CNTTXT_PAIR tex1;
		public CNTTXT_PAIR tex2;
		public CNTRINT_PAIR dtab1;
		public FLOAT npc_speed;
		public UINT UNK_0_OLD;
		public CNTTXT_PAIR UNK_0_NEW;
		public CNTTXT_PAIR snd1;
		public CNTTXT_PAIR snd2;
		public CNTTXT_PAIR snd3;
		public UINT rb_effect_on;
		public UNICODE rb_effect;
		public FLOAT rb_effect_fl;
		public UINT UNK_1_OLD;
		public CNTRINT_PAIR UNK_1_NEW;
		public UINT level_lim_dn;
		public UINT level_lim_up;
		public UNICODE effect;
		public UINT UNK_2;
		public FLOAT sound_rad;
		public FLOAT sound_vol;
		public FLOAT sound_rnd;
		public UINT quest_be;
		public UINT class_lim;
	}

	public class NpcgrpInfo_CT2_4 : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UNICODE npc_class;
		public UNICODE mesh;
		public CNTTXT_PAIR tex1;
		public CNTTXT_PAIR tex2;
		public CNTRINT_PAIR dtab1;
		public FLOAT npc_speed;
		public CNTTXT_PAIR UNK_0;
		public CNTTXT_PAIR snd1;
		public CNTTXT_PAIR snd2;
		public CNTTXT_PAIR snd3;
		public UINT rb_effect_on;
		public UNICODE rb_effect;
		public FLOAT rb_effect_fl;
		public CNTRINT_PAIR UNK_1;
		public CNTRINT_PAIR UNK_2;
		public UNICODE effect;
		public UINT UNK_3;
		public FLOAT sound_rad;
		public FLOAT sound_vol;
		public FLOAT sound_rnd;
		public UINT quest_be;
		public UINT class_lim;
		public CNTASCF_PAIR npcend;
		public UINT UNK_4;
	}
	#endregion

	#region NpcName

	public class NpcNameInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF name;
		public ASCF description;
		public Color rgb;
	}

	#endregion

	#region NpcString

	public class NpcStringInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF npcstring;
	}

	#endregion

	#region Obscene

	public class ObsceneInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF text;
	}

	#endregion

	#region Optiondata_Client

	public class Optiondata_ClientInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT level;
		public UINT var_of_level;
		public ASCF effect1_desc;
		public ASCF effect2_desc;
		public ASCF effect3_desc;
	}

	#endregion

	#region ProductName

	public class ProductNameInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UNICODE name;
		public ASCF str;
		public UNICODE icon;
	}

	#endregion

	#region QuestName

	public class QuestNameInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT quest_id;
		public UINT quest_prog;
		public ASCF main_name;
		public ASCF prog_name;
		public ASCF description;
		public CNTRINT_PAIR items;	   //list of items to get by item_id 
		public CNTRINT_PAIR num_items;   //num of each coressponding item (0 = infinity) 
		public FLOAT quest_x;			//x coord of current "pin" on map 
		public FLOAT quest_y;			//y coord of current "pin" on map 
		public FLOAT quest_z;			//z coord of current "pin" on map 
		public UINT lvl_min;			 //lvl req to start quest 
		public UINT lvl_max;			 //recommended lvl max 
		public UINT quest_type;		  //0 = quests that lead to rewards (varka, summoning rb, coin quest, etc), 1 = quests that lead to special items (lures, wedding dress), 2 = repeatable, 3 = one time 
		public ASCF entity_name;		 // 
		public UINT get_item_in_quest;   //1 = get item in quest part, 0 = no item obtained in quest 
		public UINT UNK_1;			   //1 = same tab stack, 0 = end of stack (ex: |11110|10| if ur in the 2nd stack id 6 or 7 in quest prog |12345|67| the displayed stack would be |167| in the display) 
		public UINT UNK_2;			   //no clue 
		public UINT contact_npc_id;	  //who starts the quest 
		public FLOAT contact_npc_x;	  //start quest x_loc 
		public FLOAT contact_npc_y;	  //start quest x_loc 
		public FLOAT contact_npc_z;	  //start quest x_loc 
		public ASCF restricions;		 //can be race or quest pre-reqs 
		public ASCF short_description;
		public CNTRINT_PAIR req_class;   //id of class that can do quest 
		public CNTRINT_PAIR req_item;	//id of items needed to do quest 
		public UINT clan_pet_quest;	  //0 = reg quest, 1 = pet/clan quest 
		public UINT req_quest_complete;  //id of quest that must be completed first 
		public UINT UNK_3;			   //unknown all 0 
		public UINT area_id;			 //area id (goddard, rune, giran, etc) 
		public UINT UNK_4;
		public CNTRINT_PAIR tab5;
		public CNTRINT_PAIR tab6;
		public CNTRINT_PAIR tab7;
	}

	#endregion

	#region RaidData

	public class RaidDataInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT npc_id;
		public UINT npc_level;
		public UINT affiliated_area_id;
		public FLOAT loc_x;
		public FLOAT loc_y;
		public FLOAT loc_z;
		public ASCF raid_desc;
	}

	#endregion

	#region Recipe

	public class RecipeInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public ASCF name;
		public UINT id_mk;
		public UINT id_recipe;
		public UINT level;
		public UINT id_item;
		public UINT count;
		public UINT mp_cost;
		public UINT success_rate;
		public string materials;
		public UINT UNK0;
	}

	#endregion

	#region RideData

	public class RideDataInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT extra;
		public UNICODE name;
		public FLOAT floats1;
		public FLOAT floats2;
		public FLOAT floats3;
		public FLOAT floats4;
		public FLOAT floats5;
		public FLOAT floats6;
		public FLOAT floats7;
		public FLOAT floats8;
		public FLOAT floats9;
		public FLOAT floats10;
		public FLOAT floats11;
		public FLOAT floats12;
		public FLOAT floats13;
		public FLOAT floats14;
		public FLOAT floats15;
		public FLOAT floats16;
		public FLOAT floats17;
		public FLOAT floats18;
		public FLOAT floats19;
		public FLOAT floats20;
		public FLOAT floats21;
		public FLOAT floats22;
		public FLOAT floats23;
		public FLOAT floats24;
		public FLOAT floats25;
		public FLOAT floats26;
		public FLOAT floats27;
		public FLOAT floats28;
		public FLOAT floats29;
		public FLOAT floats30;
		public FLOAT floats31;
		public FLOAT floats32;
		public FLOAT floats33;
		public FLOAT floats34;
		public FLOAT floats35;
		public FLOAT floats36;
		public FLOAT floats37;
		public FLOAT floats38;
		public FLOAT floats39;
		public FLOAT floats40;
		public FLOAT floats41;
		public FLOAT floats42;
		public FLOAT floats43;
		public FLOAT floats44;
		public FLOAT floats45;
		public FLOAT floats46;
		public FLOAT floats47;
		public FLOAT floats48;
		public FLOAT floats49;
		public FLOAT floats50;
		public FLOAT floats51;
		public FLOAT floats52;
		public FLOAT floats53;
		public FLOAT floats54;
		public FLOAT floats55;
		public FLOAT floats56;
		public FLOAT floats57;
		public FLOAT floats58;
		public FLOAT floats59;
		public FLOAT floats60;
		public FLOAT floats61;
		public FLOAT floats62;
		public FLOAT floats63;
		public FLOAT floats64;
		public FLOAT floats65;
		public FLOAT floats66;
		public FLOAT floats67;
		public FLOAT floats68;
		public FLOAT floats69;
		public FLOAT floats70;
		public FLOAT floats71;
		public FLOAT floats72;
		public FLOAT floats73;
		public FLOAT floats74;
		public FLOAT floats75;
		public FLOAT floats76;
		public FLOAT floats77;
		public FLOAT floats78;
		public FLOAT floats79;
		public FLOAT floats80;
		public FLOAT floats81;
		public FLOAT floats82;
		public FLOAT floats83;
		public FLOAT floats84;
		public FLOAT floats85;
		public FLOAT floats86;
		public FLOAT floats87;
		public FLOAT floats88;
		public FLOAT floats89;
		public FLOAT floats90;
		public FLOAT floats91;
		public FLOAT floats92;
		public FLOAT floats93;
		public FLOAT floats94;
		public FLOAT floats95;
		public FLOAT floats96;
		public FLOAT floats97;
		public FLOAT floats98;
		public FLOAT floats99;
		public FLOAT floats100;
		public FLOAT floats101;
		public FLOAT floats102;
		public FLOAT floats103;
		public FLOAT floats104;
		public FLOAT floats105;
		public FLOAT floats106;
		public FLOAT floats107;
		public FLOAT floats108;
		public FLOAT floats109;
		public FLOAT floats110;
		public FLOAT floats111;
		public FLOAT floats112;
		public FLOAT floats113;
		public FLOAT floats114;
		public FLOAT floats115;
		public FLOAT floats116;
		public FLOAT floats117;
		public FLOAT floats118;
		public FLOAT floats119;
	}

	#endregion

	#region ScenePlayerData

	public class ScenePlayerDataInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UNICODE str;
		public FLOAT dat;
	}

	#endregion
	
	#region ServerName

	public class ServerNameInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT server_id;
		public UINT tag;
		public ASCF server_name;
		public ASCF server_desc;
	}

	#endregion

	#region ShortcutAlias

	public class ShortcutAliasInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF name;
		public UINT val1;
		public UINT val2;
	}

	#endregion

	#region Skillgrp

	public class SkillgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT skill_id;
		public UINT skill_level;
		public UINT oper_type;
		public UINT mp_consume;
		public INT cast_range;
		public UINT cast_style;
		public FLOAT hit_time;
		public INT is_magic;
		public UNICODE ani_char;
		public UNICODE desc;
		public UNICODE icon_name;
		public UINT extra_eff;
		public UINT is_ench;
		public UINT ench_skill_id;
		public UINT hp_consume;
		public INT UNK_0;
		public INT UNK_1;
	}

	public class SkillgrpInfo_CT1 : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT skill_id;
		public UINT skill_level;
		public UINT oper_type;
		public UINT mp_consume;
		public INT cast_range;
		public UINT cast_style;
		public UINT UNK_0;
		public FLOAT hit_time;
		public INT is_magic;
		public UNICODE ani_char;
		public UNICODE desc;
		public UNICODE icon_name;
		public UNICODE icon_name2;
		public UINT is_ench;
		public UINT ench_skill_id;
		public UINT hp_consume;
        public ASCF nonetext1;
        public INT UNK_1;
        public INT UNK_2;
        public INT UNK_3;
        public INT UNK_4;
        public ASCF nonetext2;
    }

	#endregion

	#region SkillName

	public class SkillNameInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT level;
		public ASCF name;
		public ASCF description;
		public ASCF desc_add1;
		public ASCF desc_add2;
	}

	#endregion

	#region Skillsoundgrp

	public class SkillsoundgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT skill_id;
		public UINT skill_level;
		public UNICODE spelleffect_sound_1;
		public UNICODE spelleffect_sound_2;
		public UNICODE spelleffect_sound_3;
		public FLOAT spelleffect_sound_vol_1;
		public FLOAT spelleffect_sound_rad_1;
		public FLOAT spelleffect_sound_vol_2;
		public FLOAT spelleffect_sound_rad_2;
		public FLOAT spelleffect_sound_vol_3;
		public FLOAT spelleffect_sound_rad_3;
		public UNICODE shoteffect_sound_1;
		public UNICODE shoteffect_sound_2;
		public UNICODE shoteffect_sound_3;
		public FLOAT shoteffect_sound_vol_1;
		public FLOAT shoteffect_sound_rad_1;
		public FLOAT shoteffect_sound_vol_2;
		public FLOAT shoteffect_sound_rad_2;
		public FLOAT shoteffect_sound_vol_3;
		public FLOAT shoteffect_sound_rad_3;
		public UNICODE expeffect_sound_1;
		public UNICODE expeffect_sound_2;
		public UNICODE expeffect_sound_3;
		public FLOAT expeffect_sound_vol_1;
		public FLOAT expeffect_sound_rad_1;
		public FLOAT expeffect_sound_vol_2;
		public FLOAT expeffect_sound_rad_2;
		public FLOAT expeffect_sound_vol_3;
		public FLOAT expeffect_sound_rad_3;
		public UNICODE mfighter_sub;
		public UNICODE ffighter_sub;
		public UNICODE mdarkelf_sub;
		public UNICODE fdarkelf_sub;
		public UNICODE mdwarf_sub;
		public UNICODE fdwarf_sub;
		public UNICODE melf_sub;
		public UNICODE felf_sub;
		public UNICODE mmagic_sub;
		public UNICODE fmagic_sub;
		public UNICODE morc_sub;
		public UNICODE forc_sub;
		public UNICODE mshaman_sub;
		public UNICODE fshaman_sub;
		public UNICODE mkamael_sub;
		public UNICODE fkamael_sub;
		public UNICODE RESERVED_sub;
		public UNICODE mfighter_throw;
		public UNICODE ffighter_throw;
		public UNICODE mdarkelf_throw;
		public UNICODE fdarkelf_throw;
		public UNICODE mdwarf_throw;
		public UNICODE fdwarf_throw;
		public UNICODE melf_throw;
		public UNICODE felf_throw;
		public UNICODE mmagic_throw;
		public UNICODE fmagic_throw;
		public UNICODE morc_throw;
		public UNICODE forc_throw;
		public UNICODE mshaman_throw;
		public UNICODE fshaman_throw;
		public UNICODE mkamael_throw;
		public UNICODE fkamael_throw;
		public UNICODE RESERVED_throw;
		public FLOAT sound_vol;
		public FLOAT sound_rad;
	}

	#endregion

	#region SkillSoundSource

	public class SkillSoundSourceInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT shit1;
		public UINT shit2;
		public UINT shit3;
		public UINT shit4;
		public UINT shit5;
		public UINT shit6;
		public UINT shit7;
		public UINT shit8;
		public UINT shit9;
	}

	#endregion

	#region StaticObject

	public class StaticObjectInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UNICODE name;
	}

	#endregion

	#region SymbolName

	public class SymbolNameInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF filename;
		public ASCF alias;
		public UINT UNK_0;
	}

	#endregion

	#region SysString

	public class SysStringInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public ASCF name;
	}

	#endregion

	#region SystemMsg

	public class SystemMsgInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT UNK_0;
		public ASCF message;
		public UINT group;
		public Color rgb;
		public ASCF item_sound;
		public ASCF sys_msg_ref;
		public UINT UNK_1_1;
		public UINT UNK_1_2;
		public UINT UNK_1_3;
		public UINT UNK_1_4;
		public UINT UNK_1_5;
		public ASCF sub_msg;
		public ASCF type;
	}

	#endregion

	#region TransformData

	public class TransformDataInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UINT bin;
		public UINT npc_id;
		public UINT weapon_id;
		public UNICODE transform_effect_a;
		public UNICODE transform_effect_b;
		public UINT unk1;
		public UINT unk2;
		public UINT unk3;
		public UINT unk4;
	}

	#endregion

	#region Variationeffectgrp

	public class VariationeffectgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT int1;
		public UINT int2;
		public UINT int3;
		public UINT int4;
		public UINT int5;
		public UNICODE effect;
		public ASCF attribute;
	}

	#endregion

	#region Vehiclepartsgrp

	public class VehiclepartsgrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT id;
		public UNICODE name;
		public CNTRINT_PAIR tab1;
		public CNTRINT_PAIR tab2;
		public UINT unk1;
		public UINT unk2;
		public MTX mshtex;
		public UNICODE sounds1;
		public UNICODE sounds2;
		public UNICODE sounds3;
		public UNICODE sounds4;
		public UNICODE sounds5;
		public CNTTXT_PAIR sounds02;
	}

	#endregion

	#region Weapongrp

	public class WeapongrpInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT tag;
		public UINT id;
		public UINT drop_type;
		public UINT drop_anim_type;
		public UINT drop_radius;
		public UINT drop_height;
		public UINT UNK_0;
		public UNICODE drop_mesh1;
		public UNICODE drop_mesh2;
		public UNICODE drop_mesh3;
		public UNICODE drop_tex1;
		public UNICODE drop_tex2;
		public UNICODE drop_tex3;
		public UNICODE drop_extratex1;
		public UINT newdata1;
		public UINT newdata2;
		public UINT newdata3;
		public UINT newdata4;
		public UINT newdata5;
		public UINT newdata6;
		public UINT newdata7;
		public UINT newdata8;
		public UNICODE icon1;
		public UNICODE icon2;
		public UNICODE icon3;
		public UNICODE icon4;
		public UNICODE icon5;
		public INT durability;
		public UINT weight;
		public UINT material;
		public UINT crystallizable;
		public UINT UNK_1;
		public CNTINT_PAIR UNK_2;
		public UINT UNK_3;
		public UNICODE timetab;
		public UINT body_part;
		public UINT handness;
		public CNTTXT_PAIR wpn_mesh;
		public CNTTXT_PAIR2 wpn_mesh_new;
		public CNTTXT_PAIR wpn_tex;
		public CNTTXT_PAIR item_sound;
		public UNICODE drop_sound;
		public UNICODE equip_sound;
		public UNICODE effect;
		public UINT random_damage;
		public UINT patt;
		public UINT matt;
		public UINT weapon_type;
		public UINT crystal_type;
		public UINT critical;
		public INT hit_mod;
		public INT avoid_mod;
		public UINT shield_pdef;
		public UINT shield_rate;
		public UINT speed;
		public UINT mp_consume;
		public UINT SS_count;
		public UINT SPS_count;
		public UINT curvature;
		public UINT UNK_4;
		public INT is_hero;
		public UINT UNK_5;
		public UNICODE effA;
		public UNICODE effB;
		public UNICODE rangeA;
		public UNICODE rangeB;
		public FLOAT junk1A_1;
		public FLOAT junk1A_2;
		public FLOAT junk1A_3;
		public FLOAT junk1A_4;
		public FLOAT junk1A_5;
		public FLOAT junk1B_1;
		public FLOAT junk1B_2;
		public FLOAT junk1B_3;
		public FLOAT junk1B_4;
		public FLOAT junk1B_5;
		public FLOAT junk2A_1;
		public FLOAT junk2A_2;
		public FLOAT junk2A_3;
		public FLOAT junk2A_4;
		public FLOAT junk2A_5;
		public FLOAT junk2A_6;
		public FLOAT junk2B_1;
		public FLOAT junk2B_2;
		public FLOAT junk2B_3;
		public FLOAT junk2B_4;
		public FLOAT junk2B_5;
		public FLOAT junk2B_6;
		public INT junk3_1;
		public INT junk3_2;
		public INT junk3_3;
		public INT junk3_4;
		public INT junk3_5;
		public INT junk3_6;
		public UNICODE icons1;
		public UNICODE icons2;
		public UNICODE icons3;
		public UNICODE icons4;
	}

	#endregion

	#region ZoneName

	public class ZoneNameInfo : L2DatDefinition
	{
		/*
		Info from l2asm-disasm
		*/
		public UINT nbr;
		public UINT zone_color_id;
		public UINT x_world_grid;
		public UINT y_world_grid;
		public FLOAT top_z;
		public FLOAT bottom_z;
		public ASCF zone_name;
		public INT coords1;
		public INT coords2;
		public INT coords3;
		public INT coords4;
		public INT coords5;
		public INT coords6;
		public FLOAT UNK_1;
		public ASCF map;
		public UINT dupa;
	}

	#endregion
}
