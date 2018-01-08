using System;				// Exception
using System.IO;			// Path
using System.Windows.Forms;	// Application, MessageBox
using Utilities.Hotfix;
using Utilities.Hotfix.Xml;
using System.Collections;
using System.ServiceProcess;

namespace Utilities.Hotfix.StartService
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class StartService
	{

		private static HotfixSettings		objSettigns;
		private static Logger log = new Logger(Logger.GetNewLogFilename());

		static StartService(){
			#region Try to load settings file
			try
			{
				objSettigns = HotfixSettings.load(HotfixSettings.SETTINGS_FILENAME);
			}
			catch(Exception inner)
			{
				string strMessage = string.Format("Failed to load settings file: {0}{1}",
					Environment.NewLine,
					Path.Combine(Application.StartupPath,
					HotfixSettings.SETTINGS_FILENAME)
					);
				throw new System.InvalidOperationException(strMessage,inner);
			}
			#endregion // Try to load settings file
		}

		[STAThread]
		static int Main(string[] args)
		{
			if(args.Length>0)
			{
				ServiceProcessor objServiceProcessor = new ServiceProcessor(objSettigns,log);
				IEnumerator ServiceEnum = objServiceProcessor.GetEnumerator();
				while(ServiceEnum.MoveNext())
				{
					ServiceProcessor.NTService service = ((ServiceProcessor.NTService)ServiceEnum.Current);

					bool blnFound = false;
					foreach(string strServiceName in args)
					{
						if(strServiceName == service.strServiceName)
						{
							blnFound = true;
							break;
						}
					}

					if(blnFound)
					{
						service.StatusBefore = ServiceControllerStatus.Running;
					}

				}
				return objServiceProcessor.Restore()?0:1;
			}
			else
			{
				System.Console.WriteLine("Usage: Utilities.Hotfix.StartService <list> ");
				System.Console.WriteLine("	     <list>  - list of space separeted service names from hotfix.xml");
				System.Console.WriteLine("	               Service name is case sensitive.");
			}
			return 1;
		}
	}
}
