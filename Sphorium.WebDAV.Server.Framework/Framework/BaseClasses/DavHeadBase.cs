//-----------------------------------------------------------------------
// <copyright file="DavHeadBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Web;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Head Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#rfc.section.8.4
	///			
	///		- If the requested resource does not exist the method MUST fail with:
	///			
	///					base.AbortRequest(ServerResponseCode.NotFound)
	///		</code>
	///		
	///		<code>
	///			Returns ServerResponseCode.OK when successful
	///		</code>
	///		<seealso cref="DavMethodBase.ServerResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>		
	public abstract class DavHeadBase : DavMethodBase
	{
		/// <summary>
		/// Dav Head Framework Base Class
		/// </summary>
		protected DavHeadBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavHeadBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavHeadBase_InternalProcessDavRequest);

			this.ResponseCache = HttpCacheability.NoCache;
			this.ResponseCacheExpiration = DateTime.MinValue;
		}

		/// <summary>
		/// Head Resource
		/// </summary>
		protected DavResourceBase Resource { get; set; }

		/// <summary>
		/// Output response cacheability.
		/// </summary>
		/// <remarks>HttpCacheability.NoCache by default</remarks>
		protected HttpCacheability ResponseCache { get; set; }

		/// <summary>
		/// Output response cache expiration
		/// </summary>
		/// <remarks>No expiration by default</remarks>
		protected DateTime ResponseCacheExpiration { get; set; }

		private int DavHeadBase_ValidateDavRequest(object sender, EventArgs e)
		{
			if (base.RequestLength != 0)
				return (int)ServerResponseCode.BadRequest;

			return (int)ServerResponseCode.Ok;
		}

		private int DavHeadBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			base.HttpApplication.Response.Cache.SetCacheability(ResponseCache);

			if (ResponseCacheExpiration != DateTime.MinValue)
				this.HttpApplication.Response.Cache.SetExpires(ResponseCacheExpiration);

			if (this.Resource != null)
			{
				base.HttpApplication.Response.Headers["Content-Type"] = this.Resource.ContentType;
				base.HttpApplication.Response.Headers["Content-Length"] = this.Resource.ContentLength.ToString();
				base.HttpApplication.Response.Headers["Last-Modified"] = this.Resource.LastModified.ToString("r");
			}

			return (int)ServerResponseCode.Ok;
		}
	}
}