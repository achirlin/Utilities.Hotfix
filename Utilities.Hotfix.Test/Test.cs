using System;				// Exception
using System.IO;			// Path
using System.Windows.Forms;	// Application, MessageBox
using Utilities.Hotfix;
using Utilities.Hotfix.Xml;
using System.Collections;
using System.ServiceProcess;

namespace Utilities.Hotfix.Test
{
	class Test
	{
		private HotfixSettings		objSettigns;
		private static Logger log = new Logger(Logger.GetNewLogFilename());

		public Test()
		{
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

		public static void Main()
		{
			Test objMain;
			try
			{
				objMain = new Test();
				if(objMain != null)
				{
					//objMain.TestServiceProcessor();
					//objMain.TestInstallXmlConfigAdmin();
					//objMain.TestInstallXmlConfigWebSite();
					//objMain.TestInstallXmlConfigEcm4P8();
					//objMain.TestSettingSaveLoad();
					//objMain.ClearDotNET11TemporaryCache();
					//objMain.TestXDirectoryClass();
					//objMain.TestCopyAttributes();
					//objMain.TestWebSiteXmlUpdate();
					//objMain.TestStartServices();
					//objMain.TestService11exeConfigXmlUpdate();
					//objMain.TestLEUMI();
					//objMain.TestIssue6inHF6();
					//objMain.TestIssue5inHF6();
					//objMain.TestIssueCopyCodebase();
					//objMain.TestIssueContentElements();
					//objMain.TestMessageRouter();
					//objMain.TestHF6Issue10();
					//objMain.TestHF6Issue12();
					objMain.TestWebConfigViewModal();
				}
			}
			catch(Exception thrown)
			{
				objMain=null;
				
				string strMessage = string.Format("Message: {0}{1}{0}{0}Internal Error:{0}{2}", 
													Environment.NewLine,
													thrown.Message,
													thrown.InnerException!=null ? thrown.InnerException.Message: "");
				MessageBox.Show(strMessage);
			}
			finally
			{
				log.Show();
			}

		}


		public bool TestServiceProcessor()
		{
			ServiceProcessor objServiceProcessor = new ServiceProcessor(objSettigns,log);
			objServiceProcessor.Stop();
			objServiceProcessor.Restore();
			objServiceProcessor.Stop();
			objServiceProcessor.Restore();
			objServiceProcessor.Stop();
			objServiceProcessor.Restore();
			objServiceProcessor.Stop();
			objServiceProcessor.Restore();
			objServiceProcessor.Stop();
			objServiceProcessor.Restore();
			return true;
		}

		public bool TestStartServices()
		{
			ServiceProcessor objServiceProcessor = new ServiceProcessor(objSettigns,log);
			IEnumerator ServiceEnum = objServiceProcessor.GetEnumerator();
			while(ServiceEnum.MoveNext())
			{
				ServiceProcessor.NTService service = ((ServiceProcessor.NTService)ServiceEnum.Current);
				service.StatusBefore = ServiceControllerStatus.Running;
			}
			objServiceProcessor.Restore();
			return true;
		}

		public bool TestInstallXmlConfigAdmin()
		{
			Logger log = new Logger(Logger.GetNewLogFilename());
			string strConfigAdmin = @"c:\Hotfix 5\Setup\Config. Admin.xml";
			XmlConfigurationHotfix updateAdmin = XmlConfigurationHotfix.Load(strConfigAdmin);
			updateAdmin.objXMLContentFixes[0].contentXmlFilePath=@"c:\Inetpub\wwwroot\Admin\Web.config";
			updateAdmin.Install(log);
			return true;
		}

		public bool TestInstallXmlConfigWebSite()
		{
			Logger log = new Logger(Logger.GetNewLogFilename());
			string strConfig = @"c:\Hotfix 5\Setup\Config. Web Site.xml";
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"c:\Inetpub\wwwroot\\Web.config";
			update.Install(log);
			return true;
		}

		public bool TestInstallXmlConfigEcm4P8()
		{
			Logger log = new Logger(Logger.GetNewLogFilename());
			string strConfig = @"c:\Hotfix 5\Setup\Config.  Permission Manager Service.xml";
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"c:\Program Files\ Solutions\  Permission Manager Service\.PermissionManager.Provider.Service.exe.config";
			update.Install(log);
			return true;
		}

		public bool TestSettingSaveLoad()
		{
			Console.WriteLine("Test Loading and Saving of configuration file");
			objSettigns = HotfixSettings.load(HotfixSettings.SETTINGS_FILENAME);
			objSettigns.save(HotfixSettings.SETTINGS_FILENAME + ".bak");
			return true;
		}

		public bool ClearDotNET11TemporaryCache()
		{
			Console.WriteLine("Test cleaning of cache of ASP.NET");
			objSettigns.mobjClearTempFiles.blnIsDoClear=true;
			return objSettigns.mobjClearTempFiles.Do(log);
		}

		public bool TestXDirectoryClass()
		{
			Console.WriteLine("Test xDirectory Class");
			xDirectory.arrExcludeFileFilters = new string[]{"*.lo*","*.dll"};
			xDirectory.Copy(@"c:\ALEXANDER\SUBJECTS\28.05.2007 InstallManager\Sources III\Utilities.Hotfix\Bin\Release",
							@"c:\ALEXANDER\SUBJECTS\28.05.2007 InstallManager\Sources III\Utilities.Hotfix\Bin\Debug\");
			return true;
		}

		public bool TestCopyAttributes()
		{
			Console.WriteLine("Test Copying of attributes ");
			string strConfig = @"c:\Delete\TestHotfix\hotfix\ConfigChangeTest.xml";
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"c:\Program Files\! Solutions Test\Service A\Web.Config";
			update.Install(log);
			return true;
		}
		
		public bool TestWebSiteXmlUpdate()
		{
			Console.WriteLine("Test Copying of attributes ");
//			string strConfig = @"D:\Tech\\Release Branches\5.0 SP1\Hotfixes\Hotfix 5\Setup\Config. Web Site.xml";
			string strConfig = @"c:\Config. URL.xml";
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"c:\Inetpub\wwwroot\\Copy of Web.config";
			update.Install(log);
			return true;
		}
		
		public bool TestService11exeConfigXmlUpdate()
		{
			Console.WriteLine("TestService11exeConfigXmlUpdate");
			string strConfig = @"C:\Config.Engine Service.xml" ;
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"c:\Service1.1.exe.config";
			update.Install(log);
			return true;
		}

		public bool TestLEUMI()
		{
			Console.WriteLine("TestLEUMI");
			string strConfig = @"c:\LEUMI\TestLeumi\Config. Admin.AdminConfig.xml" ;
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"c:\LEUMI\TestLeumi\AdminConfig.xml";
			update.Install(log);
			return true;
		}

		public bool TestIssue6inHF6()
		{
			Console.WriteLine("TestIssue6inHF6");
			string strConfig = @"c:\ALEXANDER\SUBJECTS\29.08.2007 HF6 Config Changes\Test Issue 6\Config.Issue6.xml" ;
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"c:\ALEXANDER\SUBJECTS\29.08.2007 HF6 Config Changes\Test Issue 6\Copy of Issue6TestWeb.origin.config";
			update.objXMLContentFixes[1].contentXmlFilePath=@"c:\ALEXANDER\SUBJECTS\29.08.2007 HF6 Config Changes\Test Issue 6\Copy of Issue6TestWeb.origin.config";
			update.Install(log);
			return true;
		}
		
		public bool TestIssue5inHF6()
		{
			Console.WriteLine("TestIssue5inHF6");
			string strConfig = @"c:\ALEXANDER\SUBJECTS\29.08.2007 HF6 Config Changes\Test Issue 6\Config.Issue5.xml" ;
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"c:\ALEXANDER\SUBJECTS\29.08.2007 HF6 Config Changes\Test Issue 6\Copy of Issue5TestWeb.origin.config";
			update.Install(log);
			return true;
		}

		public bool TestIssueCopyCodebase()
		{
			Console.WriteLine("TestIssueCopyCodebase");
			string strConfig = @"c:\ALEXANDER\SUBJECTS\29.08.2007 HF6 Config Changes\TestIssueCopyCodebase\Config. Web Site.xml" ;
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"c:\ALEXANDER\SUBJECTS\29.08.2007 HF6 Config Changes\TestIssueCopyCodebase\Copy of Web.config";
			update.objXMLContentFixes[1].contentXmlFilePath=update.objXMLContentFixes[0].contentXmlFilePath;
			update.objXMLContentFixes[2].contentXmlFilePath=update.objXMLContentFixes[0].contentXmlFilePath;
			update.Install(log);
			return true;
		}

		public bool TestIssueContentElements()
		{
			Console.WriteLine("ContentElements");
			string strConfig = @"d:\ALEXANDER\SUBJECTS\2007.09.03 Hotfix 6 Issues\ContentElements\Config. Web Site.xml" ;
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"d:\ALEXANDER\SUBJECTS\2007.09.03 Hotfix 6 Issues\ContentElements\Copy of Web.config";
			update.Install(log);
			return true;
		}
		public bool TestMessageRouter()
		{
			Console.WriteLine("MessageRouter");
			string strConfig = @"d:\ALEXANDER\SUBJECTS\2007.09.03 Hotfix 6 Issues\Message\Changes.xml" ;
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"d:\ALEXANDER\SUBJECTS\2007.09.03 Hotfix 6 Issues\Message\Copy of Message.xml";
			update.Install(log);
			return true;
		}
		public bool TestHF6Issue10()
		{
			Console.WriteLine("TestHF6Issue10");
			string strConfig = @"d:\ALEXANDER\SUBJECTS\2007.09.03 Hotfix 6 Issues\Issue 10 ComputerName\Changes.xml" ;
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"d:\ALEXANDER\SUBJECTS\2007.09.03 Hotfix 6 Issues\Issue 10 ComputerName\Copy of Web.config";
			update.Install(log);
			return true;
		}
		public bool TestHF6Issue12()
		{
			Console.WriteLine("TestHF6Issue12");
			string strConfig = @"d:\ALEXANDER\SUBJECTS\2007.09.03 Hotfix 6 Issues\Admin\Check\Partial.Config. Admin.xml" ;
			XmlConfigurationHotfix update = XmlConfigurationHotfix.Load(strConfig);
			update.objXMLContentFixes[0].contentXmlFilePath=@"d:\ALEXANDER\SUBJECTS\2007.09.03 Hotfix 6 Issues\Admin\Check\NOT_Working_Admin_Web.config";
			update.Install(log);
			return true;
		}

		public bool TestWebConfigViewModal()
		{
			Console.WriteLine("TestWebConfigViewModal");

			XmlConfigurationHotfix update = new XmlConfigurationHotfix();
			update.objXMLContentFixes = new XmlContentFix[1];
			XmlContentFix[] xmlFixes = update.objXMLContentFixes;

			xmlFixes[0] = new XmlContentFix();
			xmlFixes[0].description = "Add openModal=\"true\" attribute to View nodes";
			xmlFixes[0].objContentFixAttribute = 
				new ContentFixAttribute(
					new Utilities.Hotfix.Xml.Attribute[]
					{
						new Utilities.Hotfix.Xml.Attribute(
						@"/configuration/uipConfiguration/views/view",
						"openModal",
						"true")
					},
					null,
					null,
					null);

			xmlFixes[0].contentXmlFilePath = @"D:\Tech\5.2\\.W2.Web.Application\Web.config";
			update.Save(@"c:\.Web.config.openModal.xml");
			update.Install(log);


			return true;


		}
	}
}