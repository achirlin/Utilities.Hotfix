using System;
using System.Xml;
using System.Xml.Serialization;

namespace Utilities.Hotfix.Xml
{
	/// <summary>
	/// Value object describes updates of single xml file.
	/// </summary>
	[Serializable()]
	public class XmlContentFix
	{
		#region properties
		// general description of specific content fix
		[XmlAttribute()]
		public string				description			="";

		// path to xml file fixed
		[XmlAttribute()]
		public string				contentXmlFilePath	="";
		
		[XmlElement(ElementName="ContentFixNodes")]
		public ContentFixNode		objContentFixNode		= new ContentFixNode();

		[XmlElement(ElementName="ContentFixAttributes")]
		public ContentFixAttribute	objContentFixAttribute	= new ContentFixAttribute();

		#endregion
		
		#region Ctor
		public XmlContentFix()
		{
		}

		/// <summary>
		/// Constructs object
		/// </summary>
		/// <param name="_description">Description of changes to file</param>
		/// <param name="_contentXmlFilePath">full qulified path to fixed file</param>
		/// <param name="_objContentFixNode">object describing fixes of XmlNodes</param>
		/// <param name="_objContentFixAttribute">object describing fixes of XmlAttribues </param>
		public XmlContentFix(	string _description,
								string _contentXmlFilePath,
								ContentFixNode _objContentFixNode,
								ContentFixAttribute _objContentFixAttribute
							)
		{
			description				= _description;
			contentXmlFilePath		= _contentXmlFilePath;
			objContentFixNode		= _objContentFixNode;
			objContentFixAttribute	= _objContentFixAttribute;
		}
		#endregion
	}
}
