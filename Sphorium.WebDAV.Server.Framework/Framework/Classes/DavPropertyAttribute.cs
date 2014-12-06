//-----------------------------------------------------------------------
// <copyright file="DavPropertyAttribute.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Xml.XPath;

namespace Sphorium.WebDAV.Server.Framework.Classes
{
	/// <summary>
	/// WebDav Property Attribute.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.All)]
	public sealed class DavPropertyAttribute : System.Attribute, ICloneable
	{
		private string __attributeName;
		private string __attributeValue;
		private string __attributeNamespace;

		[NonSerializedAttribute]
		private XPathNavigator __xPathAttribute;

		/// <summary>
		/// WebDav Property Attribute.
		/// </summary>
		public DavPropertyAttribute() { }

		/// <summary>
		/// WebDav Property Attribute.
		/// </summary>
		/// <param name="xPathAttribute"></param>
		public DavPropertyAttribute(XPathNavigator xPathAttribute)
		{
			if (xPathAttribute == null)
				throw new ArgumentNullException("xPathAttribute", InternalFunctions.GetResourceString("ArgumentNullException", "XPathAttribute"));
			else if (xPathAttribute.NodeType != XPathNodeType.Element)
				throw new ArgumentException(InternalFunctions.GetResourceString("XPathNavigatorElementArgumentException", "xPathAttribute"), "XPathAttribute");

			this.__xPathAttribute = xPathAttribute;
			this.AttributeName = xPathAttribute.LocalName;
			this.AttributeValue = xPathAttribute.Value;
			this.AttributeNamespace = xPathAttribute.NamespaceURI;
		}

		/// <summary>
		///		Object base XPathAttribute
		/// </summary>
		/// <remarks>
		///		This will be null if the DavPropertyAttribute(XPathNavigator xPathAttribute) 
		///		constructor was not called
		///	</remarks>
		public XPathNavigator XPathAttribute
		{
			get
			{
				if (this.__xPathAttribute != null)
					return this.__xPathAttribute.Clone();

				return null;
			}
		}

		/// <summary>
		/// Attribute Name
		/// </summary>
		public string AttributeName
		{
			get
			{
				if (this.__attributeName == null)
					this.__attributeName = "";

				return this.__attributeName;
			}
			set
			{
				this.__attributeName = value;
			}
		}

		/// <summary>
		/// Attribute Namespace
		/// </summary>
		public string AttributeNamespace
		{
			get
			{
				if (this.__attributeNamespace == null)
					this.__attributeNamespace = "";

				return this.__attributeNamespace;
			}
			set
			{
				this.__attributeNamespace = value;
			}
		}

		/// <summary>
		/// Attribute Value
		/// </summary>
		public string AttributeValue
		{
			get
			{
				if (this.__attributeValue == null)
					this.__attributeValue = "";

				return this.__attributeValue;
			}
			set
			{
				this.__attributeValue = value;
			}
		}

		#region ICloneable Members
		// Explicit interface method impl
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// DavPropertyAttribute Clone
		/// </summary>
		/// <remarks>Deep copy</remarks>
		/// <returns></returns>
		public DavPropertyAttribute Clone()
		{
			// Start with a flat, memberwise copy
			DavPropertyAttribute _davPropertyAttribute = (DavPropertyAttribute)this.MemberwiseClone();

			_davPropertyAttribute.__attributeName = this.AttributeName;
			_davPropertyAttribute.__attributeNamespace = this.AttributeNamespace;
			_davPropertyAttribute.__attributeValue = this.AttributeValue;

			_davPropertyAttribute.__xPathAttribute = this.XPathAttribute;
			return _davPropertyAttribute;
		}
		#endregion
	}
}


