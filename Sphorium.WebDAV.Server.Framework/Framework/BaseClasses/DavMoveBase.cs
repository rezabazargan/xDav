//-----------------------------------------------------------------------
// <copyright file="DavMoveBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource Move Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#METHOD_MOVE
	///			
	///		- When the MOVE method has completed processing it MUST have created a consistent namespace 
	///			at the destination. However, if an error occurs while moving an internal collection, 
	///			the server MUST NOT move any resources identified by members of the failed collection
	///			(i.e., the server must skip this subtree)
	///			
	///				For example, if an infinite depth move is performed on collection /a/, 
	///				which contains collections /a/b/ and /a/c/, and an error occurs moving /a/b/, an 
	///				attempt should still be made to try moving /a/c/. 
	///				
	///				Similarly, after encountering an error moving a non-collection resource as part 
	///				of an infinite depth move, the server SHOULD try to finish as much of the original 
	///				move operation as possible.
	///		</code>
	///		
	///		<code>
	///			Returns DavMoveResponseCode.Created or DavMoveResponseCode.NoContent when successful
	///		</code>
	///		<seealso cref="DavMoveResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>	
	public abstract class DavMoveBase : DavCopyMoveBase
	{
		/// <summary>
		/// Dav Resource Move Framework Base Class
		/// </summary>
		protected DavMoveBase() { }

		/// <summary>
		/// WebDav MOVE Response Codes
		/// </summary>
		protected enum DavMoveResponseCode : int
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
			///		The source resource was successfully moved, and a new resource was created 
			///		at the destination.
			/// </remarks>
			Created = 201,

			/// <summary>
			/// 204: No Content
			/// </summary>
			/// <remarks>
			///		The source resource was successfully moved to a pre-existing destination resource.
			/// </remarks>
			NoContent = 204,

			/// <summary>
			/// 403: Forbidden
			/// </summary>
			/// <remarks>
			///		The source and destination URIs are the same
			/// </remarks>
			Forbidden = 403,

			/// <summary>
			/// 409: Conflict
			/// </summary>
			/// <remarks>
			///		A resource cannot be created at the destination until one or more intermediate 
			///		collections have been created
			/// </remarks>
			Conflict = 409,

			/// <summary>
			/// 412: Precondition Failed
			/// </summary>
			/// <remarks>
			///		The server was unable to maintain the liveness of the properties listed in 
			///		the propertybehavior XML element or the Overwrite header is "F" and the state 
			///		of the destination resource is non-null.
			/// </remarks>
			PreconditionFailed = 412,

			/// <summary>
			/// 423: Locked
			/// </summary>
			/// <remarks>
			///		The source or the destination resource was locked.
			/// </remarks>
			Locked = 423,

			/// <summary>
			/// 502: Bad Gateway
			/// </summary>
			/// <remarks>
			///		This may occur when the destination is on another server 
			///		and the destination server refuses to accept the resource.
			/// </remarks>
			BadGateway = 502
		}
	}
}