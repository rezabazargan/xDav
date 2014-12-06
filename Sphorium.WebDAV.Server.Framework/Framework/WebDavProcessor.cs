//-----------------------------------------------------------------------
// <copyright file="WebDavProcessor.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Sphorium.WebDAV.Server.Framework.BaseClasses;
using Sphorium.WebDAV.Server.Framework.Interfaces;

namespace Sphorium.WebDAV.Server.Framework
{
	/// <summary>
	/// WebDav Framework entry point for Dav request processing
	/// </summary>
	public class WebDavProcessor
	{
		private static string __debugFilePath = null;
		private readonly Assembly __davSourceAssembly;

		/// <summary>
		/// Keeps track of all the inherited DavOptionsBase members
		/// </summary>
		private readonly SortedList<string, string> __davMethodList = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		/// Request / Response Debug File Path
		/// </summary>
		public static string DebugFilePath
		{
			get
			{
				return __debugFilePath;
			}
			set
			{
				__debugFilePath = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the WebDavProcessor class
		/// </summary>
		public WebDavProcessor() : this(Assembly.GetCallingAssembly()) { }

		/// <summary>
		/// Initializes a new instance of the WebDavProcessor class
		/// </summary>
		/// <param name="davSourceAssembly">Assembly containing custom DAV Method implementation </param>
		public WebDavProcessor(Assembly davSourceAssembly)
		{
			this.__davSourceAssembly = davSourceAssembly;

			//Use reflection to identify the map the Map the current 
			foreach (Type _objectType in this.DavSourceAssembly.GetTypes())
			{
				string _davMethod = null;

				if (_objectType.BaseType == typeof(DavOptionsBase))
					_davMethod = "OPTIONS";
				else if (_objectType.BaseType == typeof(DavMKColBase))
					_davMethod = "MKCOL";
				else if (_objectType.BaseType == typeof(DavPropFindBase))
					_davMethod = "PROPFIND";
				else if (_objectType.BaseType == typeof(DavHeadBase))
					_davMethod = "HEAD";
				else if (_objectType.BaseType == typeof(DavDeleteBase))
					_davMethod = "DELETE";
				else if (_objectType.BaseType == typeof(DavMoveBase))
					_davMethod = "MOVE";
				else if (_objectType.BaseType == typeof(DavCopyBase))
					_davMethod = "COPY";
				else if (_objectType.BaseType == typeof(DavPutBase))
					_davMethod = "PUT";
				else if (_objectType.BaseType == typeof(DavGetBase))
					_davMethod = "GET";
				else if (_objectType.BaseType == typeof(DavLockBase))
					_davMethod = "LOCK";
				else if (_objectType.BaseType == typeof(DavUnlockBase))
					_davMethod = "UNLOCK";
				else if (_objectType.BaseType == typeof(DavPropPatchBase))
					_davMethod = "PROPPATCH";
				else if (_objectType.BaseType == typeof(DavVersionControlBase))
					_davMethod = "VERSION-CONTROL";
				else if (_objectType.BaseType == typeof(DavReportBase))
					_davMethod = "REPORT";

				if (_davMethod != null)
				{
					if (this.__davMethodList.ContainsKey(_davMethod))
						throw new WebDavException("Duplicate objects for " + _davMethod + " found. There should only up to 1 object implementing each of the base DavMethod classes.");

					this.__davMethodList[_davMethod] = _objectType.ToString();
				}
			}
		}

		/// <summary>
		/// WebDav Framework method for Dav request processing
		/// </summary>
		/// <param name="httpApplication"></param>
		/// <remarks>
		///		Process all requests... will return 501 if the requested method is not implemented
		/// </remarks>
		public void ProcessRequest(HttpApplication httpApplication)
		{
			if (httpApplication == null)
				throw new ArgumentNullException("httpApplication", InternalFunctions.GetResourceString("ArgumentNullException", "HttpApplication"));

			//Set the status code to Method Not Allowed by default
			int _statusCode = 405;

			string _httpMethod = httpApplication.Request.HttpMethod;

			InternalFunctions.WriteDebugLog("Processing HttpMethod " + _httpMethod);
			//try
			{
				if (this.__davMethodList.ContainsKey(_httpMethod))
				{
					httpApplication.Response.Clear();
					httpApplication.Response.ClearContent();

					DavMethodBase _davMethodBase = this.DavSourceAssembly.CreateInstance(this.__davMethodList[_httpMethod]) as DavMethodBase;
					if (_davMethodBase != null)
					{
						_davMethodBase.HttpApplication = httpApplication;
						_statusCode = ((IDavRequest)_davMethodBase).ProcessRequest();
					}
				}
			}
			//catch (Exception ex)
			//{
			//    InternalFunctions.WriteDebugLog("Error processing HttpMethod " + _httpMethod + 
			//        Environment.NewLine + "Message: " + ex.Message);
			//}

			InternalFunctions.WriteDebugLog("Completed processing HttpMethod " + _httpMethod + " status code returned: " + _statusCode);

			if (this.__davMethodList.ContainsKey(_httpMethod))
			{
				httpApplication.Response.StatusCode = _statusCode;


				////TODO: remove this
				//System.Threading.Thread.Sleep(10000);

				//Perhaps implement...
				//httpApplication.Request.InputStream.Close();
				httpApplication.Response.End();
			}
		}

		#region Private Properties
		/// <summary>
		/// Dav Source Assembly
		/// </summary>
		private Assembly DavSourceAssembly
		{
			get
			{
				return this.__davSourceAssembly;
			}
		}
		#endregion
	}
}