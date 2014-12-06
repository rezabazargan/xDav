//-----------------------------------------------------------------------
// <copyright file="DavCopyBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource Copy Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#METHOD_COPY
	///			
	///		- When the COPY method has completed processing it MUST have created a consistent namespace 
	///			at the destination. However, if an error occurs while copying an internal collection, 
	///			the server MUST NOT copy any resources identified by members of this collection 
	///			(i.e., the server must skip this subtree)
	///			
	///				For example, if resource /a/b/c/d.html is to be created and /a/b/c/ does not exist
	///				
	///			MUST fail with:
	///			
	///					base.AbortRequest(DavCopyResponseCode.Conflict)
	///		</code>
	///		
	///		<code>
	///			Returns DavCopyResponseCode.Created or DavCopyResponseCode.NoContent when successful
	///		</code>
	///		<seealso cref="DavCopyResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>	
	[Serializable]
	public abstract class DavCopyBase : DavCopyMoveBase
	{
		/// <summary>
		/// Dav Resource Copy Framework Base Class
		/// </summary>
		protected DavCopyBase() { }

		/// <summary>
		/// WebDav COPY Response Codes
		/// </summary>
		protected enum DavCopyResponseCode : int
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
			///		The source resource was successfully copied. 
			///		The copy operation resulted in the creation of a new resource.
			/// </remarks>
			Created = 201,

			/// <summary>
			/// 204: No Content
			/// </summary>
			/// <remarks>
			///		The source resource was successfully copied to a pre-existing destination resource.
			/// </remarks>
			NoContent = 204,

			/// <summary>
			/// 403: Forbidden
			/// </summary>
			/// <remarks>
			///		The source and destination URIs are the same
			/// </remarks>
			/// 
			Forbidden = 403,

			/// <summary>
			/// 409: Conflict
			/// </summary>
			/// <remarks>
			///		A resource cannot be created at the destination until one or more intermediate 
			///		collections have been created.
			/// </remarks>
			Conflict = 409,

			/// <summary>
			/// 412: Precondition Failed
			/// </summary>
			/// <remarks>
			///		The server was unable to maintain the liveness of the properties listed in the 
			///		propertybehavior XML element or the Overwrite header is "F" and the state of 
			///		the destination resource is non-null.
			/// </remarks>
			PreconditionFailed = 412,

			/// <summary>
			/// 423: Locked
			/// </summary>
			/// <remarks>
			///		The destination resource was locked.
			/// </remarks>
			Locked = 423,

			/// <summary>
			/// 502: Bad Gateway
			/// </summary>
			/// <remarks>
			///		This may occur when the destination is on another server 
			///		and the destination server refuses to accept the resource.
			/// </remarks>
			BadGateway = 502,

			/// <summary>
			/// 507: Insufficient Storage
			/// </summary>
			/// <remarks>
			///		The destination resource does not have sufficient space to record 
			///		the state of the resource after the execution of this method.
			/// </remarks>
			InsufficientStorage = 507
		}
	}
}