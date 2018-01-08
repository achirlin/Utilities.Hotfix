using System;
using System.Xml;
using System.Xml.Serialization;

namespace Utilities.Hotfix.Xml
{
	/// <summary>
	/// Dsecribes change in specific attribute pointed by XPath expression
	/// </summary>
	[Serializable()]
	public class Attribute
	{
		#region properties
		
		/// <summary>
		/// string description of taken action on attribute
		/// </summary>
		[XmlAttribute()]
		public string description;
		
		/// <summary>
		/// Xpath expression pointing to changed attribute
		/// </summary>
		[XmlAttribute()]
		public string XPath;
		
		/// <summary>
		/// name of added/updated/deleted attribute
		/// </summary>
		[XmlAttribute()]
		public string attribute_name = String.Empty; // Because may be omitted in XML file, to prevent null value

		/// <summary>
		/// value of added/updated attribute
		/// </summary>
		[XmlAttribute()]
		public string attribute_value;
		
		/// <summary>
		/// namespace of added attribute
		/// </summary>
		[XmlAttribute("namespace")]
		public string strNamespaceURI = String.Empty;
		
		/// <summary>
		/// Xpath expression to Node that it's attribute with name: <see cref="strSourceAttrName"/> 
		/// will be copied
		/// </summary>
		[XmlAttribute("sourcepath")]
		public string strSourceXPath;
		
		/// <summary>
		///Copied Attribute name of node pointed by <see cref="strSourceXPath"/>
		/// </summary>
		[XmlAttribute("sourcename")]
		public string strSourceAttrName;

		/// <summary>
		/// XPath expression that if differs from string.empty will be evaluated
		/// on fixed xml document, and if nothing found as result of evaluation
		/// then attribute will be copied
		/// </summary>
		[XmlAttribute("copycondition")]
		public string strCopyIfNotFound = String.Empty;

		#endregion Properties
		
		#region Ctors

		public Attribute()
		{
		}

		public Attribute(string XPath, string attribute_name):this(XPath, attribute_name, String.Empty)
		{
		}

		/// <param name="_XPath"> _XPath - W3W XPath string to XML element(node) that attribute processed </param>
		/// <param name="_attribute_name"> the name of attribute to be processed </param>
		/// <param name="_attribute_value">the new value of attribute </param>
		public Attribute(string XPath, string attribute_name, string attribute_value):
			this(XPath,attribute_name,attribute_value,string.Empty)
		{
		}

		/// <param name="_XPath"> _XPath - W3W XPath string to XML element(node) that attribute processed </param>
		/// <param name="_attribute_name"> the name of attribute to be processed </param>
		/// <param name="_attribute_value">the new value of attribute </param>
		/// <param name="strNamespaceURI">specific namespace of given attribute</param>
		public Attribute(string XPath, string attribute_name, string attribute_value, string strNamespaceURI)
		{
			this.XPath = XPath;
			this.attribute_name = attribute_name;
			this.attribute_value = attribute_value;
			this.strNamespaceURI = strNamespaceURI;
		}
		
		#endregion Ctors
	}

}
