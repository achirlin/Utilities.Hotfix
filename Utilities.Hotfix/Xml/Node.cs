using System;
using System.Xml;
using System.Xml.Serialization;

namespace Utilities.Hotfix.Xml
{
	[Serializable()]
	public class Node
	{
		#region properties

		[XmlAttribute()]
		public string description;

		/// <summary>
		/// XPath expression to PARENT node that this node will be PREPENDed/APPENDed
		/// </summary>
		[XmlAttribute()]
		public string XPathParent;

		/// <summary>
		/// XPath expression to node to be able to check existance during PREPEND/APPEND
		/// XPath expression to node that will be DELETED
		/// </summary>
		[XmlAttribute()]
		public string XPath;
		
		/// <summary>
		/// Inner xml of added/updated nodes 
		/// </summary>
		[XmlAttribute()]
		public string xmlstring;

		#endregion

		#region Ctors
		
		public Node()
		{
		}

		/// <summary>
		/// Create node suitable to describe APPENDED/PREPENDED XmlNode
		/// </summary>
		/// <param name="XPath"></param>
		public Node(string XPath, string XPathParent, string xmlString):this(XPathParent, xmlString, String.Empty, XPath)
		{
		}

		/// <summary>
		/// Create node suitable to describe DELETED XmlNode
		/// </summary>
		/// <param name="XPath"></param>
		public Node(string XPath, string description):this(String.Empty, String.Empty, description, String.Empty)
		{
		}

		public Node(string XPathParent, string xmlString, string description, string XPath)
		{
			this.XPathParent	= XPathParent;
			this.xmlstring		= xmlString;
			this.description	= description;
			this.XPath			= XPath;
		}
		
		#endregion

	}		

}
