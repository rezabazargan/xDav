//-----------------------------------------------------------------------
// <copyright file="DavPropertyBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// WebDav Property Base Class.
	/// </summary>
	[Serializable]
	public abstract class DavPropertyBase
	{
		/// <summary>
		/// WebDav Property Base Class.
		/// </summary>
		protected DavPropertyBase()
		{
			this.Name = string.Empty;
			this.Namespace = string.Empty;
			this.Value = string.Empty;
		}

		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Namespace
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// Value
		/// </summary>
		public string Value { get; set; }
	}
}