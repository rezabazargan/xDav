//-----------------------------------------------------------------------
// <copyright file="Enumerators.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework
{
	#region Lock / Unlock
	/// <summary>
	/// Lock owner type
	/// </summary>
	public enum LockOwnerType : int
	{
		/// <summary>
		/// User
		/// </summary>
		User,

		/// <summary>
		/// URI Lock owner
		/// </summary>
		Href
	}

	/// <summary>
	/// Lock Type
	/// </summary>
	public enum LockType : int
	{
		/// <summary>
		/// Read lock
		/// </summary>
		Read,

		/// <summary>
		/// Write lock
		/// </summary>
		Write
	}

	/// <summary>
	/// Lock Scope
	/// </summary>
	public enum LockScope : int
	{
		/// <summary>
		/// Shared lock
		/// </summary>
		Shared,

		/// <summary>
		/// Exclusive lock
		/// </summary>
		Exclusive
	}
	#endregion

	/// <summary>
	/// WebDav Depth
	/// </summary>
	public enum DepthType : int
	{
		/// <summary>
		/// The method is applied only to the resource
		/// </summary>
		ResourceOnly = 0,

		/// <summary>
		/// The method is applied to the resource and to its immediate children
		/// </summary>
		ResourceChildren = 1,

		/// <summary>
		/// The method is applied to the resource and to all of its children
		/// </summary>
		Infinity = 2
	}

	/// <summary>
	/// WebDav Property Handling Behavior
	/// </summary>
	/// <remarks>
	///		Used by MOVE / COPY
	///	</remarks>
	public enum PropertyBehavior : int
	{
		/// <summary>
		/// Omit
		/// </summary>
		/// <remarks>
		///		The omit XML element instructs the server that it should use best effort to 
		///		copy properties but a failure to copy a property MUST NOT cause the method to fail.
		/// </remarks>
		Omit,

		/// <summary>
		/// Keep Alive
		/// </summary>
		/// <remarks>
		///		Specifies requirements for the copying/moving of live properties.
		/// </remarks>
		KeepAlive
	}

	/// <summary>
	/// WebDav Resource Type
	/// </summary>
	public enum ResourceType : int
	{
		/// <summary>
		/// Collection Resource
		/// </summary>
		Collection,

		/// <summary>
		/// File Resource
		/// </summary>
		Resource
	}
}
