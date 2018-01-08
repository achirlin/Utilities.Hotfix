using System;
using System.ServiceProcess;
using System.Collections;
using System.IO;
using System.Diagnostics;

using Utilities.Hotfix.Xml;
namespace Utilities.Hotfix
{
	public delegate void InstallationStartedDelegate(ref bool blnCancel, string strDescription);
	public delegate void InstallationCompletedDelegate(string strDescription, string message);

	public class HotfixInstaller
	{
		public event InstallationStartedDelegate	InstallationStartedEvent;
		public event InstallationCompletedDelegate	InstallationCompletedEvent;

		// folder contains components to installation
		private	string			hRootPath;
		HotfixSettings			settings			= new HotfixSettings();
		Logger					log					= new Logger(Logger.GetNewLogFilename());
		ArrayList				processed			= new ArrayList();
		ServiceProcessor		objServiceProcessor;
		private string			SettingsFile;


		/// <summary>
		/// Logger object valid for this installer object
		/// </summary>
		public Logger Log
		{
			get
			{
				return this.log;
			}
		}

		public string LogFileName
		{
			get
			{
				return log.LogFileName;
			}
		}

		/// <summary>
		/// Initialize instance of installer by default pathes to: settings and components folder
		/// </summary>
		public HotfixInstaller()
		{
			// means take the relative path to components folder from settings file
			hRootPath = null;
			SettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
										HotfixSettings.SETTINGS_FILENAME);
		}

