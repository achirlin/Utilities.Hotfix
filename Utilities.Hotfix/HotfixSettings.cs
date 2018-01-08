using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace Utilities.Hotfix
{	
	[Serializable()]
	[XmlRoot("HotfixSettings")]
	public class HotfixSettings
	{
		[XmlAttribute("description")]
		public string strGeneralDescription = String.Empty;

		/// <summary>
		/// Object describing controlled service
		/// </summary>
		[Serializable()]
		public class Service
		{
			[XmlAttribute("service_name")]
			public string SERVICE_NAME;
			
			/// <summary>
			/// Set position of service in list of started services
			/// </summary>
			[XmlAttribute("startorder")]
			public int	intStartOrder;
			
			/// <summary>
			/// Set position of service in list of stopped services
			/// </summary>
			[XmlAttribute("stoporder")]
			public int	intStopOrder;

			public Service()
			{
			}

			public Service(string strServiceName, int intStartOrder, int intStopOrder)
			{
				this.SERVICE_NAME	= strServiceName;
				this.intStartOrder	= intStartOrder;
				this.intStopOrder	= intStopOrder;
			}
			
			public override string ToString()
			{
				return SERVICE_NAME;
			}

		}


		/// <summary>
		/// DestinationRootFolder describes folder contains some of installed components 
		/// To be able to reference this destination folder during installation
		/// of specific component each destination folder tagged with TypeName
		/// property. Path to specific file system folder built from ?XXX? keyword expandings
		/// taken from <see cref="ExpandKeyword"/> collection. Component referenced in
		/// <see cref="FolderType"/> will be installed to this destination folder, but
		/// component that found and not referenced will be installed to destination folder that
		/// has Default=true value.
		/// </summary>
		[Serializable()]
		public class DestinationRootFolder
		{
			[XmlAttribute(AttributeName = "type_name")]
			public string TypeName;
			
			[XmlAttribute()]
			public string path;
			
			[XmlAttribute("default")]
			public bool Default;

		}


		[Serializable()]
		public class FolderType
		{
			[XmlAttribute(AttributeName = "type_name")]
			public string TypeName;
			[XmlAttribute("folder_hotfix")]
			public string hotfix;
		}


		[Serializable()]
		public class FolderMapping
		{
			[XmlAttribute("folder_hotfix")]
			public string folder;
			[XmlAttribute("folder_real")]
			public string real;
		}


		[Serializable()]
		public class ExcludeFolder
		{
			[XmlAttribute("folder_hotfix")]
			public string folder;
		}

		
		[Serializable()]
		public class ExpandKeyword
		{
			[XmlAttribute()]
			public string name;
			[XmlArray()]
			public string[] Value;
		}

		
		[Serializable()]
		public class XmlFileFix
		{
			[XmlAttribute("folder_hotfix")]
			public string folder;
			[XmlAttribute()]
			public string update;
		}


		[Serializable()]
		public class BackupInstruction
		{
			[XmlTextAttribute()]
			public string strFolder			= @"Backups";
			
			[XmlAttribute(AttributeName ="doBackup")]
			public bool blnIsDoBackup		= true;

			/// <summary>
			/// True if path given in <see cref="strFolder"/> is relative to
			/// current domain base direcotry, else is full qualified path to
			/// backup folder
			/// </summary>
			[XmlAttribute("relative")]
			public bool blnIsRelative		= true;
		}


		[Serializable()]
		public class BackupExclusions
		{
			/// <summary>
			/// Components' names excluded from backup
			/// </summary>
			[XmlArray("ComponentFilters")]
			public string[] arrComponentFilters = new string[]{};			

			/// <summary>
			/// Wildcard definitions of files excluded during backup procedure
			/// </summary>
			[XmlArray("FileFilters")]
			public string[] strGlobalFileExclusions = new string[]{};
		}


		[Serializable()]
		public class ClearTemporaryAspNetFiles
		{
			[XmlAttribute("doClear")]
			public bool		blnIsDoClear;
			
			/// <summary>
			/// 
			/// </summary>
			[XmlArray("versions")]
			public string[]	marrVersion = new string[]{};
			
			public bool Do()
			{
				return Do(null);
			}
			
			public bool Do(Logger log)
			{
				const string CONSTTEMPDIR = "Temporary ASP.NET Files";

				if(blnIsDoClear)
				{
					#region STARTING
					if(marrVersion.Length>0)
					{
						if(log!=null)
						{
							log.WriteLine(LogType.Info,"HI042: " + 
								"[Cleaning of temporary ASP.NET files]"
							);
						}
					}
					#endregion // STARTING

					foreach(string strSpecific in marrVersion)
					{
						string strTempDir  = String.Empty;

						try
						{
							strTempDir = 
								Path.Combine(Path.Combine(
								@System.Environment.ExpandEnvironmentVariables("%WINDIR%"), 
								@"Microsoft.NET"),
								string.Format(@"Framework\{0}\{1}", strSpecific, CONSTTEMPDIR)
								);
			
							DirectoryInfo objDir = new DirectoryInfo(strTempDir);
			
							// Recursively delete all files from folder
							if(objDir.Exists)
							{
								objDir.Delete(true);
							
								// ReCreate root folder of temporary files
								if(!objDir.Exists)
								{
									objDir.Parent.CreateSubdirectory(CONSTTEMPDIR);
								}
								#region CLEANED
								if(marrVersion.Length>0)
								{
									if(log!=null)
									{
										log.WriteLine(LogType.Info,"HI043: " + 
											string.Format("[Cleaned temporary ASP.NET files] {0}",strTempDir)
											);
									}
								}
								#endregion //CLEANED
							}
								
						}
						catch(Exception objException)
						{
							#region Log error
							if(log!=null)
							{
								log.WriteLine(LogType.Error,"HI041: " + 
									string.Format("Error during cleaning of temporary files in {0}, {1}",
									strTempDir, 
									objException.ToString()));
							}
							#endregion // Log error
						}
					}//for
				}//if
				return true;
			}//Do

		}//ClearTemporaryAspNetFiles


		private	ComponentsFolder		mobjComponentsFolder;
		private	BackupInstruction		mobjBackup;
		public	Service[]				Services;					
		public	DestinationRootFolder[]	DestinationRootFolders;		
		public	FolderType[]			FolderTypes;				
		public	FolderMapping[]			FolderMappings;				
		public	ExcludeFolder[]			ExcludeFolders;
		public	XmlFileFix[]			XmlFileFixes		= new XmlFileFix[]{};
		public const string				SETTINGS_FILENAME	= "hotfix.xml";
		public const string				QUOTE				= "?";
		public ExpandKeyword[]			ExpandKeywords;
		[XmlElement("BackupExclusions")]
		public BackupExclusions			mobjBackupExclusions = new BackupExclusions();
		[XmlElement("ClearTempFiles")]
		public ClearTemporaryAspNetFiles mobjClearTempFiles = new ClearTemporaryAspNetFiles();
		
		private string					mstrSettingsFileName;

		/// <summary>
		/// File name of loaded settings file
		/// </summary>
		public string SettingsFileName
		{
			get
			{
				return mstrSettingsFileName;
			}
		}

		public HotfixSettings()
		{
			#region fill default controlled services' names in SPECIFIC order !!!
			
			Services = new Service[]{
				//World Wide Web publishing started/stopped first of all
				//         (SERVICE_NAME,			  Start Order, Stop Order)
				new Service("w3svc",							1,1)
			};
			
			// Exlude logging files from backup
			mobjBackupExclusions.strGlobalFileExclusions=new string[]{"*.log"};
			
			// Clear temporary asp.net files from cache
			mobjClearTempFiles.blnIsDoClear = true;
			mobjClearTempFiles.marrVersion = new string[] { "v1.1.4322" };
			
			#endregion
		}

		
		/// <summary>
		/// Relative to path of settings file to folder of installed components.
		/// Default path points to parent folder of "folder of exe of hotfix installer".
		/// </summary>
		public class ComponentsFolder
		{
			/// <summary>
			/// Path to folder with installed components
			/// </summary>
			[XmlTextAttribute()]
			public string strFolder			= @"..\";
			
			/// <summary>
			/// True if path given in <see cref="strFolder"/> is relative to
			/// current domain base direcotry, else <see cref="strFolder"/> is full 
			/// qualified path to folder contains installed components
			/// </summary>
			[XmlAttribute("relative")]
			public bool blnIsRelative		= true;
		}


		/// <summary>
		/// Relative to path of settings file to folder of backups.
		/// Default path points to "folder of exe of hotfix installer"/Backups.
		/// </summary>
		[XmlElement("BackupsFolder")]
		public BackupInstruction Backup
		{
			get
			{
				return this.mobjBackup;
			}
			set
			{
				this.mobjBackup = value;
			}
		}


		[XmlElement("ComponentsFolder")]
		public ComponentsFolder ComponentsFolderSetting
		{
			get
			{
				return mobjComponentsFolder;
			}
			set
			{
				this.mobjComponentsFolder = value;
			}
		}


		/// <summary>
		/// Check if folder name is in list of excluded folders
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		public bool IsExcluded(DirectoryInfo folder)
		{
			foreach(ExcludeFolder f in ExcludeFolders)
			{
				if(f.folder.ToLower() == folder.Name.ToLower())
					return true;
			}
			return false;
		}

		/// <summary>
		/// Check if given name is in list of names of components excluded from backup
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>		
		public bool IsExcludedFromBackup(string strComponentName)
		{
			foreach(string strName in mobjBackupExclusions.arrComponentFilters)
			{
				if(strName.ToLower() == strComponentName.ToLower())
					return true;
			}
			return false;
		}

		/// <summary>
		/// Determine folder TypeName according to folder name, or
		/// get default TypeName
		/// </summary>
		/// <param name="folderName"></param>
		/// <returns></returns>
		public string GetFolderType(string folderName)
		{
			// get type of component according to folder name
			string TypeName=null;
			foreach(FolderType ft in FolderTypes)
			{
				if(ft.hotfix.ToLower() == folderName.ToLower())
				{
					TypeName = ft.TypeName;
					break;
				}
			}

			// if folder is not specified then determine default type
			if(TypeName == null)
			{
				foreach(DestinationRootFolder drFolder in DestinationRootFolders)
				{
					if(drFolder.Default)
					{
						TypeName = drFolder.TypeName;
						break;
					}
				}
			}
			return TypeName;
		}


		/// <summary>
		/// Creates array of posible destination pathes according to given
		/// folder type name and possible expandings.
		/// Warnign:
		///		The only one expand possible in folder path
		/// </summary>
		/// <param name="TypeName"></param>
		/// <returns></returns>
		public string[] GetDestinationFolder(string TypeName)
		{
			ArrayList destpathes=new ArrayList();

			foreach(DestinationRootFolder drFolder in DestinationRootFolders)
			{
				if(drFolder.TypeName == TypeName)
				{
					if(drFolder.path.IndexOf(QUOTE)> -1)
					{
						foreach(ExpandKeyword test in ExpandKeywords)
						{
							// TODO: Use RegEx: "\?([A-Za-z]+)\?"
							string searched = string.Format("{1}{0}{1}", test.name, QUOTE); // like ?HDD?
							int index = drFolder.path.IndexOf(searched);
							if(index > -1)
							{
								foreach(string Value in test.Value)
								{
									destpathes.Add(drFolder.path.Replace(searched,Value));
								}
							}
						}
					}
					else
					{
						destpathes.Add(drFolder.path);
					}
					break;
				}
			}
			return (string[])destpathes.ToArray( typeof(string) );
		}


		/// <summary>
		/// Serialize settings to file
		/// </summary>
		/// <param name="xmlFilePath">full qualified path to file</param>
		public void save(string xmlFilePath)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(HotfixSettings)); 
				TextWriter tw = new StreamWriter(xmlFilePath); 
				serializer.Serialize(tw,this); 
				tw.Close();
				this.mstrSettingsFileName = xmlFilePath;
			}
			catch(Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
			}
		}


		/// <summary>
		/// Deserialize settings from file
		/// </summary>
		/// <param name="xmlFilePath"> full qualified path to file</param>
		/// <returns></returns>
		public static HotfixSettings load(string xmlFilePath)
		{
			//deserialize from file
			XmlSerializer serializer = new XmlSerializer(typeof(HotfixSettings)); 
			TextReader tr = new StreamReader(xmlFilePath); 
			HotfixSettings inst = serializer.Deserialize(tr) as HotfixSettings;
			inst.mstrSettingsFileName = xmlFilePath;
			tr.Close();
			return inst;
		}

	} //class
}//namespace
