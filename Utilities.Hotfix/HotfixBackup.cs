using System;
using System.IO;
using System.Collections;

using Utilities.Hotfix.Xml;

namespace Utilities.Hotfix
{
	public class HotfixBackup
	{
		HotfixSettings	settings			= new HotfixSettings();
		Logger			log					= new Logger(Logger.GetNewLogFilename());
		ArrayList		processed			= new ArrayList();

		string		strSpecificBackup	= string.Empty;		//folder name of specific currently made backup
		string		strBackupFolder		= string.Empty;

		public HotfixBackup(HotfixSettings settings, Logger log, ArrayList	processed)
		{
			this.settings = settings;
			this.log = log;
			this.processed = processed;

			//----------------------------------------------------------------------------
			// folder of all backups
			//----------------------------------------------------------------------------
			if(settings.Backup.blnIsRelative)
			{
				strBackupFolder= Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
											  settings.Backup.strFolder);
			}
			else
			{
				strBackupFolder = settings.Backup.strFolder;
			}

		}

		public bool doBackup()
		{
			bool		blnIsSucceed		= true;

			//----------------------------------------------------------------------------
			// folder of all backups
			//----------------------------------------------------------------------------
			DirectoryInfo objBackups = new DirectoryInfo(strBackupFolder);
			
			//----------------------------------------------------------------------------
			// get/create folder of backups
			//----------------------------------------------------------------------------
			#region get/create folder of backups
			if(!objBackups.Exists)
			{
				try
				{
					objBackups.Create();
				}
				catch(Exception objException)
				{
					log.WriteLine(
						LogType.Error,
						string.Format("HI017: " + "Failed to create root folder of backups: {0}",
						objException.ToString())
						);
					return false;
				}
			}

			//----------------------------------------------------------------------------
			// folder name of specific currently made backup
			//----------------------------------------------------------------------------
			//create specific backup folder under the folder of backups
			strSpecificBackup = "Backup " + DateTime.Now.ToString("[dd-MM-yyyy HHmmss]");
			DirectoryInfo objBackupFolder = null;
			try
			{
				objBackupFolder = objBackups.CreateSubdirectory(strSpecificBackup);
			}
			catch(Exception objException)
			{
				log.WriteLine(
					LogType.Error, 
					string.Format("HI018: " + "Failed to create folder for specific backup: " + strSpecificBackup + ", Exception: {0}", objException.ToString())
					);
				return false;
			}

			#endregion // get/create folder of backups
			
			//----------------------------------------------------------------------------
			// backup each installed component
			//----------------------------------------------------------------------------
			#region backup each installed component
			
			foreach(SrcDestPair objSrcDest in processed)
			{
				//----------------------------------------------------------
				// Skip backup of component if marked in ComponentFiltes section of BackupExclusions
				//----------------------------------------------------------
				if(this.settings.IsExcludedFromBackup(objSrcDest.srcDir.Name))
				{
					log.WriteLine(LogType.Info,"HI069: " + 
						String.Format(@"SKIPPED backuping of the component: ""{0}""", objSrcDest.srcDir.Name)
						);
					continue;
				}
				
				// The path to currently backuped component
				string strBackuped = string.Empty;

				try
				{
					foreach(DirectoryInfo destPath in objSrcDest.destDir)
					{
						strBackuped = destPath.FullName;

						if(destPath.Exists)
						{
							log.WriteLine(LogType.Info,"HI019: " + 
								String.Format(@"BACKUPING Component: ""{0}""", strBackuped)
								);
							
							//create folder of backup for specific component
							DirectoryInfo objComponentBackup = objBackupFolder.CreateSubdirectory(objSrcDest.srcDir.Name);
							
							// Add masks for excluded files
							xDirectory.arrExcludeFileFilters = settings.mobjBackupExclusions.strGlobalFileExclusions;
							
							xDirectory.Copy(destPath, objComponentBackup);
							
							// !!! Only first installed component will be backuped to prevent overwriting
							break;
						}
					}

					#region Check and log possible multiple backup pathes
					
					if(objSrcDest.destDir.Length > 1)
					{
						int		mintAmountExists	= 0;
						string	mstrBackuped		= String.Empty;

						foreach(DirectoryInfo destPath in objSrcDest.destDir)
						{
							if(destPath.Exists)
							{
								mintAmountExists++;
								mstrBackuped = destPath.FullName;
							}
						}
						
						if(mintAmountExists > 1)
						{
							// Installed component will be copied to more than one destination
							// the only one destination(first) will be backuped
							log.WriteLine(LogType.Warning,"HI045: " + 
								String.Format(@"Multiple destinations found for component: ""{0}"" ", objSrcDest.srcDir.ToString()) + 
								String.Format("Only one component BACKUPED: {0}", mstrBackuped));
						}
					}
					#endregion // Check and log possible multiple backup pathes

				}
				catch(Exception ex)
				{
					blnIsSucceed=false;
					#region Log: Failed during backup
					log.WriteLine(
						LogType.Error, "HI020: " + 
						String.Format("Failed during backup: {0}, Error:{1}",
						strBackuped, ex.ToString())
						);
					#endregion // Log: Failed during backup
					break;
				}
			}
			
			#endregion // backup each installed component

			//----------------------------------------------------------------------------
			// Prepare BIN folder of hotfix in Backup
			//----------------------------------------------------------------------------
			#region  Prepare BIN folder of hotfix in Backup
			//----------------------------------------------------------------------------
			// 1) prepare backup bin folder with all needed exe and dll files
			//----------------------------------------------------------------------------
			DirectoryInfo objFolderInstaller = objBackupFolder.CreateSubdirectory("Setup");

			string[] arrFileToCopy = new string[]{
													 "Utilities.Hotfix.dll",
													 "Utilities.Hotfix.Config.exe",
													 "Utilities.Hotfix.StartService.exe"
												 };
			// see also AppDomain Class usage example
			// http://207.46.16.251/en-us/library/system.appdomain.aspx
			
			FileInfo objExeInstaller = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
			objExeInstaller.CopyTo(Path.Combine(objFolderInstaller.FullName,objExeInstaller.Name),true);
			
			foreach(string strFileToCopy in arrFileToCopy)
			{
				FileInfo objDllInstaller = new FileInfo(strFileToCopy);
				objDllInstaller.CopyTo(Path.Combine(objFolderInstaller.FullName,objDllInstaller.Name),true);
			}

			//----------------------------------------------------------------------------
			// Create and save UNDO configuration file
			//----------------------------------------------------------------------------

			HotfixSettings objBackupSettings = new HotfixSettings();
			objBackupSettings = HotfixSettings.load(settings.SettingsFileName);
			objBackupSettings.strGeneralDescription = " ROOLBACK: " + objBackupSettings.strGeneralDescription;
			
			//----------------------------------------------------------------------------
			// Backup XML configuration files and update UNDO settings configuration
			//----------------------------------------------------------------------------
			#region Backup XML configuration files and update UNDO settings configuration

			HotfixSettings.DestinationRootFolder objXmlConfig = new HotfixSettings.DestinationRootFolder();
			
			objXmlConfig.TypeName	= "Xml.Config.Backup";
			objXmlConfig.path		= "";
			objXmlConfig.Default	= false;

			objBackupSettings.DestinationRootFolders = (HotfixSettings.DestinationRootFolder[])
													AppendItem(objBackupSettings.DestinationRootFolders,
															   objXmlConfig,
															   typeof(HotfixSettings.DestinationRootFolder));

			foreach(HotfixSettings.XmlFileFix filefix in settings.XmlFileFixes)
			{
				// find out which component we talk about to be able to find
				// configuration xml file for: filefix
				foreach(SrcDestPair comp in processed)
				{
					if(comp.srcDir.Name.ToLower() == filefix.folder.ToLower())
					{
						//path to fix describing changes for: filefix
						string fileOfFixes = Path.Combine( comp.srcDir.FullName, filefix.update );
						
						XmlConfigurationHotfix componentFix = XmlConfigurationHotfix.Load(fileOfFixes);
						
						//get related pathes of patched files
						if(componentFix.objXMLContentFixes.Length > 0)
						{
							foreach(DirectoryInfo path in comp.destDir)
							{
								//create absolute full qualified path to fixed xml
								foreach(XmlContentFix contentfix in componentFix.objXMLContentFixes)
								{
									string strFixedXmlFile = Path.Combine(path.FullName, contentfix.contentXmlFilePath);
									FileInfo objFixedXmlFile = new FileInfo(strFixedXmlFile);

									if(objFixedXmlFile.Exists)
									{
										HotfixSettings.FolderType objFolderType = new HotfixSettings.FolderType();
										objFolderType.TypeName = objXmlConfig.TypeName;
										objFolderType.hotfix = "Xml.Config." + comp.srcDir.Name;
										objBackupSettings.FolderTypes = (HotfixSettings.FolderType[])
																	AppendItem(objBackupSettings.FolderTypes,
																			objFolderType,
																			typeof(HotfixSettings.FolderType));

										HotfixSettings.FolderMapping objFolderMapping = new HotfixSettings.FolderMapping();
										objFolderMapping.folder = objFolderType.hotfix;
										// ms-help://MS.MSDNQTR.2006JAN.1033/cpref/html/frlrfSystemIOPathClassGetDirectoryNameTopic.htm
										objFolderMapping.real = Path.GetDirectoryName(strFixedXmlFile);
										objBackupSettings.FolderMappings = (HotfixSettings.FolderMapping[])
																AppendItem(objBackupSettings.FolderMappings,
																		objFolderMapping,
																		typeof(HotfixSettings.FolderMapping));

										objBackupFolder.CreateSubdirectory(objFolderType.hotfix);
										
										//Copy file to backup directory
										objFixedXmlFile.CopyTo(
											Path.Combine(Path.Combine(
												objBackupFolder.FullName,
												objFolderType.hotfix),
												objFixedXmlFile.Name));
									}

									//only first configuration file of processed component backuped
									break;
								}
							}
						}
					}
				}

			}
			#endregion // Backup XML configuration files and update UNDO settings configuration

			//Disable hotfix "backup" for "backup"
			objBackupSettings.Backup.blnIsDoBackup = false;

			//Zeroes configuration for exlclusions of backup
			objBackupSettings.mobjBackupExclusions.arrComponentFilters		= new string[]{};
			objBackupSettings.mobjBackupExclusions.strGlobalFileExclusions	= new string[]{};
			
			// Set components folder exactly to just created folder with backuped components
			objBackupSettings.ComponentsFolderSetting.blnIsRelative=false;
			objBackupSettings.ComponentsFolderSetting.strFolder = objBackupFolder.FullName;

			//xml fixes unrelevant for restoration from backup
			objBackupSettings.XmlFileFixes = new HotfixSettings.XmlFileFix[]{};
			objBackupSettings.save(Path.Combine(objFolderInstaller.FullName, HotfixSettings.SETTINGS_FILENAME));
			
			#endregion

			return blnIsSucceed;
		}


