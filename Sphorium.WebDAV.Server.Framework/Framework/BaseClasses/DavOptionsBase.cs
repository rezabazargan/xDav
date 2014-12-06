//-----------------------------------------------------------------------
// <copyright file="DavOptionsBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Text;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Server OPTIONS Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant / RFC3253 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#dav.compliance.classes
	///			http://www.webdav.org/specs/rfc3253.html#rfc.section.3.9	
	///			
	///		Class 1:
	///			Class 1 compliant resources MUST return, at minimum, the value "1" in the Dav header 
	///			on all responses to the OPTIONS method.
	///		Class 2: 
	///			Class 2 compliant resources MUST meet all class 1 requirements and support the LOCK 
	///			method, the supportedlock property, the lockdiscovery property, the Time-Out response 
	///			header and the Lock-Token request header. A class "2" compliant resource SHOULD also 
	///			support the Time-Out request header and the owner XML element.
	///			
	///			Class 2 compliant resources MUST return, at minimum, the values "1" and "2" in the 
	///			Dav header on all responses to the OPTIONS method.
	///		</code>
	///		
	///		<code>
	///			Returns ServerResponseCode.OK when successful
	///		</code>
	/// </remarks>	

	public abstract class DavOptionsBase : DavMethodBase
	{
		/// <summary>
		/// Dav Server OPTIONS Framework Base Class
		/// </summary>
		protected DavOptionsBase()
		{
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavOptionsBase_InternalProcessDavRequest);
			this.SupportedHttpMethods = HttpMethods.None;
		}

		/// <summary>
		/// WebDav HttpMethods
		/// </summary>
		[FlagsAttribute]
		protected internal enum HttpMethods : int
		{
			/// <summary>
			/// No supported methods
			/// </summary>
			None = 0,

			/// <summary>
			/// Supports OPTIONS Method
			/// </summary>
			Options = 1,

			/// <summary>
			/// Supports GET Method
			/// </summary>
			Get = 2,

			/// <summary>
			/// Supports HEAD Method
			/// </summary>
			Head = 4,

			/// <summary>
			/// Supports DELETE Method
			/// </summary>
			Delete = 8,

			/// <summary>
			/// Supports PUT Method
			/// </summary>
			Put = 16,

			/// <summary>
			/// Supports COPY Method
			/// </summary>
			Copy = 32,

			/// <summary>
			/// Supports MOVE Method
			/// </summary>
			Move = 64,

			/// <summary>
			/// Supports MKCOL Method
			/// </summary>
			MKCol = 128,

			/// <summary>
			/// Supports PROPFIND Method
			/// </summary>
			PropFind = 256,

			/// <summary>
			/// Supports PROPPATCH Method
			/// </summary>
			PropPatch = 512,

			/// <summary>
			/// Supports LOCK Method
			/// </summary>
			Lock = 1024,

			/// <summary>
			/// Supports UNLOCK Method
			/// </summary>
			Unlock = 2048,

			/// <summary>
			/// Supports VERSION-CONTROL Method
			/// </summary>
			VersionControl = 4096,

			/// <summary>
			/// Supports REPORT Method
			/// </summary>
			Report = 8192,

			/// <summary>
			/// Supports All Methods
			/// </summary>
			All = HttpMethods.Copy | HttpMethods.Delete | HttpMethods.Get | HttpMethods.Head | HttpMethods.Lock |
				HttpMethods.MKCol | HttpMethods.Move | HttpMethods.Options | HttpMethods.PropFind | HttpMethods.PropPatch | HttpMethods.Put |
				HttpMethods.Report | HttpMethods.Unlock | HttpMethods.VersionControl
		}

		/// <summary>
		/// WebDav Server Supported Http Methods
		/// </summary>
		protected HttpMethods SupportedHttpMethods { get; set; }

		private int DavOptionsBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			string _davClass = "1";
			if ((SupportedHttpMethods & HttpMethods.Lock) != 0 && (SupportedHttpMethods & HttpMethods.PropFind) != 0)
				_davClass += ", 2";

			StringBuilder _allowedMethods = new StringBuilder();
			foreach (string enumName in Enum.GetNames(typeof(HttpMethods)))
			{
				if (enumName != "All" && enumName != "None")
				{
					HttpMethods _httpMethod = (HttpMethods)Enum.Parse(typeof(HttpMethods), enumName, true);

					if ((SupportedHttpMethods & _httpMethod) != 0)
					{
						if (_httpMethod == HttpMethods.VersionControl)
							_allowedMethods.Append("version-control" + ", ");
						else
							_allowedMethods.Append(enumName.ToUpper() + ", ");
					}
				}
			}

			//Remove the trailing comma [, ]
			if (_allowedMethods.Length > 2)
				_allowedMethods.Remove(_allowedMethods.Length - 2, 2);

			base.HttpApplication.Response.AddHeader("DAV", _davClass);
			base.HttpApplication.Response.AddHeader("Public", _allowedMethods.ToString());
			base.HttpApplication.Response.AddHeader("Allow", _allowedMethods.ToString());
			base.HttpApplication.Response.AddHeader("Accept-Ranges", "bytes");

			return (int)ServerResponseCode.Ok;
		}
	}
}
