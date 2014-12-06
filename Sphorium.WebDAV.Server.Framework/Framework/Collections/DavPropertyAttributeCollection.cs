//-----------------------------------------------------------------------
// <copyright file="DavPropertyAttributeCollection.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Sphorium.WebDAV.Server.Framework.Classes;

namespace Sphorium.WebDAV.Server.Framework.Collections
{
	/// <summary>
	/// WebDav Property Attribute Collection.
	/// </summary>
	[Serializable]
	public class DavPropertyAttributeCollection : Collection<DavPropertyAttribute>, ICloneable
	{
		/// <summary>
		/// WebDav Property Attribute Collection.
		/// </summary>
		public DavPropertyAttributeCollection() { }

		/// <summary>
		/// Retrieve a property attribute by name
		/// </summary>
		public DavPropertyAttribute this[string propertyAttributeName]
		{
			get
			{
				DavPropertyAttribute _propertyAttribute = null;
				foreach (DavPropertyAttribute _item in base.Items)
				{
					if (String.Equals(_item.AttributeName, propertyAttributeName, StringComparison.InvariantCultureIgnoreCase))
					{
						_propertyAttribute = _item;
						break;
					}
				}

				return _propertyAttribute;
			}
		}

		/// <summary>
		/// Retrieve all the available collection property attributenames
		/// </summary>
		public List<string> GetAllPropertyAttributeNames()
		{
			List<string> _propAttribNames = new List<string>();
			foreach (DavPropertyAttribute _item in base.Items)
				_propAttribNames.Add(_item.AttributeName);

			return _propAttribNames;
		}


		#region ICloneable Members
		// Explicit interface method impl
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// DavPropertyAttributeCollection Clone
		/// </summary>
		/// <remarks>Deep copy</remarks>
		/// <returns></returns>
		public DavPropertyAttributeCollection Clone()
		{
			// Start with a flat, memberwise copy
			DavPropertyAttributeCollection _davPropAttribCollection = new DavPropertyAttributeCollection();

			// Then deep-copy everything that needs the 
			foreach (DavPropertyAttribute _davPropertyAttribute in this)
				_davPropAttribCollection.Add(_davPropertyAttribute.Clone());

			return _davPropAttribCollection;
		}
		#endregion
	}
}


