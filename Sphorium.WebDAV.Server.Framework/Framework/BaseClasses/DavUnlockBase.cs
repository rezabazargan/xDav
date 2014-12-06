//-----------------------------------------------------------------------
// <copyright file="DavUnlockBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource Unlock Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#METHOD_UNLOCK
	///			
	///		- The UNLOCK method removes the lock identified by the lock token in the Lock-Token 
	///		request header from the Request-URI, and all other resources included in the lock. 
	///		If all resources which have been locked under the submitted lock token can not be 
	///		unlocked then the UNLOCK request MUST fail.
	///		</code>
	///		
	///		<code>
	///			Returns DavUnlockResponseCode.NoContent when successful
	///		</code>
	///		<seealso cref="DavUnlockResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>
	public abstract class DavUnlockBase : DavMethodBase
	{
		/// <summary>
		/// Dav Resource Unlock Framework Base Class
		/// </summary>
		protected DavUnlockBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavUnlockBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavUnlockBase_InternalProcessDavRequest);
		}

		/// <summary>
		/// WebDav UNLOCK Response Codes
		/// </summary>
		protected enum DavUnlockResponseCode : int
		{
			/// <summary>
			/// 0: None
			/// </summary>
			/// <remarks>
			///		Default enumerator value
			/// </remarks>
			None = 0,

			/// <summary>
			/// 204: NoContent
			/// </summary>
			/// <remarks>The unlock command completed successfully</remarks>
			NoContent = 204,

			/// <summary>
			/// 400: Bad Request
			/// </summary>
			/// <example>If the client does not provide a lock token</example>
			BadRequest = 400,


			/// <summary>
			/// 401: Unauthorized
			/// </summary>
			/// <example>If the client is not authorized to unlock the resource</example>
			Unauthorized = 401,

			/// <summary>
			/// 412: Precondition Failed
			/// </summary>
			/// <example>
			///		If the client provides a lock token to unlock a resource that isn't locked or
			///		provides an incorrect lock token
			///	</example>
			PreconditionFailed = 412
		}

		/// <summary>
		/// Lock Token
		/// </summary>
		protected string LockToken { get; private set; }

		private int DavUnlockBase_ValidateDavRequest(object sender, EventArgs e)
		{
			int _returnCode = (int)ServerResponseCode.Ok;

			if (base.RequestLength != 0)
				_returnCode = (int)ServerResponseCode.BadRequest;

			else if (HttpApplication.Request.Headers["Lock-Token"] == null)
				_returnCode = (int)DavUnlockResponseCode.BadRequest;

			else
				this.LockToken = InternalFunctions.ParseOpaqueLockToken(base.HttpApplication.Request.Headers["Lock-Token"]);

			return _returnCode;
		}

		private int DavUnlockBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			return (int)DavUnlockResponseCode.NoContent;
		}
	}
}