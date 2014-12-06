//-----------------------------------------------------------------------
// <copyright file="DigestAuthorization.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.Security
{
	/// <summary>
	/// Digest Authorization event arguments
	/// </summary>
	public sealed class DigestAuthorizationArgs : EventArgs
	{
		private string __nonce;
		private bool __staleNonce;
		private string __password;
		private readonly string __realm;
		private readonly string __userName;

		/// <summary>
		/// Digest Authorization event arguments
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="realm"></param>
		public DigestAuthorizationArgs(string userName, string realm)
		{
			this.__userName = userName;
			this.__realm = realm;
		}

		/// <summary>
		/// User Name
		/// </summary>
		public string UserName
		{
			get
			{
				return this.__userName;
			}
		}

		/// <summary>
		/// Password
		/// </summary>
		/// <remarks>
		/// Required for digest authorization
		/// </remarks>
		public string Password
		{
			get
			{
				return this.__password;
			}
			set
			{
				this.__password = value;
			}
		}

		/// <summary>
		/// Nonce
		/// </summary>
		/// <remarks>
		/// Update to apply a custom nonce
		/// </remarks>
		public string Nonce
		{
			get
			{
				return this.__nonce;
			}
			set
			{
				this.__nonce = value;
			}
		}

		/// <summary>
		/// Nonce Is Stale
		/// </summary>
		/// <remarks>
		/// Update to require a new nonce
		/// </remarks>
		public bool StaleNonce
		{
			get
			{
				return this.__staleNonce;
			}
			set
			{
				this.__staleNonce = value;
			}
		}

		/// <summary>
		/// Realm
		/// </summary>
		public string Realm
		{
			get
			{
				return this.__realm;
			}
		}
	}
}
