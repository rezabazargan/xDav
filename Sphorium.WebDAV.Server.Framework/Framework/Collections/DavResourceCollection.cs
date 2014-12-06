//-----------------------------------------------------------------------
// <copyright file="DavResourceCollection.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;

using Sphorium.WebDAV.Server.Framework.BaseClasses;

namespace Sphorium.WebDAV.Server.Framework.Collections
{
	/// <summary>
	/// WebDav Resource Collection.
	/// </summary>
	[Serializable]
	public class DavResourceCollection : Collection<DavResourceBase>
	{
		/// <summary>
		/// WebDav Lock Property Collection.
		/// </summary>
		public DavResourceCollection() { }
	}
}


