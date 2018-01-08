using System;
using System.Xml;
using System.Xml.Serialization;

namespace Utilities.Hotfix.Xml
{
	/// <summary>
	/// Value class of Node objects for updating XML file:
	/// -	prepend node before specified node 
	/// -	append node after specified node
	/// -	delete specified node (entire including descendent nodes)
	/// </summary>
	[Serializable()]
	public class ContentFixNode
	{
		#region properties
		public Node[] PrependChildren;		// prepend children elements in node
		public Node[] AppendChildren;		// append children elements in node
		public Node[] DeleteChildren;		// deleted nodes
		#endregion
		
		#region ctors
		public ContentFixNode()
		{
			PrependChildren = new Node[]{};
			AppendChildren = new Node[]{};
			DeleteChildren = new Node[]{};
		}
		public ContentFixNode(
			Node[] _PrependChildren,
			Node[] _AppendChildren,
			Node[] _DeleteChildren)
		{
			PrependChildren = _PrependChildren;
			AppendChildren = _AppendChildren;
			DeleteChildren = _DeleteChildren;
		}
		#endregion
	}
}
