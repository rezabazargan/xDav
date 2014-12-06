//-----------------------------------------------------------------------
// <copyright file="BasicAuthorizationArgs.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.Security
{
	/// <summary>
	/// Basic Authorization event arguments
	/// </summary>
	public sealed class BasicAuthorizationArgs : EventArgs
	{
		private readonly string __realm;
		private readonly string __userName;
		private readonly string __password;
		private bool __authorized = false;

		/// <summary>
		/// Basic Authorization event arguments
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="realm"></param>
		public BasicAuthorizationArgs(string userName, string password, string realm)
		{
			this.__userName = userName;
			this.__password = password;
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
		public string Password
		{
			get
			{
				return this.__password;
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

		/// <summary>
		/// Authorized 
		/// </summary>
		/// <value>
		/// TRUE if the request is authorized
		/// </value>
		public bool Authorized
		{
			get
			{
				return this.__authorized;
			}
			set
			{
				this.__authorized = value;
			}
		}
	}
}
