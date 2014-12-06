//-----------------------------------------------------------------------
// <copyright file="DavPropertyCollection.cs" company="Sphorium Technologies">
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
	/// WebDav Property Collection.
	/// </summary>
	[Serializable]
	public class DavPropertyCollection : Collection<DavProperty>, ICloneable
	{
		/// <summary>
		/// WebDav Property Collection.
		/// </summary>
		public DavPropertyCollection() { }

		/// <summary>
		/// Retrieve a property by name
		/// </summary>
		public DavProperty this[string propertyName]
		{
			get
			{
				DavProperty _property = null;
				foreach (DavProperty _item in base.Items)
				{
					if (String.Equals(_item.Name, propertyName, StringComparison.InvariantCultureIgnoreCase))
					{
						_property = _item;
						break;
					}
				}

				return _property;
			}
		}

		/// <summary>
		/// Removes a property from the collection by name
		/// </summary>
		/// <param name="propertyName"></param>
		public void Remove(string propertyName)
		{
			for (int i = 0; i < base.Count; i++)
			{
				DavProperty _item = this[i];
				if (String.Equals(_item.Name, propertyName, StringComparison.InvariantCultureIgnoreCase))
				{
					base.RemoveAt(i);
					break;
				}
			}
		}

		/// <summary>
		/// Retrieve all the available collection property names
		/// </summary>
		public List<string> GetPropertyNames()
		{
			List<string> _propNames = new List<string>();
			foreach (DavProperty _item in base.Items)
				_propNames.Add(_item.Name);

			return _propNames;
		}

		/// <summary>
		/// Copy an existing property collection
		/// </summary>
		/// <param name="propertyCollection"></param>
		public void Copy(DavPropertyCollection propertyCollection)
		{
			if (propertyCollection == null)
				throw new ArgumentNullException("propertyCollection", InternalFunctions.GetResourceString("ArgumentNullException", "PropertyCollection"));

			base.Clear();
			foreach (DavProperty _davProperty in propertyCollection)
			{
				if (_davProperty.Name != null)
					this.Add(_davProperty.Clone());
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
		public DavPropertyCollection Clone()
		{
			// Start with a flat, memberwise copy
			DavPropertyCollection _davPropertyCollection = new DavPropertyCollection();

			// Then deep-copy everything that needs the 
			foreach (DavProperty _davProperty in this)
				_davPropertyCollection.Add(_davProperty.Clone());

			return _davPropertyCollection;
		}
		#endregion
	}
}