		/// <summary>
		/// Calculates amount of free space required to backup
		/// Only installed components taken in account.
		/// Filter of excluded files taken  in account.
		/// </summary>
		/// <returns>Amount of free space required in Bytes</returns>
		public long CalculateRequiredFreeSpace()
		{
			long lngCalculated= 0;
			
			foreach(SrcDestPair objSrcDest in processed)
			{
				try
				{
					foreach(DirectoryInfo destPath in objSrcDest.destDir)
					{
						if(destPath.Exists)
						{
							ArrayList arrFilesList		= new ArrayList();
							ArrayList arrFoldersList	= new ArrayList();
							
							// Add masks for excluded files
							xDirectory.arrExcludeFileFilters = settings.mobjBackupExclusions.strGlobalFileExclusions;
							xDirectory.Investigate(destPath, ref arrFilesList, ref arrFoldersList);
							
							foreach(FileInfo objCopiedFile in arrFilesList)
							{
								lngCalculated += objCopiedFile.Length;
							}
							
							// !!! Only first installed component will be backuped to prevent overwriting
							break;
						}
					}
				}
				catch(Exception objException)
				{
					#region Log: Failed during calculating required free space
					log.WriteLine(
						LogType.Error, "HI053: " + 
						String.Format("Failed during calculating required free space. Error:{0}",
						objException.ToString())
						);
					#endregion // Failed during calculating required free space
				}
			}

			return lngCalculated;
		}


