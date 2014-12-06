//-----------------------------------------------------------------------
// <copyright file="DavResourceVersionCollection.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;

using Sphorium.WebDAV.Server.Framework.Classes;

namespace Sphorium.WebDAV.Server.Framework.Collections
{
	/// <summary>
	/// WebDav Resource Version Collection.
	/// </summary>
	[Serializable]
	public class DavResourceVersionCollection : Collection<DavResourceVersion>, ICloneable
	{
		/// <summary>
		/// WebDav Lock Property Collection.
		/// </summary>
		public DavResourceVersionCollection() { }

		#region ICloneable Members
		// Explicit interface method impl
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// DavResourceVersionCollection Clone
		/// </summary>
		/// <remarks>Deep copy</remarks>
		/// <returns></returns>
		public DavResourceVersionCollection Clone()
		{
			// Start with a flat, memberwise copy
			DavResourceVersionCollection _davResourceVersionCollection = new DavResourceVersionCollection();

			// Then deep-copy everything that needs the 
			foreach (DavResourceVersion _davResourceVersion in this)
				_davResourceVersionCollection.Add(_davResourceVersion.Clone());

			return _davResourceVersionCollection;
		}
		#endregion
	}
}


