//-----------------------------------------------------------------------
// <copyright file="DavResourceFindBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Sphorium.WebDAV.Server.Framework.Classes;
using Sphorium.WebDAV.Server.Framework.Collections;
using Sphorium.WebDAV.Server.Framework.Resources;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource Find Framework Base Class
	/// </summary>
	/// <remarks>
	///		Used by DAVPropFindBase and DAVReportBase
	/// </remarks>	
	public abstract class DavResourceFindBase : DavMethodBase
	{
		/// <summary>
		/// Dav Resource Find Framework Base Class
		/// </summary>
		protected DavResourceFindBase()
		{
			this.FileResources = new DavResourceCollection();
			this.MissingResources = new DavResourceCollection();
			this.CollectionResources = new DavResourceCollection();
		}

		/// <summary>
		/// Add a resource
		/// </summary>
		/// <param name="resource"></param>
		protected void AddResource(DavResourceBase resource)
		{
			if (resource is DavFile)
				this.FileResources.Add(resource);
			else if (resource is DavFolder)
				this.CollectionResources.Add(resource);
		}

		/// <summary>
		/// Resource count
		/// </summary>
		protected int ResourceCount
		{
			get
			{
				return this.CollectionResources.Count + this.FileResources.Count;
			}
		}

		/// <summary>
		/// Retrieve the current collection resources
		/// </summary>
		protected DavResourceCollection CollectionResources { get; private set; }

		/// <summary>
		/// Retrieve the current file resources
		/// </summary>
		protected DavResourceCollection FileResources { get; private set; }

		/// <summary>
		/// Retrieve the current missing resources
		/// </summary>
		protected DavResourceCollection MissingResources { get; private set; }


		/// <summary>
		/// Requested properties
		/// </summary>
		protected DavPropertyCollection RequestProperties
		{
			get
			{
				DavPropertyCollection _davProperties = new DavPropertyCollection();

				if (base.RequestXml != null)
				{
					XPathNodeIterator _propNodeIterator = base.RequestXml.SelectDescendants("prop", "DAV:", false);
					if (_propNodeIterator.MoveNext())
					{
						XPathNodeIterator _nodeChildren = _propNodeIterator.Current.SelectChildren(XPathNodeType.All);
						while (_nodeChildren.MoveNext())
						{
							XPathNavigator _currentNode = _nodeChildren.Current;

							if (_currentNode.NodeType == XPathNodeType.Element)
								_davProperties.Add(new DavProperty(_currentNode));
						}
					}
				}

				return _davProperties;
			}
		}

		#region Internal / Private Helper Methods
		/// <summary>
		/// Load the available property names
		/// </summary>
		/// <returns></returns>
		internal int LoadNodePropertyNames()
		{
			using (Stream _responseStream = new MemoryStream())
			{
				XmlTextWriter _xmlWriter = new XmlTextWriter(_responseStream, new UTF8Encoding(false));

				_xmlWriter.Formatting = Formatting.Indented;
				_xmlWriter.IndentChar = '\t';
				_xmlWriter.Indentation = 1;
				_xmlWriter.WriteStartDocument();

				//Set the Multistatus
				_xmlWriter.WriteStartElement("D", "multistatus", "DAV:");

				//Append the folders
				foreach (DavFolder _davFolder in CollectionResources)
					AppendResourceNodeProperties(_davFolder, _xmlWriter);

				//Append the files
				foreach (DavFile _davFile in FileResources)
					AppendResourceNodeProperties(_davFile, _xmlWriter);

				_xmlWriter.WriteEndElement();
				_xmlWriter.WriteEndDocument();
				_xmlWriter.Flush();

				base.SetResponseXml(_responseStream);
				_xmlWriter.Close();
			}
			return (int)ServerResponseCode.MultiStatus;
		}


		/// <summary>
		/// Append a resource's properties to the xml text writer
		/// </summary>
		/// <param name="davResource"></param>
		/// <param name="xmlWriter"></param>
		private static void AppendResourceNodeProperties(DavResourceBase davResource, XmlTextWriter xmlWriter)
		{
			//Open the response element
			xmlWriter.WriteStartElement("response", "DAV:");

			//Load the valid items HTTP/1.1 200 OK
			xmlWriter.WriteElementString("href", "DAV:", davResource.ResourcePath);

			//Open the propstat element section
			xmlWriter.WriteStartElement("propstat", "DAV:");

			//Open the prop element section
			xmlWriter.WriteStartElement("prop", "DAV:");

			//Load the valid properties
			foreach (DavProperty _davProperty in davResource.ResourceProperties)
				_davProperty.ToXML(xmlWriter);

			//Close the prop element section
			xmlWriter.WriteEndElement();

			//Write the status record
			xmlWriter.WriteElementString("status", "DAV:", InternalFunctions.GetEnumHttpResponse(ServerResponseCode.Ok));

			//Close the propstat element section
			xmlWriter.WriteEndElement();
			//END Load the valid items HTTP/1.1 200 OK

			//Close the response element
			xmlWriter.WriteEndElement();
		}



		/// <summary>
		/// Load the requested properties
		/// </summary>
		/// <param name="properties">Requested properties or null for all properties</param>
		/// <returns></returns>
		internal int LoadNodes(DavPropertyCollection properties)
		{
			using (Stream _responseStream = new MemoryStream())
			{
				XmlTextWriter _xmlWriter = new XmlTextWriter(_responseStream, new UTF8Encoding(false));

				_xmlWriter.Formatting = Formatting.Indented;
				_xmlWriter.IndentChar = '\t';
				_xmlWriter.Indentation = 1;
				_xmlWriter.WriteStartDocument();

				//Set the Multistatus
				_xmlWriter.WriteStartElement("D", "multistatus", "DAV:");

				//Append the folders
				foreach (DavFolder _davFolder in CollectionResources)
					AppendResourceNode(_davFolder, properties, _xmlWriter);

				//Append the files
				foreach (DavFile _davFile in FileResources)
					AppendResourceNode(_davFile, properties, _xmlWriter);

				//Append the files
				foreach (DavResourceBase _missingResource in MissingResources)
					AppendResourceNode(_missingResource, properties, _xmlWriter);

				_xmlWriter.WriteEndElement();
				_xmlWriter.WriteEndDocument();
				_xmlWriter.Flush();

				base.SetResponseXml(_responseStream);
				_xmlWriter.Close();
			}
			return (int)ServerResponseCode.MultiStatus;
		}


		/// <summary>
		/// Append a resource to the xml text writer
		/// </summary>
		/// <param name="davResource"></param>
		/// <param name="requestedProperties"></param>
		/// <param name="xmlWriter"></param>
		private static void AppendResourceNode(DavResourceBase davResource, DavPropertyCollection requestedProperties, XmlTextWriter xmlWriter)
		{
			DavPropertyCollection _validProperties = new DavPropertyCollection();
			DavPropertyCollection _invalidProperties = new DavPropertyCollection();

			davResource.GetPropertyValues(requestedProperties, _validProperties, _invalidProperties);

			//Open the response element
			xmlWriter.WriteStartElement("response", "DAV:");

			//Load the valid items HTTP/1.1 200 OK
			xmlWriter.WriteElementString("href", "DAV:", davResource.ResourcePath);

			//Open the propstat element section
			xmlWriter.WriteStartElement("propstat", "DAV:");

			//Open the prop element section
			xmlWriter.WriteStartElement("prop", "DAV:");

			//Load the valid properties
			foreach (DavProperty _davProperty in _validProperties)
				_davProperty.ToXML(xmlWriter);

			//Close the prop element section
			xmlWriter.WriteEndElement();

			//Write the status record
			xmlWriter.WriteElementString("status", "DAV:", InternalFunctions.GetEnumHttpResponse(ServerResponseCode.Ok));

			//Close the propstat element section
			xmlWriter.WriteEndElement();
			//END Load the valid items HTTP/1.1 200 OK

			//Load the invalid items HTTP/1.1 404 Not Found
			if (_invalidProperties.Count > 0)
			{
				xmlWriter.WriteStartElement("propstat", "DAV:");
				xmlWriter.WriteElementString("status", "DAV:", InternalFunctions.GetEnumHttpResponse(ServerResponseCode.NotFound));

				//Open the prop element section
				xmlWriter.WriteStartElement("prop", "DAV:");

				//Load all the invalid properties
				foreach (DavProperty _davProperty in _invalidProperties)
					_davProperty.ToXML(xmlWriter);

				//Close the prop element section
				xmlWriter.WriteEndElement();
				//Close the propstat element section
				xmlWriter.WriteEndElement();
			}
			//END Load the invalid items HTTP/1.1 404 Not Found

			//Close the response element
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// Append a resource to the xml text writer
		/// </summary>
		/// <param name="davResource"></param>
		/// <param name="requestedProperties"></param>
		/// <param name="xmlWriter"></param>
		private static void AppendInvalidResourceNode(DavResourceBase davResource, DavPropertyCollection requestedProperties, XmlTextWriter xmlWriter)
		{
			DavPropertyCollection _validProperties = new DavPropertyCollection();
			DavPropertyCollection _invalidProperties = new DavPropertyCollection();

			davResource.GetPropertyValues(requestedProperties, _validProperties, _invalidProperties);

			//Open the response element
			xmlWriter.WriteStartElement("response", "DAV:");

			//Load the valid items HTTP/1.1 200 OK
			xmlWriter.WriteElementString("href", "DAV:", davResource.ResourcePath);

			//Open the propstat element section
			xmlWriter.WriteStartElement("propstat", "DAV:");

			//Open the prop element section
			xmlWriter.WriteStartElement("prop", "DAV:");

			//Load the valid properties
			foreach (DavProperty _davProperty in _validProperties)
				_davProperty.ToXML(xmlWriter);

			//Close the prop element section
			xmlWriter.WriteEndElement();

			//Write the status record
			xmlWriter.WriteElementString("status", "DAV:", InternalFunctions.GetEnumHttpResponse(ServerResponseCode.Ok));

			//Close the propstat element section
			xmlWriter.WriteEndElement();
			//END Load the valid items HTTP/1.1 200 OK

			//Load the invalid items HTTP/1.1 404 Not Found
			if (_invalidProperties.Count > 0)
			{
				xmlWriter.WriteStartElement("propstat", "DAV:");

				//Open the prop element section
				xmlWriter.WriteStartElement("prop", "DAV:");

				//Load all the invalid properties
				foreach (DavProperty _davProperty in _invalidProperties)
					_davProperty.ToXML(xmlWriter);

				//Close the prop element section
				xmlWriter.WriteEndElement();

				//Write the status record
				xmlWriter.WriteElementString("status", "DAV:", InternalFunctions.GetEnumHttpResponse(ServerResponseCode.NotFound));

				//Close the propstat element section
				xmlWriter.WriteEndElement();
			}
			//END Load the invalid items HTTP/1.1 404 Not Found

			//Close the response element
			xmlWriter.WriteEndElement();
		}

		#endregion
	}
}