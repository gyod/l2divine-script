#region Using directives

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using LmUtils;

#endregion

namespace L2DatEncDec
{
    public enum MsgList
    {
        SystemDir_Error,
        DirectoryDialog_Desc,
        MsgBoxTitle_Error,
        MsgBoxTitle_Information,
        COMPLETE,
        ERROR,
        ChronicleSetting,
        TextEncoding,
        PleaseWait,
        FileNotFound,
        DataIsEmpty,
        CompleteLoad,
        CompleteSave,
        CompleteImp,
        CompleteExp,
        CompletePatchSystem,
    }

    #region Localization

    public class Localization
    {
        private static Xml xml;
        public string LangPath
        {
            get
            {
                return LmUtils.GlobalUtilities.GetStartDirectory(false) + "lang";
            }
        }

        public Localization()
        {
            Reload();
        }

        public void Reload()
        {
            xml = new Xml(Path.Combine(LangPath, Program.config.LangFileName));
            xml.ThisCanThrowExeptions = true;
            xml.Reload();
        }

        public string getMessage(MsgList msg)
        {
            if (xml.GetNode("Messages") == null)
                return msg.ToString() + " ";
            if (xml.GetNode("Messages." + msg.ToString()) != null)
                return xml["Messages." + msg.ToString()];
            else
                return msg.ToString();
        }

        public void setLocalization(Form targetForm)
        {
            if (xml.GetNode(targetForm.Name) == null) return;

            MemberInfo[] members = targetForm.GetType().GetMembers(
                                        BindingFlags.Public | BindingFlags.Instance);
            foreach (MemberInfo m in members)
            {
                if (m.MemberType == MemberTypes.Field)
                {
                    FieldInfo FType = targetForm.GetType().GetField(m.Name);
                    if (FType == null) continue;
                    if (FType.GetValue(targetForm) != null)
                    {
                        if (!FType.FieldType.FullName.StartsWith("System.Windows.Forms."))
                            continue;
                        string NodeName = targetForm.Name + "." + m.Name;
                        if (xml.GetNode(NodeName) != null)
                        {
                            if (FType.FieldType.FullName.EndsWith("Button"))
                                ((Button)FType.GetValue(targetForm)).Text = xml[NodeName];
                            else if (FType.FieldType.FullName.EndsWith("Label"))
                                ((Label)FType.GetValue(targetForm)).Text = xml[NodeName];
                            else if (FType.FieldType.FullName.EndsWith("ToolStripMenuItem"))
                                ((ToolStripMenuItem)FType.GetValue(targetForm)).Text = xml[NodeName];
                        }
                        else
                        {
                            if (FType.FieldType.FullName.EndsWith("Button"))
                                xml[NodeName] = ((Button)FType.GetValue(targetForm)).Text;
                            else if (FType.FieldType.FullName.EndsWith("Label"))
                                xml[NodeName] = ((Label)FType.GetValue(targetForm)).Text;
                            else if (FType.FieldType.FullName.EndsWith("ToolStripMenuItem"))
                                xml[NodeName] = ((ToolStripMenuItem)FType.GetValue(targetForm)).Text;
                        }
                    }
                }
            }
        }
    }

    #endregion
}
