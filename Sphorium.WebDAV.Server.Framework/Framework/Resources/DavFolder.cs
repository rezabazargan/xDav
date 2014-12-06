//-----------------------------------------------------------------------
// <copyright file="DavFolder.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Sphorium.WebDAV.Server.Framework.BaseClasses;

namespace Sphorium.WebDAV.Server.Framework.Resources
{
	/// <summary>
	/// Dav Folder Resource Framework Base Class
	/// </summary>
	public class DavFolder : DavResourceBase
	{
		/// <summary>
		/// Initializes a new instance of the DavFolder class
		/// </summary>
		/// <param name="displayName">Resource display name</param>
		/// <param name="folderPath">Resource path</param>
		public DavFolder(string displayName, string folderPath)
			: base(displayName, folderPath)
		{
			this.IsHidden = "0";
		}

		/// <summary>
		/// Dav Folder Content Type
		/// </summary>
		public override string ContentType
		{
			get
			{
				////base.ContentType = "application/webdav-collection";
				return "application/octet-stream";
			}
		}

		/// <summary>
		/// Dav Resource Type
		/// </summary>
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.Collection;
			}
		}

		/// <summary>
		/// Gets the collection type
		/// </summary>
		/// <remarks>Will always return 1</remarks>
		public string IsCollection
		{
			get
			{
				return "1";
			}
		}

		/// <summary>
		/// Gets or sets the IsHidden
		/// </summary>
		public string IsHidden
		{
			get; set;
		}

		/// <summary>
		/// Gets the IsFolder property
		/// </summary>
		[ResourcePropertyAttribute("isFolder")]
		public string IsFolder
		{
			get
			{
				return "t";
			}
		}

		/// <summary>
		/// Gets DavFolder Path
		/// </summary>
		[ResourcePropertyAttribute(false)]
		public string FolderPath
		{
			get
			{
				return this.ResourcePath;
			}
		}
	}
}