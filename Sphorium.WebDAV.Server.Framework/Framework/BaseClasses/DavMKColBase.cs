//-----------------------------------------------------------------------
// <copyright file="DavMKColBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Collection Creation Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#METHOD_MKCOL
	///			
	///		- If the resource identified by the Request-URI is non-null then the MKCOL 
	///			MUST fail with:
	///			
	///					base.AbortRequest(DavMKColResponseCode.MethodNotAllowed)
	///		
	///		
	///		- If the resource identified by the Request-URI cannot be a member of a parent 
	///			collection then the MKCOL MUST fail with:
	///			
	///					base.AbortRequest(DavMKColResponseCode.MethodNotAllowed)
	///	
	///		
	///		- All ancestors MUST already exist 
	///				
	///				For example, if a request to create collection /a/b/c/d/ is made, 
	///				and neither /a/b/ nor /a/b/c/ exists
	///		
	///		  MKCOL MUST fail with:
	///					
	///					base.AbortRequest(DavMKColResponseCode.Conflict)
	///		</code>
	///		
	///		<code>
	///			Returns DavMKColResponseCode.Created when successful
	///		</code>
	///		<seealso cref="DavMKColResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>
	public abstract class DavMKColBase : DavMethodBase
	{
		/// <summary>
		/// Dav Collection Creation Framework Base Class
		/// </summary>
		protected DavMKColBase()
		{
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavMKColBase_InternalProcessDavRequest);
		}

		/// <summary>
		/// WebDav MKCOL Response Codes
		/// </summary>
		protected enum DavMKColResponseCode : int
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
			///		Collection completed successfully
			/// </remarks>
			Created = 201,

			/// <summary>
			/// 403: Forbidden
			/// </summary>
			/// <remarks>
			///		This indicates one of two conditions: 
			///			- The server does not allow the creation of collections at the given 
			///				location in its namespace
			///			- The parent collection of the Request-URI exists but cannot accept members.
			/// </remarks>
			Forbidden = 403,

			/// <summary>
			/// 405: Method Not Allowed
			/// </summary>
			/// <remarks>
			///		Can only be executed on a deleted/non-existent resource
			/// </remarks>
			MethodNotAllowed = 405,

			/// <summary>
			/// 409: Conflict
			/// </summary>
			/// <remarks>
			///		If the parent collection does not exist, or a resource exists with the parent's
			///		name but is not a collection
			/// </remarks>
			Conflict = 409,

			/// <summary>
			/// 415: Unsupported Media Type
			/// </summary>
			/// <remarks>
			///		The server does not support the request type of the body
			/// </remarks>
			UnsupportedMediaType = 415,

			/// <summary>
			/// 507: Insufficient Storage
			/// </summary>
			/// <remarks>
			///		The resource does not have sufficient space to record the state of the 
			///		resource after the execution of this method.
			/// </remarks>
			InsufficientStorage = 507
		}

		private int DavMKColBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			return (int)DavMKColResponseCode.Created;
		}
	}
}