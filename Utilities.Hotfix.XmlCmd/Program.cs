using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Utilities.Hotfix.Xml;

namespace Utilities.Hotfix.XmlCmd
{
	public class XmlCmd
	{
		static int Main(string[] args)
		{
			if (args.Length == 0)
			{
				Usage();
			}
			else if (args.Length > 2)
			{
				Console.WriteLine("ERROR: Unexpected amount of arguments.");
				Usage();
			}
			else if(args.Length == 2)
			{
				string target = args[0];
				string patchPath = args[1];

				if(!File.Exists(target))
				{
					Console.WriteLine("ERROR: Target file not found.");
					Usage();
					return 1;
				}

				if(!File.Exists(patchPath))
				{
					Console.WriteLine("ERROR: Patch file not found.");
					Usage();
					return 1;
				}

				try
				{
					// Target and Patch files found,
					// apply target path to all defindex content fix in the patch file
					XmlConfigurationHotfix patchFix = XmlConfigurationHotfix.Load(patchPath);
					foreach (var XmlContentFix in patchFix.objXMLContentFixes)
					{
						XmlContentFix.contentXmlFilePath = target;
					}

					// Run the install of the patch with a logger instance
					Logger log = new Logger();
					patchFix.Install(log);
				}
				catch (Exception ex)
				{
					Console.WriteLine("ERROR: Unexpected behavior: {0}", ex.Message);
					return -1;
				}
			}

			return 0;
		}

		public static void Usage()
		{
			string usage = 
@"    
	Command line tool to update target.xml file with instructions from patch.xml file.

	Syntax:
			Utilities.Hotfix.XmlCmd.exe target.xml patch.xml

			target.xml - patched file
			patch.xml  - XML file with processing instructions

	Author:
			A.Chirlin, 2007
			achirlin@gmail.com
";
			Console.WriteLine(usage);

		}

	}
}
