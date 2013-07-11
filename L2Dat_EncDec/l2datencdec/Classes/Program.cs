using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace L2DatEncDec
{
	static class Program
	{
		public static Config config;
        public static Localization language;
        public static LmUtils.Log log;
		public static MainForm main_form;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main ()
		{
			Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault(false);

            Program.log = new LmUtils.Log("Log.xml");


			Program.log.Add ("Program started");
            Program.config = new Config();
            Program.language = new Localization();
			Program.log.LogHistory = Program.config.LogHistory;

            Program.main_form = new MainForm();

            MessageBox.parentForm = Program.main_form;

			try
			{
                Application.Run(Program.main_form);
                Program.log.Add("Quit program");
			}
			catch (Exception e)
			{
				Program.log.Add (e);
			}
		}
	}
}