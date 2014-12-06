//-----------------------------------------------------------------------
// <copyright file="DavProperty.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Xml;
using System.Xml.XPath;

using Sphorium.WebDAV.Server.Framework.BaseClasses;
using Sphorium.WebDAV.Server.Framework.Collections;

namespace Sphorium.WebDAV.Server.Framework.Classes
{
	/// <summary>
	/// WebDav Property.
	/// </summary>
	[Serializable]
	public class DavProperty : DavPropertyBase, ICloneable
	{
		private DavPropertyCollection __nestedProperties = new DavPropertyCollection();
		private DavPropertyAttributeCollection __attributes = new DavPropertyAttributeCollection();

		/// <summary>
		/// WebDav Property.
		/// </summary>
		public DavProperty() { }

		/// <summary>
		/// WebDav Property.
		/// </summary>
		/// <param name="property"></param>
		public DavProperty(XPathNavigator property)
		{
			if (property == null)
				throw new ArgumentNullException("property", InternalFunctions.GetResourceString("ArgumentNullException", "Property"));
			else if (property.NodeType != XPathNodeType.Element)
				throw new ArgumentException(InternalFunctions.GetResourceString("XPathNavigatorElementArgumentException", "Property"), "property");

			base.Name = property.LocalName;
			base.Namespace = property.NamespaceURI;

			if (property.HasAttributes)
			{
				//TODO: Support element attributes
				//string _here = "";
				//Add the attributes first
				//			foreach (XmlAttribute _xmlAttribute in property.Attributes)
				//				Attributes.Add(new DavPropertyAttribute(_xmlAttribute));
			}

			if (property.MoveToFirstChild())
			{
				if (property.NodeType == XPathNodeType.Element)
				{
					NestedProperties.Add(new DavProperty(property.Clone()));

					while (property.MoveToNext())
					{
						if (property.NodeType == XPathNodeType.Element)
							NestedProperties.Add(new DavProperty(property.Clone()));
					}
				}
				else if (property.NodeType == XPathNodeType.Text)
				{
					base.Value = property.Value;
					property.MoveToParent();
				}
			}
		}

		/// <summary>
		/// WebDav Property.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="propertyValue"></param>
		/// <param name="propertyNamespace"></param>
		public DavProperty(string propertyName, string propertyValue, string propertyNamespace)
		{
			base.Name = propertyName;
			base.Value = propertyValue;
			base.Namespace = propertyNamespace;
		}

		/// <summary>
		/// WebDav Property
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="propertyNamespace"></param>
		public DavProperty(string propertyName, string propertyNamespace)
		{
			base.Name = propertyName;
			base.Namespace = propertyNamespace;
		}

		/// <summary>
		/// Nested Dav Properties
		/// </summary>
		public DavPropertyCollection NestedProperties
		{
			get
			{
				return this.__nestedProperties;
			}
		}

		/// <summary>
		/// Nested Dav Properties
		/// </summary>
		public DavPropertyAttributeCollection Attributes
		{
			get
			{
				return this.__attributes;
			}
		}

		internal void ToXML(XmlTextWriter xmlWriter)
		{
			xmlWriter.WriteStartElement(base.Name, base.Namespace);

			foreach (DavPropertyAttribute _propertyAttribute in Attributes)
			{
				if (_propertyAttribute.XPathAttribute != null)
					xmlWriter.WriteAttributeString(_propertyAttribute.AttributeName, _propertyAttribute.AttributeNamespace, _propertyAttribute.AttributeValue);
				else
					xmlWriter.WriteAttributeString(_propertyAttribute.AttributeNamespace + ":" + _propertyAttribute.AttributeName, _propertyAttribute.AttributeValue);
			}

			if (base.Value != null && base.Value.Length != 0)
				xmlWriter.WriteString(base.Value);

			//Append all the nested elements
			foreach (DavProperty _nestedProperty in NestedProperties)
				_nestedProperty.ToXML(xmlWriter);

			//Close the prop element section
			xmlWriter.WriteEndElement();
		}

		#region ICloneable Members
		// Explicit interface method impl
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// DavProperty Clone
		/// </summary>
		/// <remarks>Deep copy</remarks>
		/// <returns></returns>
		public DavProperty Clone()
		{
			// Start with a flat, memberwise copy
			DavProperty _davProperty = (DavProperty)this.MemberwiseClone();

			// Then deep-copy everything that needs the 
			_davProperty.__attributes = this.Attributes.Clone();
			_davProperty.__nestedProperties = this.NestedProperties.Clone();

			return _davProperty;
		}
		#endregion
	}
}