		/// <summary>
		/// Initialize instance of installer
		/// </summary>
		/// <param name="SettingsFile">full qualified path to hotfix configuration file</param>
		/// <param name="hrootpath">path to folder contains intended to install components (folders)</param>
		public HotfixInstaller(string SettingsFile, string hRootPath)
		{
			this.hRootPath		= hRootPath;
			this.SettingsFile	= SettingsFile;			
		}
		
		
		public void Install()
		{
			try
			{ 
				log.WriteLine(LogType.Empty,"");
				log.WriteLine(LogType.Empty,"");
				string message = "INSTALLATION STARTED, PLEASE WAIT.";
				Console.WriteLine(message);
				log.WriteLine(LogType.Info, "HI058: " + message);

				//-------------------------------------------------------
				// Initilize installer object by settings given
				//-------------------------------------------------------
				settings = HotfixSettings.load(SettingsFile);

				//-------------------------------------------------------
				// Get last chance before installation
				//-------------------------------------------------------
				#region Get last chance before installation
				bool blnCancel = false;
				if (InstallationStartedEvent != null)
				{
					InstallationStartedEvent(ref blnCancel, settings.strGeneralDescription);
					if(blnCancel)
					{
						message = "INSTALLATION CANCELLED.";
						Console.WriteLine(message);
						log.WriteLine(LogType.Info, "HI014: " + message);
						return;
					}
				}
				#endregion // Get last chance before installation


				//-------------------------------------------------------
				// Print header of machine/user/os attributes
				//-------------------------------------------------------
				log.WriteLine(LogType.Info, string.Format(@"HI059: Machine          :\\{0}\{1}", Environment.UserDomainName,Environment.MachineName));
				log.WriteLine(LogType.Info, string.Format(@"HI060: Username         :{0}", Environment.UserName));
				log.WriteLine(LogType.Info, string.Format(@"HI061: Framework        :{0}", Environment.Version.ToString()));
				log.WriteLine(LogType.Info, string.Format(@"HI062: OS Version       :{0}", Environment.OSVersion.ToString()));
				log.WriteLine(LogType.Info, string.Format(@"HI063: Current directory:{0}", Environment.CurrentDirectory));
				

				//-------------------------------------------------------
				// Initialize "hotfix root folder" (where installed components are)
				// relative to start up path
				//-------------------------------------------------------
				DirectoryInfo hRootFolder = null;
				if(hRootPath == null){
					// determine path to installed components( relative/full qualified)
					if(settings.ComponentsFolderSetting.blnIsRelative)
					{
						hRootPath= Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
												settings.ComponentsFolderSetting.strFolder);
					}
					else
					{
						hRootPath = settings.ComponentsFolderSetting.strFolder;
					}
				}
				hRootFolder = new DirectoryInfo(hRootPath);
				
				//-------------------------------------------------------
				// Initialize list of installed components (processed directories)
				// and possible destinations
				//-------------------------------------------------------
				#region Initialize list of processed directories
				foreach(DirectoryInfo sub in hRootFolder.GetDirectories())
				{
					//if not excluded from installed components
					if(!settings.IsExcluded(sub))
					{
						// found out destination folder of installed component
						try
						{
							//exception will be thrown for some wrong arguments
							DirectoryInfo[] dest = DiscoverDest(sub);
							
							// add to list of installed components
							SrcDestPair pair = new SrcDestPair(sub, dest);
							processed.Add(pair);
						}
						catch(ArgumentException ex)
						{
							log.WriteLine(LogType.Error,"HI001: " + ex.ToString());
						}
					}
				}//foreach
				
				if(processed.Count == 0)
				{
					string strMessage = string.Format("Nothing found to be installed.{0}Check hotfix root folder: {1}.{0}Check the exlusions list.", Environment.NewLine, hRootFolder.FullName);
					log.WriteLine(LogType.Info,"HI002: " + strMessage);
					return;
				}
				#endregion

				//-------------------------------------------------------
				// Load NT Service processor with ONLY actually installed services
				//-------------------------------------------------------
				objServiceProcessor = new ServiceProcessor(this.settings,this.log);
			
				//-------------------------------------------------------
				// Checking available free disk space
				//-------------------------------------------------------
				#region Checking available free disk space
				bool blnContinue = true;
				HotfixBackup objBackup = new HotfixBackup(settings,log,processed);
				if(this.settings.Backup.blnIsDoBackup)
				{
					Console.WriteLine("Checking available free disk space.");
						
					long lngAvailable   = objBackup.CalclateAvailable();
					long lngRequired	= objBackup.CalculateRequiredFreeSpace();

					message = string.Format("There is not enough free disk space for backuping. Required: {0}MB, Available: {1}MB",
						DriveInfoSystem.ToMB(lngRequired),
						DriveInfoSystem.ToMB(lngAvailable));
					
					if(lngAvailable <= lngRequired)
					{
						log.WriteLine(LogType.Error, "HI055: " + message);
						blnContinue = false;
						return;
					}
					else
					{
						message = "[Checking available free disk space]";
						log.WriteLine(LogType.Info, "HI057: " + message);
						message = string.Format("Required: {0}MB, Available: {1}MB",
							DriveInfoSystem.ToMB(lngRequired),
							DriveInfoSystem.ToMB(lngAvailable));
						log.WriteLine(LogType.Info, "HI056: " + message);
					}
				}
				#endregion // Checking available free disk space


				//-------------------------------------------------------
				// Save current Status of service and Stop running services
				//-------------------------------------------------------
				Console.WriteLine("Stopping services.");
				
				log.WriteLine(LogType.Info,"HI003: " + "[STOP controlled services]");
				
				if( objServiceProcessor.Stop() )
				{
					log.WriteLine(LogType.Info,"HI004: " + "Controlled services stopped successfully.");

					//-------------------------------------------------------
					// Backup installed components in format of hotfix
					//-------------------------------------------------------
					#region Backup installed components in format of hotfix
					
					if(this.settings.Backup.blnIsDoBackup)
					{
						Console.WriteLine("Backuping installed components.");
						message = "[Backuping installed components]";
						
						log.WriteLine(LogType.Info,"HI008: " + message);
						
						blnContinue = objBackup.doBackup();
						
						if(!blnContinue)
						{
							message = "Failed to backup installed components. Can't continue.";
							log.WriteLine(LogType.Error, "HI005: " + message);
							return;
						}
					}
					#endregion

					//-------------------------------------------------------
					// Process each item in list of components. 
					// Copying from hotfix to destination folder.
					//-------------------------------------------------------
					Console.WriteLine("Installing components.");
					#region Process each installed directory
					log.WriteLine(LogType.Info,"HI006: " + "[INSTALLING Hotfix components]: " + processed.Count);
					foreach(SrcDestPair item in processed)
					{
						try
						{
							foreach(DirectoryInfo destPath in item.destDir)
							{
								if(destPath.Exists)
								{
									log.WriteLine(LogType.Info,"HI007: " + 
										String.Format(@"FOLDER: ""{0}"" found, INSTALLING ...", destPath.FullName)
										);
									xDirectory.Copy(item.srcDir,destPath);
								}
								else
								{
									//skipped
									log.WriteLine(LogType.Info,"HI008: " +  String.Format(@"FOLDER: ""{0}"" is not installed, skipped.", destPath.FullName));
								}
							}
						}
						catch(Exception ex)
						{
							log.WriteLine(
								LogType.Error,"HI009: " + 
								String.Format("Failed during installing:{0}, Error:{1}",
								item.srcDir.ToString(),ex.ToString())
								);
						}
					}
					#endregion

					//-------------------------------------------------------
					// Fix/Update Xml configuration files according to specification
					// in <XmlFileFixes> for each <XmlFileFix>
					//-------------------------------------------------------
					Console.WriteLine("Applying changes in configuration files");
					
					#region Xml Configuration Fixing
					
					log.WriteLine(LogType.Info,"HI010: " + "[Xml configuration files update]");
					
					//make each xml fix as it appearing in XmlFileFixes section
					foreach(HotfixSettings.XmlFileFix filefix in settings.XmlFileFixes)
					{
						// find out which component we talk about to be able to find
						// configuration xml file for: filefix
						foreach(SrcDestPair comp in processed)
						{
							if(comp.srcDir.Name.ToLower() == filefix.folder.ToLower())
							{
								//path to fix describing changes for: filefix
								string fileOfFixes = Path.Combine( comp.srcDir.FullName,filefix.update );
									
								XmlConfigurationHotfix componentFix = XmlConfigurationHotfix.Load(fileOfFixes);
								
								//get related pathes of patched files
								if(componentFix.objXMLContentFixes.Length>0)
								{
									string[] pathes =new string[componentFix.objXMLContentFixes.Length];
									
									int index=0;
									foreach(XmlContentFix contentfix in componentFix.objXMLContentFixes)
									{
										pathes[index]=contentfix.contentXmlFilePath;
										index++;
									}
									
									foreach(DirectoryInfo path in comp.destDir)
									{
										
										//create absolute full qualified path to fixed xml
										foreach(XmlContentFix contentfix in componentFix.objXMLContentFixes)
										{
											contentfix.contentXmlFilePath = Path.Combine(path.FullName, contentfix.contentXmlFilePath);
										}

										try
										{
											//---> install fixes on Xml <---
											componentFix.Install(log);
										}
										catch(Exception ex)
										{
											log.WriteLine(
												LogType.Error, "HI011: " + 
												String.Format("Failed during Xml Configuration Fixing: {0}, Error:{1}",
												fileOfFixes, 
												ex.ToString())
												);
										}

										// restore related path before next destination folder
										index=0;
										foreach(XmlContentFix contentfix in componentFix.objXMLContentFixes)
										{
											contentfix.contentXmlFilePath = pathes[index];
											index++;
										}
									}
								}
								break;	
							}
						}//SrcDestPair
					}//filefix
					#endregion // Xml Configuration Fixing

					//-------------------------------------------------------
					// Clear Temporary ASP.NET files cache
					//-------------------------------------------------------
					Console.WriteLine("Clear Temporary ASP.NET files cache.");
					settings.mobjClearTempFiles.Do(log);

					//-------------------------------------------------------
					// Restore previous Status of controlled services
					//-------------------------------------------------------
					#region Restore controlled services Status
					
					log.WriteLine(LogType.Info,"HI012: " + "[RESTORE Status of controlled services ]");
					Console.WriteLine("Starting services.");
					
					if(objServiceProcessor.Restore())
					{
						log.WriteLine(LogType.Info,"HI013: " + "Status of controlled services restored successfully.");
					}
					else
					{

						//-------------------------------------------------------
						// Because KBXXXX of Microsoft not always ServiceController class able to start
						// service. I found workaround with starting some other process that tries
						// to start services again. This process takes as command line argument
						// of space separeted services names collected in error handler of
						// ServiceProcessor
						//-------------------------------------------------------

						System.Diagnostics.Process proc = new System.Diagnostics.Process();
						try
						{
							proc.EnableRaisingEvents=false;
							proc.StartInfo.FileName="Utilities.Hotfix.StartService.exe";
							proc.StartInfo.UseShellExecute=true;
							proc.StartInfo.Arguments=objServiceProcessor.strFailedList;
							log.WriteLine(LogType.Error, "HI052: " + objServiceProcessor.strFailedList);
							proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
							proc.Start();
						}
						catch
						{
						}
					}
					#endregion // restore controlled services Status

					//-------------------------------------------------------
					// Log that installation finished
					//-------------------------------------------------------
					string strMsgFinished = "Installation finished.";
					log.WriteLine(LogType.Info,"HI070: " + strMsgFinished);
					Console.WriteLine(strMsgFinished);
				}
				else
				{
					message = string.Format("Failed to stop controlled services. Can't continue.");
					log.WriteLine(LogType.Error, "HI015: " + message);
				}
			}
			catch(System.InvalidOperationException)
			{
				string message = string.Format("Failed to load hotfix instalation settings file: {0}{1} ",Environment.NewLine,this.SettingsFile);
				Console.WriteLine();
				log.WriteLine(LogType.Error, "HI016: " + message);
			}
			finally
			{
				if(InstallationCompletedEvent!=null)
					InstallationCompletedEvent(settings.strGeneralDescription, "Please check log file.");
				log.Show();
			}
		}

		/// <summary>
		/// Finding out destination full qualified path to installed component
		/// </summary>
		/// <param name="folder">DirectoryInfo object of installed component (folder)</param>
		/// <returns>Array of DirectoryInfo object of possible destinations for given 
		/// component. Array.Length > 1 if ?XXX? names will appear in
		/// path in DestinationRootFolder and more than one expanding found in 
		/// "Expand" section for specified ?XXX? name.
		/// </returns>
		protected virtual DirectoryInfo[] DiscoverDest(DirectoryInfo folder)
		{
			string			foldertype		= null;
			string[]		destrootpath	= null;
			string			destfoldername	= null;
			ArrayList		returned		= new ArrayList();

			// get specified folder type or default one
			foldertype = settings.GetFolderType(folder.Name);
			
			if(foldertype!=null)
			{
				// get possible destination folders list
				destrootpath = settings.GetDestinationFolder(foldertype);
				
				if(destrootpath != null && destrootpath.Length > 0)
				{
					// folder name is remapped or taken as folder name itself
					destfoldername = folder.Name;
					foreach(HotfixSettings.FolderMapping fm in settings.FolderMappings)
					{
						if(fm.folder.ToLower() == folder.Name.ToLower())
						{
							destfoldername=fm.real;
							break;
						}
					}
					// create detination path to copied folder (component)
					for(int index=0;index < destrootpath.Length; index++)
					{
						try
						{
							DirectoryInfo dest = new DirectoryInfo(
								Path.Combine(destrootpath[index],destfoldername));
							returned.Add(dest);
						}
						catch(Exception)
						{
							//Path.Combine failed means wrong path to destination folder
							throw new ArgumentException(
								String.Format(
									"Failed to determine the destination folder path for component: \"{0}\", path \"{1}\"",
									folder.Name, destrootpath[index])
								);
						}
					}
				}
				else
				{
					//Error:
					//	Undefined possible destinations for component and type.
					throw new ArgumentException(
						String.Format(
								"Undefined possible destinations for component: \"{0}\" and type: \"{1}\"",
								folder.Name, foldertype));
				}
			}
			else
			{
				//	One of Possible errors:
				//		- unknown component folder type
				//		- default component type is not set
				throw new ArgumentException("Unknown component folder type. Default component type is not set.");
			}
			
			return (DirectoryInfo[])returned.ToArray(typeof(DirectoryInfo));
		}


	}

	/// <summary>
	/// Value class contains:
	///		full qualified path to installed component in hofix root folder
	///		array of resolved destinations for installed component
	/// </summary>
	internal class SrcDestPair
	{
		public DirectoryInfo	srcDir;
		public DirectoryInfo[]	destDir;

		public SrcDestPair(DirectoryInfo srcDir, DirectoryInfo[] destDir)
		{
			this.destDir = destDir;
			this.srcDir = srcDir;
		}

		public override string ToString()
		{
			string info= srcDir.FullName;
			
			foreach(DirectoryInfo path in destDir)
			{
				info += "\n\t --> " + path.FullName;
			}
			return info;
		}

	}

}
