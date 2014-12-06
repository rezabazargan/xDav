//-----------------------------------------------------------------------
// <copyright file="DavPutBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource Put Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#METHOD_PUT
	///			
	///		- A PUT that would result in the creation of a resource without an appropriately 
	///			scoped parent collection
	///			
	///				For example, if resource /a/b/c/d.html is to be created and /a/b/c/ does not exist
	///				
	///			MUST fail with:
	///			
	///					base.AbortRequest(DavPutResponseCode.Conflict)
	///		</code>
	///		
	///		<code>
	///			Returns DavPutResponseCode.Created when successful
	///		</code>
	///		<seealso cref="DavPutResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>		
	public abstract class DavPutBase : DavMethodBase
	{
		/// <summary>
		/// Dav Resource Put Framework Base Class
		/// </summary>
		protected DavPutBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavPutBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavPutBase_InternalProcessDavRequest);
		}

		/// <summary>
		/// WebDav PUT Response Codes
		/// </summary>
		protected enum DavPutResponseCode : int
		{
			/// <summary>
			/// 0: None
			/// </summary>
			/// <remarks>
			///		Default enumerator value
			/// </remarks>
			None = 0,

			/// <summary>
			/// 201: Created
			/// </summary>
			/// <remarks>
			///		Resource completed successfully
			/// </remarks>
			Created = 201,

			/// <summary>
			/// 403: Forbidden
			/// </summary>
			/// <remarks>
			///		This indicates one of two conditions: 
			///			- The server does not allow the creation of the resource at the given 
			///				location in its namespace
			///			- The parent collection of the Request-URI exists but cannot accept members.
			/// </remarks>
			Forbidden = 403,

			/// <summary>
			/// 409: Conflict
			/// </summary>
			/// <remarks>
			///		If all ancestor collections do not exist
			/// </remarks>
			Conflict = 409,

			/// <summary>
			/// 423: Resource Locked
			/// </summary>
			/// <remarks>
			///		If the resource is already locked with an exclusive lock or if the resource
			///		is already locked with a shared lock and the client requests and exclusive lock
			/// </remarks>
			Locked = 423,

			/// <remarks>
			///		The resource does not have sufficient space to record the state of the 
			///		resource after the execution of this method.
			/// </remarks>
			InsufficientStorage = 507
		}

		/// <summary>
		/// Request content type
		/// </summary>
		protected string ContentType
		{
			get
			{
				return base.HttpApplication.Request.ContentType;
			}
		}

		/// <summary>
		/// Check to see if an existing resource should be overwritten
		/// </summary>
		protected bool OverwriteExistingResource
		{
			get
			{
				if (base.HttpApplication.Request.Headers["If-None-Match"] != null)
					return false;

				return true;
			}
		}

		/// <summary>
		/// Request input 
		/// </summary>
		protected byte[] GetRequestInput()
		{
			StreamReader _inputStream = new StreamReader(base.HttpApplication.Request.InputStream);

			long _inputSize = _inputStream.BaseStream.Length;

			byte[] _inputBytes = new byte[_inputSize];
			_inputStream.BaseStream.Read(_inputBytes, 0, (int)_inputSize);
			return _inputBytes;
		}

		private int DavPutBase_ValidateDavRequest(object sender, EventArgs e)
		{
			//if (base.RequestLength == 0)
			//	return (int)ServerResponseCode.BadRequest;

			return (int)ServerResponseCode.Ok;
		}

		private int DavPutBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			//return (int)DavPutResponseCode.Created;
			return (int)ServerResponseCode.Ok;
		}
	}
}