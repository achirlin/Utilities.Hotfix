using System;
using System.Xml;
using System.Xml.Serialization;

namespace Utilities.Hotfix.Xml
{
	/// <summary>
	/// Value class of Attribute objects for 
	/// adding new attributes, update attributes and deleting attributes.
	/// </summary>
	[Serializable()]
	public class ContentFixAttribute
	{
		#region properties
		public Attribute[] AddAttributes;
		public Attribute[] UpdateAttributes;
		public Attribute[] DeleteAttributes;
		public Attribute[] CopyAttributes; 
		#endregion

		#region ctors
		public ContentFixAttribute()
		{
			AddAttributes		= new Attribute[]{};
			UpdateAttributes	= new Attribute[]{};
			DeleteAttributes	= new Attribute[]{};
			CopyAttributes		= new Attribute[]{};
		}
		public ContentFixAttribute(
			Attribute[] AddAttributes,
			Attribute[] UpdateAttributes,
			Attribute[] DeleteAttributes,
			Attribute[] CopyAttributes)
		{
			this.AddAttributes		= AddAttributes;
			this.UpdateAttributes	= UpdateAttributes;
			this.DeleteAttributes	= DeleteAttributes;
			this.CopyAttributes		= CopyAttributes;
		}
		#endregion
	}
}
