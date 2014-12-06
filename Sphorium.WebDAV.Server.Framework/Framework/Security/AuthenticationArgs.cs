//-----------------------------------------------------------------------
// <copyright file="AuthenticationArgs.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.Security
{
	/// <summary>
	/// Authentication Type
	/// </summary>
	public enum Authentication
	{
		/// <summary>
		/// No Authentication
		/// </summary>
		None = 0,

		/// <summary>
		/// Basic Authentication
		/// </summary>
		Basic = 1,

		/// <summary>
		/// Digest Authentication
		/// </summary>
		Digest = 2
	}

	/// <summary>
	/// Authentication event arguments
	/// </summary>
	public sealed class AuthenticationArgs : EventArgs
	{
		/// <summary>
		/// Authentication event arguments
		/// </summary>
		/// <param name="requestUri"></param>
		/// <param name="realm"></param>
		/// <param name="authType"></param>
		public AuthenticationArgs(Uri requestUri, string realm, Authentication authType)
		{
			this.Realm = realm;
			this.AuthType = authType;
			this.RequestUri = requestUri;
			this.ProcessAuthorization = true;
		}

		/// <summary>
		/// Requested Uri
		/// </summary>
		public Uri RequestUri { get; private set; }

		/// <summary>
		/// Authentication Type
		/// </summary>
		public Authentication AuthType { get; private set; }

		/// <summary>
		/// Authentication Realm
		/// </summary>
		public string Realm { get; set; }

		/// <summary>
		/// Process Authorization
		/// </summary>
		public bool ProcessAuthorization { get; set; }
	}
}
