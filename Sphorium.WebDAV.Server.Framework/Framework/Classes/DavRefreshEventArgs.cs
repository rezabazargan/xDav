//-----------------------------------------------------------------------
// <copyright file="DavRefreshEventArgs.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.Classes
{
	/// <summary>
	/// WebDav Lock Refresh event arguments
	/// </summary>
	public sealed class DavRefreshEventArgs : EventArgs, ICloneable
	{
		private int __lockTimeout;
		private string __lockToken = "";

		/// <summary>
		/// WebDav Lock Refresh event arguments
		/// </summary>
		/// <param name="lockToken"></param>
		/// <param name="lockTimeout"></param>
		public DavRefreshEventArgs(string lockToken, int lockTimeout)
		{
			this.__lockToken = lockToken;
			this.__lockTimeout = lockTimeout;
		}

		/// <summary>
		/// Argument Lock Token
		/// </summary>
		public string LockToken
		{
			get
			{
				return this.__lockToken;
			}
		}

		/// <summary>
		/// Argument Lock Timeout
		/// </summary>
		public int LockTimeout
		{
			get
			{
				return this.__lockTimeout;
			}
		}

		#region ICloneable Members
		// Explicit interface method impl
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// DavRefreshEventArgs Clone
		/// </summary>
		/// <remarks>Deep copy</remarks>
		/// <returns></returns>
		public DavRefreshEventArgs Clone()
		{
			// Start with a flat, memberwise copy
			DavRefreshEventArgs _davRefreshEventArgs = (DavRefreshEventArgs)this.MemberwiseClone();
			return _davRefreshEventArgs;
		}
		#endregion
	}
}
