//-----------------------------------------------------------------------
// <copyright file="WebDavException.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Sphorium.WebDAV.Server.Framework
{
	/// <summary>
	/// WebDav Server Framework Exception.
	/// </summary>
	[Serializable]
	public class WebDavException : Exception
	{
		/// <summary>
		/// WebDav Server Framework Exception.
		/// </summary>
		public WebDavException() { }

		/// <summary>
		/// WebDav Server Framework Exception.
		/// </summary>
		/// <param name="message"></param>
		public WebDavException(string message) : base(message) { }

		/// <summary>
		/// WebDav Server Framework Exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public WebDavException(string message, Exception innerException) : base(message, innerException) { }

		/// <summary>
		/// WebDav Server Framework Exception.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected WebDavException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