		/// <summary>
		/// Calculates availbale free space on disk that backup will be created
		/// </summary>
		/// <returns>Amount of free space in bytes</returns>
		public long CalclateAvailable()
		{
			long lngAvailable = 0;

			DirectoryInfo objRootFolderBackups = new DirectoryInfo(strBackupFolder);
			try
			{
				DriveInfoSystem info = DriveInfo.GetInfo(objRootFolderBackups.Root.ToString());
				lngAvailable = info.Available;
			}
			catch(Exception objException)
			{
				#region Log: Failed during calculating available free space
				log.WriteLine(
					LogType.Error, "HI054: " + 
					String.Format("Failed during calculating available free space. Error:{0}",
					objException.ToString())
					);
				#endregion // Failed during calculating required free space
			}
			return lngAvailable;
		}


		/// <summary>
		/// Append item to the end of given array.
		/// Not effective - reallocation happends.
		/// </summary>
		/// <param name="arrExtended">Appended array</param>
		/// <param name="objAppended">Appended object</param>
		/// <param name="objTypeOf">Type of appended item</param>
		/// <returns>Newly allocated array with appended item</returns>
		private System.Array AppendItem(System.Array arrExtended, Object objAppended, System.Type objTypeOf)
		{
			int intNewLength = arrExtended.Length+1;
			System.Array arrReturned = Array.CreateInstance(objTypeOf,intNewLength);
			arrExtended.CopyTo(arrReturned,0);
			arrReturned.SetValue(objAppended,arrExtended.Length);
			return arrReturned;
		}

	}
}
