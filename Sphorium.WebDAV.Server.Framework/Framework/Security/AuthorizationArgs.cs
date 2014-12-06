//-----------------------------------------------------------------------
// <copyright file="AuthorizationArgs.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.Security
{
	/// <summary>
	/// Authentication event arguments
	/// </summary>
	public sealed class AuthorizationArgs : EventArgs
	{
		/// <summary>
		/// Authorization event arguments
		/// </summary>
		/// <param name="authArgs"></param>
		public AuthorizationArgs(AuthenticationArgs authArgs)
		{
			this.AuthenticationInfo = authArgs;
		}

		/// <summary>
		/// Authentication Info
		/// </summary>
		public AuthenticationArgs AuthenticationInfo { get; private set; }

		/// <summary>
		/// UserName
		/// </summary>
		public string UserName { get; internal set; }

		/// <summary>
		/// Request Authorization
		/// </summary>
		public bool RequestAuthorized { get; internal set; }
	}
}
