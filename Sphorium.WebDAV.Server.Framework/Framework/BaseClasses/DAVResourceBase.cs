//-----------------------------------------------------------------------
// <copyright file="DavResourceBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Sphorium.WebDAV.Server.Framework.Classes;
using Sphorium.WebDAV.Server.Framework.Collections;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Summary description for DavResource.
	/// </summary>
	public abstract class DavResourceBase
	{
		/// <summary>
		/// Keeps track of internal parameters
		/// </summary>
		private Dictionary<string, PropertyInfo> __classProperties = new Dictionary<string, PropertyInfo>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		/// Dav Resource Framework Base Class
		/// </summary>
		/// <param name="displayName">Resource display name</param>
		/// <param name="resourcePath">Resource path</param>
		protected DavResourceBase(string displayName, string resourcePath)
		{
			this.DisplayName = displayName;
			this.ResourcePath = resourcePath;

			this.LastModified = DateTime.MinValue;
			this.CreationDate = DateTime.MinValue;
			this.ContentLanguage = "en-us";

			this.ActiveLocks = new DavLockPropertyCollection();
			this.ResourceVersions = new DavResourceVersionCollection();


			//if (displayName == null || displayName.Length == 0)
			//    throw new ArgumentNullException("displayName", InternalFunctions.GetResourceString("ArgumentNullStringException", "DisplayName"));

			if (resourcePath == null || resourcePath.Length == 0)
				throw new ArgumentNullException("resourcePath", InternalFunctions.GetResourceString("ArgumentNullStringException", "ResourcePath"));

			//Populate the class properties hashtable
			foreach (PropertyInfo _property in this.GetType().GetProperties())
			{
				bool _includeProperty = true;

				//Check to see if we should include this property
				ResourcePropertyAttribute _propertyAttribute = Attribute.GetCustomAttribute(_property, typeof(ResourcePropertyAttribute)) as ResourcePropertyAttribute;
				if (_propertyAttribute != null)
					_includeProperty = _propertyAttribute.EnableAutoDiscovery;

				if (_includeProperty)
				{
					if (!this.__classProperties.ContainsKey(_property.Name))
						this.__classProperties.Add(_property.Name, _property);
				}
			}

			this.CustomProperties = new DavPropertyCollection();
		}


		/// <summary>
		/// Returns a list of all available property names
		/// </summary>
		/// <returns></returns>
		[ResourcePropertyAttribute(false)]
		public DavPropertyCollection ResourceProperties
		{
			get
			{
				DavPropertyCollection _resourceProperties = new DavPropertyCollection();

				foreach (PropertyInfo _property in this.__classProperties.Values)
				{
					string _xmlPropName = _property.Name.ToLower();

					//Check to see if we should include this property
					ResourcePropertyAttribute _propertyAttribute = Attribute.GetCustomAttribute(_property, typeof(ResourcePropertyAttribute)) as ResourcePropertyAttribute;
					if (_propertyAttribute != null && !string.IsNullOrEmpty(_propertyAttribute.XmlPropName))
						_xmlPropName = _propertyAttribute.XmlPropName;

					_resourceProperties.Add(new DavProperty(_xmlPropName, "DAV:"));
				}

				//Add the custom properties as well
				foreach (DavProperty _customProperty in CustomProperties)
					_resourceProperties.Add(_customProperty);

				return _resourceProperties;
			}
		}

		/// <summary>
		/// Resource custom properties
		/// </summary>
		[ResourcePropertyAttribute(false)]
		public DavPropertyCollection CustomProperties { get; private set; }


		/// <summary>
		/// Retrieve resource property values
		/// </summary>
		/// <param name="requestedProperties"></param>
		/// <param name="validProperties"></param>
		/// <param name="invalidProperties"></param>
		public virtual void GetPropertyValues(DavPropertyCollection requestedProperties, DavPropertyCollection validProperties, DavPropertyCollection invalidProperties)
		{
			if (validProperties == null)
				throw new ArgumentNullException("validProperties", InternalFunctions.GetResourceString("ArgumentNullException", "ValidProperties"));
			else if (invalidProperties == null)
				throw new ArgumentNullException("invalidProperties", InternalFunctions.GetResourceString("ArgumentNullException", "InvalidProperties"));

			//Clear out all the properties
			validProperties.Clear();
			invalidProperties.Clear();

			//Requesting ALL available properties
			DavPropertyCollection _requestedProperties;
			if (requestedProperties == null)
				_requestedProperties = this.ResourceProperties.Clone();
			else
				_requestedProperties = requestedProperties.Clone();


			//Check to see if there is a valid property
			foreach (DavProperty _property in _requestedProperties)
			{
				string _propertyName = _property.Name ?? "";
				if (_propertyName.ToLower(CultureInfo.InvariantCulture).StartsWith("get"))
					_propertyName = _propertyName.Substring(3);

				if (this.__classProperties.ContainsKey(_propertyName))
				{
					PropertyInfo _resourceProperty = this.__classProperties[_propertyName] as PropertyInfo;
					if (_resourceProperty != null)
					{
						if (_resourceProperty.PropertyType == typeof(XPathNavigator))
						{
							XPathNavigator _propertyValue = (XPathNavigator)_resourceProperty.GetValue(this, null);
							if (_propertyValue != null)
								validProperties.Add(new DavProperty(_propertyValue));
						}
						else if (_resourceProperty.PropertyType == typeof(ResourceType))
						{
							ResourceType _resource = (ResourceType)_resourceProperty.GetValue(this, null);

							switch (_resource)
							{
								case ResourceType.Collection:
									DavProperty _folderResourceType = new DavProperty("resourcetype", "DAV:");
									_folderResourceType.NestedProperties.Add(new DavProperty("collection", "DAV:"));

									validProperties.Add(_folderResourceType);
									break;

								//case ResourceType.Resource:
								//    //DavProperty _fileResourceType = new DavProperty("resourcetype", "DAV:");
								//    //validProperties.Add(_fileResourceType);
								//    break;
							}
						}
						else if (_resourceProperty.PropertyType == typeof(bool))
						{
							bool _propertyValue = (bool)_resourceProperty.GetValue(this, null);

							if (_propertyValue)
								validProperties.Add(_property);
							else
								invalidProperties.Add(_property);
						}
						else if (_resourceProperty.PropertyType == typeof(DateTime))
						{
							DateTime _propertyValue = (DateTime)_resourceProperty.GetValue(this, null);
							if (_propertyValue != DateTime.MinValue)
							{
								switch (_resourceProperty.Name.ToLower(CultureInfo.InvariantCulture))
								{
									case "lastmodified":
										{
											//DavPropertyAttribute _propertyAttribute = new DavPropertyAttribute();
											//_propertyAttribute.AttributeName = "dt";
											//_propertyAttribute.AttributeNamespace = "b";
											//_propertyAttribute.AttributeValue = "dateTime.rfc1123";

											//_property.Attributes.Add(_propertyAttribute);
											//_property.Value = _propertyValue.ToString("r", CultureInfo.InvariantCulture);

											//_property.Value = _propertyValue.ToString(CultureInfo.InvariantCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.InvariantCulture);

											_property.Value = _propertyValue.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
										}
										break;

									case "creationdate":
										{
											//DavPropertyAttribute _propertyAttribute = new DavPropertyAttribute();
											//_propertyAttribute.AttributeName = "dt";
											//_propertyAttribute.AttributeNamespace = "b";
											//_propertyAttribute.AttributeValue = "dateTime.tz";

											//_property.Attributes.Add(_propertyAttribute);
											//_property.Value = this.__creationDate.ToString("s", CultureInfo.InvariantCulture);

											_property.Value = _propertyValue.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
										}
										break;
								}

								validProperties.Add(_property);
							}
							else
								invalidProperties.Add(_property);
						}
						else
						{
							string _resourceValue = _resourceProperty.GetValue(this, null).ToString();

							if (_resourceValue != null && _resourceValue.Length > 0)
							{
								_property.Value = _resourceValue;
								validProperties.Add(_property);
							}
							else
								invalidProperties.Add(_property);
						}
					}
				}
				else if (this.CustomProperties[_propertyName] != null)
					validProperties.Add(_property);
				else
					invalidProperties.Add(_property);
			}
		}


		#region Lock Support
		/// <summary>
		/// Property detailing if the resource supports exclusive locking
		/// </summary>
		[ResourcePropertyAttribute(false)]
		public bool SupportsExclusiveLock { get; set; }


		/// <summary>
		/// Property detailing if the resource supports shared locking
		/// </summary>
		[ResourcePropertyAttribute(false)]
		public bool SupportsSharedLock { get; set; }


		/// <summary>
		/// Active Locks
		/// </summary>
		[ResourcePropertyAttribute(false)]
		public DavLockPropertyCollection ActiveLocks { get; private set; }


		/// <summary>
		/// Active Locks
		/// </summary>
		[ResourcePropertyAttribute(false)]
		public DavResourceVersionCollection ResourceVersions { get; private set; }


		/// <summary>
		/// XmlNode detailing current resource locks
		/// </summary>
		public XPathNavigator LockDiscovery
		{
			get
			{
				XPathNavigator _lockDiscoveryNavigator = null;
				using (Stream _responseStream = new MemoryStream())
				{
					XmlTextWriter _xmlWriter = new XmlTextWriter(_responseStream, Encoding.UTF8);
					_xmlWriter.Formatting = Formatting.Indented;
					_xmlWriter.IndentChar = '\t';
					_xmlWriter.Indentation = 1;
					_xmlWriter.WriteStartDocument();

					//Set the Multistatus
					_xmlWriter.WriteStartElement("D", "lockdiscovery", "DAV:");

					foreach (DavLockProperty _lockProperty in ActiveLocks)
						_lockProperty.ActiveLock.WriteTo(_xmlWriter);

					_xmlWriter.WriteEndElement();
					_xmlWriter.WriteEndDocument();
					_xmlWriter.Flush();

					//Move the stream back to the start position
					_responseStream.Seek(0, SeekOrigin.Begin);
					_lockDiscoveryNavigator = new XPathDocument(_responseStream).CreateNavigator();

					XPathNodeIterator _lockDiscoveryNodeIterator = _lockDiscoveryNavigator.SelectDescendants("lockdiscovery", "DAV:", false);
					if (_lockDiscoveryNodeIterator.MoveNext())
						_lockDiscoveryNavigator = _lockDiscoveryNodeIterator.Current;

					_xmlWriter.Close();
				}
				return _lockDiscoveryNavigator;
			}
		}


		/// <summary>
		/// Supported Lock information
		/// </summary>
		public XPathNavigator SupportedLock
		{
			get
			{
				XPathNavigator _supportedLockNavigator = null;
				using (Stream _responseStream = new MemoryStream())
				{
					XmlTextWriter _xmlWriter = new XmlTextWriter(_responseStream, Encoding.UTF8);
					_xmlWriter.Formatting = Formatting.Indented;
					_xmlWriter.IndentChar = '\t';
					_xmlWriter.Indentation = 1;
					_xmlWriter.WriteStartDocument();

					//Set the Multistatus
					_xmlWriter.WriteStartElement("D", "supportedlock", "DAV:");

					if (this.SupportsExclusiveLock)
					{
						_xmlWriter.WriteStartElement("lockentry", "DAV:");
						_xmlWriter.WriteStartElement("lockscope", "DAV:");
						_xmlWriter.WriteElementString("exclusive", "DAV:", "");
						_xmlWriter.WriteEndElement();
						_xmlWriter.WriteStartElement("locktype", "DAV:");
						_xmlWriter.WriteElementString("write", "DAV:", "");
						_xmlWriter.WriteEndElement();
						_xmlWriter.WriteEndElement();
					}

					if (this.SupportsSharedLock)
					{
						_xmlWriter.WriteStartElement("lockentry", "DAV:");
						_xmlWriter.WriteStartElement("lockscope", "DAV:");
						_xmlWriter.WriteElementString("shared", "DAV:", "");
						_xmlWriter.WriteEndElement();
						_xmlWriter.WriteStartElement("locktype", "DAV:");
						_xmlWriter.WriteElementString("write", "DAV:", "");
						_xmlWriter.WriteEndElement();
						_xmlWriter.WriteEndElement();
					}

					_xmlWriter.WriteEndElement();
					_xmlWriter.WriteEndDocument();
					_xmlWriter.Flush();


					//Move the stream back to the start position
					_responseStream.Seek(0, SeekOrigin.Begin);
					_supportedLockNavigator = new XPathDocument(_responseStream).CreateNavigator();

					XPathNodeIterator _supportedLockNodeIterator = _supportedLockNavigator.SelectDescendants("supportedlock", "DAV:", false);
					if (_supportedLockNodeIterator.MoveNext())
						_supportedLockNavigator = _supportedLockNodeIterator.Current;

					_xmlWriter.Close();
				}
				return _supportedLockNavigator;
			}
		}
		#endregion

		/// <summary>
		/// Dav Resource display name
		/// </summary>
		public string DisplayName { get; private set; }

		/// <summary>
		/// Dav Resource display name
		/// </summary>
		[ResourcePropertyAttribute("getcontentlanguage")]
		public string ContentLanguage { get; set; }


		/// <summary>
		/// Dav Resource content length
		/// </summary>
		[ResourcePropertyAttribute("getcontentlength")]
		public int ContentLength { get; set; }


		/// <summary>
		/// Resource Type
		/// </summary>
		public abstract ResourceType ResourceType { get; }


		/// <summary>
		/// Resource Path
		/// </summary>
		protected internal string ResourcePath { get; private set; }


		/// <summary>
		/// Resource Content Type
		/// </summary>
		[ResourcePropertyAttribute("getcontenttype")]
		public abstract string ContentType { get; }


		#region DateTime Parameters
		/// <summary>
		/// Dav Resource last modified datetime
		/// </summary>
		[ResourcePropertyAttribute("getlastmodified")]
		public DateTime LastModified { get; set; }

		/// <summary>
		///	Dav Resource created DateTime
		/// </summary>
		public DateTime CreationDate { get; set; }
		#endregion

	}

	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
	internal sealed class ResourcePropertyAttribute : Attribute
	{
		/// <summary>
		/// Resource Attribute
		/// </summary>
		/// <param name="enableAutoDiscovery">
		///		Specify if this property should be included when using reflection
		///	</param>
		public ResourcePropertyAttribute(bool enableAutoDiscovery) : this(enableAutoDiscovery, "") { }

		/// <summary>
		/// Resource Attribute
		/// </summary>
		/// <param name="xmlPropName"></param>
		public ResourcePropertyAttribute(string xmlPropName) : this(true, xmlPropName) { }

		/// <summary>
		/// Resource Attribute
		/// </summary>
		/// <param name="enableAutoDiscovery"></param>
		/// <param name="xmlPropName"></param>
		public ResourcePropertyAttribute(bool enableAutoDiscovery, string xmlPropName)
		{
			this.XmlPropName = xmlPropName;
			this.EnableAutoDiscovery = enableAutoDiscovery;
		}


		/// <summary>
		/// Enable Auto Discovery
		/// </summary>
		public bool EnableAutoDiscovery { get; private set; }

		/// <summary>
		/// XML property name
		/// </summary>
		public string XmlPropName { get; private set; }
	}
}