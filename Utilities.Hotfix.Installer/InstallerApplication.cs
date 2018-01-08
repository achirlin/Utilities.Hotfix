using System;
using System.Windows.Forms;

namespace Utilities.Hotfix
{
	class InstallerApplication
	{
		[STAThread]
		public static void Main()
		{
			HotfixInstaller installer = null;
			
			try
			{
				installer = new HotfixInstaller();
				installer.InstallationStartedEvent		+=	new InstallationStartedDelegate(installer_InstallationStartedEvent);
				installer.InstallationCompletedEvent	+=	new InstallationCompletedDelegate(installer_InstallationCompletedEvent);
				installer.Install();
			}
			catch(Exception ex)
			{
				#region	Diagnostic message describing thrown exception
				
				string message = string.Empty;
				System.Text.StringBuilder objMessageBuilder = new System.Text.StringBuilder();

				objMessageBuilder.AppendFormat("HOTFIX INSTALLATION TERMINATED WITH ERROR.{0}",Environment.NewLine);
				objMessageBuilder.AppendFormat("Check log file for details: {0}{1}", installer.LogFileName,Environment.NewLine);
 				objMessageBuilder.AppendFormat("Exception thrown:{0}{1}",Environment.NewLine, ex.ToString());
				
				message = objMessageBuilder.ToString();
				
				Console.WriteLine(message);

				if(installer != null && installer.Log != null)
				{
					installer.Log.WriteLine(LogType.Error, "HI050: " + message);
					installer.Log.Show();
				}
				
				#endregion // Diagnostic message describing thrown exception
			}

		}

		#region Event Handlers
		
		/// <summary>
		/// Handles last chanse before installation
		/// </summary>
		/// <param name="blnCancel"></param>
		/// <param name="strDescription"></param>
		private static void installer_InstallationStartedEvent(ref bool blnCancel, string strDescription)
		{
			DialogResult mResult;
			blnCancel = false;
			
			mResult  = MessageBox.Show(	null,
				string.Format("Are you sure that you want to install: {0}", strDescription),
				"",
				MessageBoxButtons.YesNo);
			
			if(mResult == DialogResult.No)
			{
				blnCancel=true;
			}
		}

		private static void installer_InstallationCompletedEvent(string strDescription, string message)
		{
			Console.WriteLine("{0}: Installation completed. {1}.", strDescription, message);
		}
		#endregion // Event Handlers

	}
}
