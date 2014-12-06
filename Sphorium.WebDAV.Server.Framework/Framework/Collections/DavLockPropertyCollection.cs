//-----------------------------------------------------------------------
// <copyright file="DavLockPropertyCollection.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;

using Sphorium.WebDAV.Server.Framework.Classes;

namespace Sphorium.WebDAV.Server.Framework.Collections
{
	/// <summary>
	/// WebDav Lock Property Collection.
	/// </summary>
	[Serializable]
	public class DavLockPropertyCollection : Collection<DavLockProperty>, ICloneable
	{
		/// <summary>
		/// WebDav Lock Property Collection.
		/// </summary>
		public DavLockPropertyCollection() { }

		#region ICloneable Members
		// Explicit interface method impl
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// DavLockPropertyCollection Clone
		/// </summary>
		/// <remarks>Deep copy</remarks>
		/// <returns></returns>
		public DavLockPropertyCollection Clone()
		{
			// Start with a flat, memberwise copy
			DavLockPropertyCollection _davLockPropertyCollection = new DavLockPropertyCollection();

			// Then deep-copy everything that needs the 
			foreach (DavLockProperty _davLockProperty in this)
				_davLockPropertyCollection.Add(_davLockProperty.Clone());

			return _davLockPropertyCollection;
		}
		#endregion
	}
}


