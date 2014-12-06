//-----------------------------------------------------------------------
// <copyright file="DavGetBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Web;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	//TODO: conditional GET support:
	//	request message includes an If-Modified-Since, If-Unmodified-Since,
	//	If-Match, If-None-Match, or If-Range header field

	//TODO: partial GET support:
	//	request message includes a Range header field

	/// <summary>
	/// Dav Resource Get Framework Base Class
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
	///			Returns DavGetResponseCode.Created when successful
	///		</code>
	///		<seealso cref="DavGetResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>	
	public abstract class DavGetBase : DavMethodBase
	{
		/// <summary>
		/// Dav Resource Get Framework Base Class
		/// </summary>
		protected DavGetBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavGetBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavGetBase_InternalProcessDavRequest);

			this.ResponseCache = HttpCacheability.NoCache;
			this.ResponseCacheExpiration = DateTime.MinValue;
		}

		/// <summary>
		/// WebDav GET Response Codes
		/// </summary>
		public enum DavGetResponseCode : int
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
			///		The resource was sent 
			/// </remarks>
			Created = 201
		}

		/// <summary>
		/// Output response content type
		/// </summary>
		protected string ContentType
		{
			get
			{
				return base.HttpApplication.Request.ContentType;
			}
			set
			{
				base.HttpApplication.Request.ContentType = value;
			}
		}

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

		/// <summary>
		/// Resource output response
		/// </summary>
		protected byte[] ResponseOutput { get; set; }

		private int DavGetBase_ValidateDavRequest(object sender, EventArgs e)
		{
			if (base.RequestLength != 0)
				return (int)ServerResponseCode.BadRequest;

			try
			{
				//Try to set the content type based on the requested extension
				this.ContentType = InternalFunctions.GetMimeType(Path.GetExtension(this.RelativeRequestPath));
			}
			catch (Exception) { }

			return (int)ServerResponseCode.Ok;
		}

		private int DavGetBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			base.HttpApplication.Response.Cache.SetCacheability(ResponseCache);

			if (ResponseCacheExpiration != DateTime.MinValue)
				this.HttpApplication.Response.Cache.SetExpires(ResponseCacheExpiration);

			if (this.ResponseOutput.Length > 0)
			{
				//Write the response
				using (BinaryWriter _outputStream = new BinaryWriter(base.HttpApplication.Response.OutputStream))
				{
					_outputStream.Write(this.ResponseOutput);
					_outputStream.Close();
				}
			}

			return (int)ServerResponseCode.Ok;
			//DavGetResponseCode.Created;
		}
	}
}