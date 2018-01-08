using System;
using NUnit.Framework;
using System.Xml;
using Attribute = Utilities.Hotfix.Xml.Attribute;
using Utilities.Hotfix.Xml;
using System.IO;
using System.Xml.XPath;

namespace Utilities.Hotfix.UNITests
{
	public class XmlAttributesChanges : UnitTest
	{
		static protected string strTestedXmlFile = "TestXmlAttributes.xml";
		static Logger log = new Logger();
		static protected XmlDocument objXmlTested = null;

		public XmlAttributesChanges():base()
		{
		}
		
		[TestFixtureSetUp()]
		public override void Init()
		{
			// Delete result of previous update
			// To be sure that updated xml file REALLY created again
			if(File.Exists(strTestedXmlFile))
				File.Delete(strTestedXmlFile);

			// Drop log file
			if(File.Exists(log.LogFileName))
				File.Delete(log.LogFileName);
			
			// Extract and load XML file that changes/tests applied to
			GetResourceToFile(strTestedXmlFile);
			objXmlTested = new XmlDocument();
			objXmlTested.Load(strTestedXmlFile);

			Console.WriteLine("XmlAttributesChanges test class constructed.");
		}

		
		[TestFixtureTearDown]
		public override void Dispose()
		{
			Console.WriteLine("XmlAttributesChanges test class disposed.");
		}

		[Test, Category("Xml attributes manipulation")]
		public void AttributeUpdate()
		{
			// /Root/A/@updateme attribute exists so update should success
			Attribute objUpdatedExist = new Attribute("/Root/A", "updateme", "newvalue");
			
			// /Root/A/@updateme attribute dosn't exist so update should fail
			Attribute objUpdatedNotExist = new Attribute("/Root/A", "newattribute", "newvalue");
			Attribute[] arrUpdatedAttributes = new Attribute[] { objUpdatedExist, objUpdatedNotExist };
			
			XmlContentFix objXmlFix = new XmlContentFix();
			objXmlFix.contentXmlFilePath = strTestedXmlFile;
			objXmlFix.description = "Update existed and not existed attributes";
			objXmlFix.objContentFixAttribute = new ContentFixAttribute();
			objXmlFix.objContentFixAttribute.UpdateAttributes = arrUpdatedAttributes;
			
			XmlContentFix[] objXmlFixes = new XmlContentFix[] { objXmlFix };
			new XmlConfigurationHotfix(1f, "AttributeAddUpdate", objXmlFixes).Install(log);
			

			// Self checking procedure of updated node
			XmlDocument objChecked = new XmlDocument();
			objChecked.Load(strTestedXmlFile);
			XmlNodeList objList  = objChecked.SelectNodes("/Root/A[@updateme='newvalue']");
			if(objList.Item(0).Attributes["updateme"].Value == "newvalue")
			{
				Console.WriteLine("SUCCESS of updating: /Root/A[@updateme='newvalue']");
			}
			else
				new Exception("Failed to update attribute: /Root/A[@updateme='newvalue']");

			log.Show();
		}
	}
}
