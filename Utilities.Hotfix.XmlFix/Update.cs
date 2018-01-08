using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Utilities.Hotfix.Xml;

namespace Utilities.Hotfix.XmlFix
{
	public class Update
	{
		private static Logger log = new Logger(Logger.GetNewLogFilename());

		public class FileRef : IEquatable<FileRef>
		{
			public string ConfigFile = string.Empty;
			
			public FileRef(string strConfigFile)
			{
				ConfigFile = strConfigFile;
			}			
			
			public override string ToString()
			{
				return ConfigFile;
			}

			public bool Equals(FileRef other)
			{
				return (this.ConfigFile.ToLower() == other.ConfigFile.ToLower());
			}
		}

		public List<FileRef> mobjTargetFiles = new List<FileRef>();
		OpenFileDialog mobjOpenFileDialog = null;

		public Update(OpenFileDialog objOpenDialog)
		{
			mobjOpenFileDialog = objOpenDialog;
		}

		public bool Add()
		{
			bool blnReturn = false;
			// add to list if not exist
			mobjOpenFileDialog.FileName = string.Empty;
			if (mobjOpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				FileRef added = new FileRef(mobjOpenFileDialog.FileName);
				if (mobjTargetFiles.IndexOf(added) == -1)
				{
					mobjTargetFiles.Add(added);
					blnReturn = true;
				}
			}
			return blnReturn;
		}

		public bool Remove(FileRef objFileRef)
		{
			bool blnReturn = false;
			// delete from list if exist
			int index = mobjTargetFiles.IndexOf(objFileRef);
			if (index > -1)
			{
				mobjTargetFiles.RemoveAt(index);
			}
			return blnReturn;
		}

		public bool UpdateTarget()
		{
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

			foreach (FileRef objFileRef in mobjTargetFiles)
			{
				xmlFixes[0].contentXmlFilePath = objFileRef.ConfigFile;
				update.Install(log);
			}

			log.Show();

			return true;
		}

		public bool RollBack()
		{
			XmlConfigurationHotfix update = new XmlConfigurationHotfix();
			update.objXMLContentFixes = new XmlContentFix[1];
			XmlContentFix[] xmlFixes = update.objXMLContentFixes;

			xmlFixes[0] = new XmlContentFix();
			xmlFixes[0].description = "Delete openModal attribute of View nodes";
			xmlFixes[0].objContentFixAttribute =
				new ContentFixAttribute(
					null,
					null,
					new Utilities.Hotfix.Xml.Attribute[]
					{
						new Utilities.Hotfix.Xml.Attribute(
						@"/configuration/uipConfiguration/views/view",
						"openModal")
					},
					null);

			foreach (FileRef objFileRef in mobjTargetFiles)
			{
				xmlFixes[0].contentXmlFilePath = objFileRef.ConfigFile;
				update.Install(log);
			}

			log.Show();

			return true;
		}
	}
}
