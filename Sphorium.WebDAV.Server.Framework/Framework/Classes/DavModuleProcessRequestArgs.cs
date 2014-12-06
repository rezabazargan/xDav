//-----------------------------------------------------------------------
// <copyright file="DavModuleProcessRequestArgs.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.Classes
{
	/// <summary>
	/// WebDav Module Process Request event arguments
	/// </summary>
	public sealed class DavModuleProcessRequestArgs : EventArgs
	{
		private bool __processRequest;
		private readonly Uri __requestUri;

		/// <summary>
		/// WebDav Module Process Request event arguments
		/// </summary>
		/// <param name="requestUri"></param>
		/// <param name="processRequest"></param>
		public DavModuleProcessRequestArgs(Uri requestUri, bool processRequest)
		{
			this.__requestUri = requestUri;
			this.__processRequest = processRequest;
		}

		/// <summary>
		/// Request Uri
		/// </summary>
		public Uri RequestUri
		{
			get
			{
				return this.__requestUri;
			}
		}

		/// <summary>
		/// Argument Lock Timeout
		/// </summary>
		public bool ProcessRequest
		{
			get
			{
				return this.__processRequest;
			}
			set
			{
				this.__processRequest = value;
			}
		}
	}
}
