using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace Utilities.Hotfix.Xml
{
	[Serializable()]
	public class XmlConfigurationHotfix
	{
		#region Properties
		
		[XmlAttribute()]
		public float version;
		
		[XmlAttribute()]
		public string description;
		
		[XmlElement(ElementName="XMLContentFixes")]
		public XmlContentFix[] objXMLContentFixes;

		private XmlNamespaceManager objNamespaceManager		= new XmlNamespaceManager(new NameTable());
		private ArrayList			arrDetectedNamespaces	= new ArrayList();

		#endregion

		#region Ctors
		
		public XmlConfigurationHotfix()
		{
			version				= 0.0f;
			description			= string.Empty;
			objXMLContentFixes	= new XmlContentFix[]{};
		}
		
		/// <summary>
		/// Top level class to describe configuration changes in XML files.
		/// </summary>
		/// <param name="_version">Version of hotfix applied</param>
		/// <param name="_description">Literal description of fixes applied</param>
		/// <param name="fixes">Array of XmlContentFix objects describing fixes for single xml file</param>
		public XmlConfigurationHotfix(	float			_version, 
										string			_description, 
										XmlContentFix[] fixes)
		{
			version				= _version;
			description			= _description;
			objXMLContentFixes	= fixes;
		}
		
		#endregion

		#region FUNCTIONS
		public void Save(string xmlfile)
		{
			//serialize to file
			XmlSerializer serializer = new XmlSerializer(typeof(XmlConfigurationHotfix)); 
			TextWriter tw = new StreamWriter(xmlfile); 
			serializer.Serialize(tw,this); 
			tw.Close();
		}
		
		public static XmlConfigurationHotfix Load(string xmlfile)
		{
			//desirialize from file
			XmlSerializer serializer = new XmlSerializer(typeof(XmlConfigurationHotfix)); 
			TextReader tr = new StreamReader(xmlfile); 
			XmlConfigurationHotfix inst = serializer.Deserialize(tr) as XmlConfigurationHotfix; 
			tr.Close();
			return inst;
		}


		public void Install(Logger log)
		{
			//----------------------------------------------------
			// Create default instance of logger if nothing sent
			//----------------------------------------------------
			if(log == null)
			{
				log = new Logger(Logger.GetNewLogFilename());
			}

			//----------------------------------------------------
			// Check that some fixes available and install them
			//----------------------------------------------------
			if(objXMLContentFixes.Length >0 && File.Exists(objXMLContentFixes[0].contentXmlFilePath))
			{
				log.WriteLine(LogType.Info, "HI026: " +
					String.Format("[Installing xml file update]: {0}, Description: \"{1}\", Version: {2}",
						objXMLContentFixes.Length, 
						this.description, 
						this.version)
				);

				#region install each content fix
				foreach(XmlContentFix contentfix in objXMLContentFixes)
				{
					XmlDocument		fixedDoc  = null; // Fixed XML document
					XmlTextWriter	objWriter = null; // Writer to persist changes in fixedDoc

					if(File.Exists(contentfix.contentXmlFilePath))
					{
						try
						{
							//-----------------------------------------------------------------
							// Load fixed Xml Document
							//-----------------------------------------------------------------
							fixedDoc = new XmlDocument();
							fixedDoc.Load(contentfix.contentXmlFilePath);

							//-----------------------------------------------------------------
							// Detect and prepare namespaces and namespace manager
							//-----------------------------------------------------------------
							this.DetectNamespaces(fixedDoc);
						
							log.WriteLine(LogType.Info, "HI027: " +
								String.Format("Updating target Xml file: \"{1}\", Description: \"{0}\"",
								contentfix.description, 
								contentfix.contentXmlFilePath)
								);

							#region NODE FIXING
							
							//-----------------------------------------------------------------
							// NODE FIXING
							//-----------------------------------------------------------------

							#region DELETE node selected by XPath expression
							foreach(Node node in contentfix.objContentFixNode.DeleteChildren)
							{
								
								XmlNodeList list = fixedDoc.SelectNodes(node.XPath,objNamespaceManager);
								
								foreach(XmlNode fixednode in list)
								{
									fixednode.ParentNode.RemoveChild(fixednode);

									log.WriteLine(LogType.Info, "HI033: " +
										string.Format("XmlNode DELETED. XPath: \"{0}\"", node.XPath));
								}
								
								if(list == null || list.Count == 0)
								{
									log.WriteLine(LogType.Warning, "HI034: " +
										string.Format("According to XPath expression: \"{0}\" nothing selected for deleting.", 
										node.XPath)
										);
								}

							}
							#endregion

							#region PREPEND fragments to specified parent node
							
							// reverse order of nodes because each of them will be added as first
							Array.Reverse(contentfix.objContentFixNode.PrependChildren);
							
							foreach(Node node in contentfix.objContentFixNode.PrependChildren)
							{
								XmlNodeList objNodesExist = fixedDoc.SelectNodes(node.XPath, objNamespaceManager);

								if(objNodesExist.Count == 0)
								{
								
									XmlNodeList list = fixedDoc.SelectNodes(node.XPathParent,objNamespaceManager);
								
									//XmlDocumentFragment inherits from XmlNode
									XmlDocumentFragment fragment = fixedDoc.CreateDocumentFragment();
								
									// set fragment xml string
									fragment.InnerXml = node.xmlstring;
								
									foreach(XmlNode fixednode in list)
									{
										fixednode.PrependChild(fragment);
									
										log.WriteLine(LogType.Info, "HI035: " +
											string.Format("XmlNode PREPENDed to parent. XPathParent:\"{0}\"",
											node.XPathParent));
									}

								}
								else
								{
									log.WriteLine(LogType.Warning, "HI036: " +
										string.Format("XmlNode NOT PREPENDed to parent - already exist. XPathParent: \"{0}\"",
										node.XPathParent));
								}
							}

							//restore native order of added nodes 
							Array.Reverse(contentfix.objContentFixNode.PrependChildren);
							
							#endregion

							#region APPEND fragments to specified parent node
							foreach(Node node in contentfix.objContentFixNode.AppendChildren)
							{
								XmlNodeList objNodesExist = fixedDoc.SelectNodes(node.XPath, objNamespaceManager);

								if(objNodesExist.Count == 0)
								{
									XmlNodeList list = fixedDoc.SelectNodes(node.XPathParent,objNamespaceManager);
									
									//XmlDocumentFragment inherit from XmlNode
									XmlDocumentFragment fragment = fixedDoc.CreateDocumentFragment();
									
									try
									{
										// If node.xmlstring contains non well formatted xml string then
										// exception will be thrown, error will be logged
										fragment.InnerXml = node.xmlstring;
									}
									catch
									{
										log.WriteLine(LogType.Error, "HI065: " +
											string.Format("Node: {1}, Failed to parse inner xml fragment: \"{0}\"",
											node.xmlstring, node.description));
										continue;
									}
								
									foreach(XmlNode fixednode in list)
									{
									
										fixednode.AppendChild(fragment);

										log.WriteLine(LogType.Info, "HI037: " +
											string.Format("XmlNode APPENDed to parent: XPath:\"{0}\". Description: \"{1}\"",
											node.XPathParent, node.description));
									}
								}
								else
								{
									log.WriteLine(LogType.Warning, "HI038: " +
										string.Format("XmlNode NOT APPENDed to parent - already exist. XPathParent: \"{0}\". Description: \"{1}\"",
										node.XPathParent, node.description));
								}							
							}

							#endregion
							
							#endregion // NODE FIXING

							#region ATTRIBUTES FIXING
							
							//-----------------------------------------------------------------
							// ATTRIBUTES FIXING
							//-----------------------------------------------------------------
							// Adding / Copying / Updating  / Deleting

							#region ADD		attributes to specified nodes
							
							// Add attributes to specified nodes
							if(contentfix.objContentFixAttribute.AddAttributes != null)
							foreach(Attribute attr in contentfix.objContentFixAttribute.AddAttributes)
							{
								// Pick up nodes that new attributes will be added to
								XmlNodeList list = fixedDoc.SelectNodes(attr.XPath,objNamespaceManager);
								
								XmlNode added = fixedDoc.CreateNode(XmlNodeType.Attribute,attr.attribute_name,attr.strNamespaceURI);
								
								// Follow each picked up node and set named item (attribute)
								foreach(XmlNode fixednode in list)
								{
									added.Value=attr.attribute_value;
									
									// TODO: check existing attributes - FINISHED( existing attribute will be replaced)
									// An attribute node to store in this collection. 
									// The node will later be accessible using the name of the node. 
									// If a node with that name is ALREADY present in the collection, 
									//	it is replaced by the new one; 
									//  otherwise, the node is appended to the end of the collection. 
									fixednode.Attributes.SetNamedItem(added);

									log.WriteLine(LogType.Info, "HI032: " +
										String.Format("Added attribute: XPath: \"{2}\", NAME: \"{0}\", VALUE: \"{1}\"", 
										added.Name,
										added.Value,
										attr.XPath));

								}
							}
							#endregion ATTRIBUTES FIXING

							#region COPYING attribute from source
							
							// Copying attributes to specified nodes
							if(contentfix.objContentFixAttribute.CopyAttributes != null)
							foreach(Attribute attr in contentfix.objContentFixAttribute.CopyAttributes)
							{
								//SOURCE: Pick up node that attribute copied from
								XmlNodeList objSourceNodes = fixedDoc.SelectNodes(attr.strSourceXPath,objNamespaceManager);

								//DESTINATION: Pick up nodes that copied attributes will be added to
								XmlNodeList list = fixedDoc.SelectNodes(attr.XPath,objNamespaceManager);
								
								//--------------------------------------------------------------------
								// CONDITIONal copying: Copy attribute if nothing found on XPath expression
								//--------------------------------------------------------------------
								#region CONDITIONal copying
								if(attr.strCopyIfNotFound != string.Empty)
								{
									try
									{
										XmlNodeList objConditionList = fixedDoc.SelectNodes(attr.strCopyIfNotFound,objNamespaceManager);
										if(objConditionList.Count >0)
										{
											log.WriteLine(LogType.Warning, "HI066: " +
												String.Format("Copying cancelled. Condition: \"{0}\", Count:\"{1}\"", 
												attr.strCopyIfNotFound,
												objConditionList.Count
												));
											continue;
										}
									}
									catch(Exception objException)
									{
										#region Log: Failed to evaluate condition of attribute coping
										log.WriteLine(LogType.Error, "HI067: " +
											String.Format("Failed on copy condition: \"{0}\", Message: \"{1}\"", 
											attr.strCopyIfNotFound,
											objException.Message
											));
										#endregion // Log: Failed to evaluate condition of attribute coping
									}
								}

								#endregion // CONDITIONal copying:
																		 
								if(objSourceNodes.Count > 0)
								{
									XmlAttribute objSourceAttribute = null;
									//Get Source Attribute object from source node
									try
									{
										objSourceAttribute = 
											objSourceNodes.Item(0).Attributes.GetNamedItem(attr.strSourceAttrName) as XmlAttribute;
									}
									catch
									{
										objSourceAttribute = null;
										log.WriteLine(LogType.Error, "HI048: " +
											String.Format("Copied attribute Not found: Source: \"{0}\", NAME: \"{1}\"", 
											objSourceAttribute.Name,
											objSourceAttribute.Value
											));
									}
									if(objSourceAttribute == null )
									{
										//Attribute not found, continue to next copied attribute
										continue;
									}
									
									// If name of target attribute not specified then get the name of source 
									// attribute to target attribute
									if(attr.attribute_name == string.Empty)
									{
										attr.attribute_name = objSourceAttribute.Name;
									}

									XmlNode objTargetAttribute = fixedDoc.CreateNode(XmlNodeType.Attribute,
										attr.attribute_name,
										attr.strNamespaceURI);
									// Check that destination nodes exist
									if(list.Count == 0)
									{
										#region Log that nothing found to copy to
										log.WriteLine(LogType.Warning, "HI049: " +
											String.Format("Copy attribute, Updated node not found: \"{0}\", NAME: \"{1}\"", 
											attr.XPath,
											objTargetAttribute.Name
											));
										#endregion // Log copied attribute
									}
									
									//============================================
									// Copy prepared attribute to destination node
									//============================================
									foreach(XmlNode fixednode in list)
									{
										objTargetAttribute.Value=objSourceAttribute.Value;
										
										fixednode.Attributes.SetNamedItem(objTargetAttribute);

										#region Log copied attribute
										log.WriteLine(LogType.Info, "HI047: " +
											String.Format("Copied attribute: Target: \"{0}\", NAME: \"{1}\", VALUE: \"{2}\", Source:\"{3}\", NAME: \"{4}\", VALUE: \"{5}\"", 
											attr.XPath,
											objTargetAttribute.Name,
											objTargetAttribute.Value,
											attr.strSourceXPath,
											objSourceAttribute.Name,
											objSourceAttribute.Value
											));
										#endregion // Log copied attribute
									}
								}
							}
							#endregion

							#region UPDATE attributes of specified nodes
							// update attributes of specified nodes
							if(contentfix.objContentFixAttribute.UpdateAttributes != null)
							foreach(Attribute attr in contentfix.objContentFixAttribute.UpdateAttributes)
							{
								XmlNodeList objList = fixedDoc.SelectNodes(attr.XPath,objNamespaceManager);
								foreach(XmlNode fixednode in objList)
								{
									XmlAttribute updated = fixednode.Attributes[attr.attribute_name];
									if(updated != null)
									{
										string old_value = updated.Value;
										// attribute found and updated
										updated.Value=attr.attribute_value;

										#region Log that attribute Updated
										log.WriteLine(LogType.Info, "HI030: " +
											String.Format("Updated attribute XPath: \"{3}\", NAME: \"{0}\", VALUE: \"{1}\"-> \"{2}\", DESCRIPTION:{4}", 
											updated.Name,
											old_value,
											updated.Value,
											attr.XPath,
											attr.description));
										#endregion // Log that attribute Updated
									}
									else
									{	
										#region Log Error that attribute not found
										log.WriteLine(LogType.Error, "HI031: " +
											String.Format("Updated attribute was NOT FOUND: XPath: \"{1}\", NAME: \"{0}\", DESCRIPTION:{2}", 
											attr.attribute_name,
											attr.XPath,
											attr.description));
										#endregion //attribute not found
									}
								}

								//----------------------------------------------------------
								// List of updated attributes empty, nothing will be updated
								//----------------------------------------------------------
								if(objList != null && objList.Count==0)
								{
									#region Log that Attribute not found or up to date
									log.WriteLine(LogType.Warning, "HI064: " +
										String.Format("Attribute up to date (or not found): XPath: \"{0}\", NAME: \"{1}\", DESCRIPTION:{2}", 
										attr.XPath,
										attr.attribute_name,
										attr.description));
									#endregion //Log that Attribute not found or up to date
								}
							}
							#endregion
							
							#region DELETE attributes to specified nodes
							
							// delete attributes to specified nodes
							if(contentfix.objContentFixAttribute.DeleteAttributes != null)
							foreach(Attribute attr in contentfix.objContentFixAttribute.DeleteAttributes)
							{
								XmlNodeList list = fixedDoc.SelectNodes(attr.XPath, objNamespaceManager);
								
								foreach(XmlNode fixednode in list)
								{
									XmlAttribute removed = fixednode.Attributes[attr.attribute_name];
									if(removed!=null)
									{
										// attribute found and deleted
										fixednode.Attributes.Remove(removed);
										log.WriteLine(LogType.Info, "HI028: " +
											String.Format("Deleted attribute: \"{2}\", NAME: \"{0}\", VALUE: \"{1}\"", 
											removed.Name,
											removed.Value,
											attr.XPath));
									}
									else
									{	// Attribute not found: already deleted OR never existed
										// That's kind of Error but for multiple updates, deleted attribute
										// will not be exist so that's not error in general
										log.WriteLine(LogType.Warning, "HI029: " +
											String.Format("Deleted attribute was NOT FOUND : XPath\"{1}\", NAME: \"{0}\"", 
											attr.attribute_name, 
											attr.XPath));
									}
								}
							}
							
							#endregion
							
							#endregion

							//-----------------------------------------------------------------
							// Save fixed Xml Document to a file and auto-indent the output
							//-----------------------------------------------------------------
							objWriter = new XmlTextWriter(contentfix.contentXmlFilePath, System.Text.Encoding.UTF8);
							objWriter.Formatting = Formatting.Indented;
							fixedDoc.Save(objWriter);

							log.WriteLine(LogType.Info, "HI039: " +
								string.Format("Updated Xml file saved to: {0}",contentfix.contentXmlFilePath));

							//Updating of xml file finished ...
						}
						catch(Exception objException)
						{
							log.WriteLine(LogType.Error, "HI040: " +
								"Failed to load or update xml file: " + contentfix.contentXmlFilePath + Environment.NewLine + 
								objException.ToString());
						}
						finally
						{
							#region Free/close fixed document
							fixedDoc = null;
							try
							{
								objWriter.Close();
							}
							catch
							{
							}
							#endregion // Dispose document to free/close fixed document
						}
					}
					else 
					{	// File intended to be fixed not found
						log.WriteLine(LogType.Warning, "HI044: " +
							"Xml file not found: " + contentfix.contentXmlFilePath);
					}

				}//foreach contentfix
				#endregion // install each content fix
			}

		}


		/// <summary>
		/// Find out all namespaces defined in given XmlDocument
		/// </summary>
		/// <returns></returns>
		public Array DetectNamespaces(XmlDocument xmlDoc)
		{
			arrDetectedNamespaces	= new ArrayList();
			objNamespaceManager		= new XmlNamespaceManager(new NameTable());

			NamespaceDiscover(xmlDoc.DocumentElement);

			foreach (string ns in arrDetectedNamespaces)
			{
				int tokenPos = ns.IndexOf(':');

				string prefix	= ns.Substring(0, tokenPos);
				string uri		= ns.Substring(tokenPos + 1);

				objNamespaceManager.AddNamespace(prefix.Trim(), uri.Trim());
			}

			return arrDetectedNamespaces.ToArray();
		}


		/// <summary>
		/// Find out all namespaces for given Node and descendant nodes (in recursive manner)
		/// </summary>
		/// <param name="xNode">Checked XmlNode object that search starts</param>
		private void NamespaceDiscover(XmlNode xNode)
		{
			string nsPrefix;
			string nsUri;

			if (xNode.NodeType == XmlNodeType.Element)
			{
				if (xNode.Attributes.Count > 0)
				{
					int attrNum = 0;
					while (attrNum < xNode.Attributes.Count)
					{
						if (xNode.Attributes[attrNum].Name.StartsWith("xmlns"))
						{
							string nsDef = xNode.Attributes[attrNum].Name.Split('=')[0];
							if (nsDef != "xmlns")
							{
								nsPrefix = nsDef.Split(':')[1];
							}
							else
							{
								nsPrefix = "def";
							}
							nsUri = xNode.Attributes[attrNum].Value;

							arrDetectedNamespaces.Add(string.Format("{0}: {1}", nsPrefix, nsUri));
						}
						attrNum++;
					}
				}
				if (xNode.HasChildNodes)
				{
					int elemNum = 0;
					while (elemNum < xNode.ChildNodes.Count)
					{
						NamespaceDiscover(xNode.ChildNodes[elemNum]);
						elemNum++;
					}
				}
			}
		}

		
		#endregion // FUNCTIONS
	}
}
