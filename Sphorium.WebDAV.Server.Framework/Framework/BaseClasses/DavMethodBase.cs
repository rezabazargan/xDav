//-----------------------------------------------------------------------
// <copyright file="DavMethodBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml.XPath;
using Sphorium.WebDAV.Server.Framework.Interfaces;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Method Framework Base Class
	/// </summary>
	public abstract class DavMethodBase : IDavRequest
	{
		private HttpApplication __httpApplication;
		private XPathNavigator __requestXmlNavigator;
		private int __abortStatusCode = (int)ServerResponseCode.Ok;
		private int __httpResponseCode = (int)ServerResponseCode.Ok;
		private int __internalStatusCode = (int)ServerResponseCode.Ok;
		private string __responseXml = "";
		private bool __fireOnProcessDavRequest = true;

		/// <summary>
		/// Dav Method Framework Base Class
		/// </summary>
		protected DavMethodBase() { }

		#region Delegate Section
		/// <summary>
		/// Dav Request handler delegate
		/// </summary>
		protected delegate void DavRequestEventHandler(object sender, EventArgs e);

		/// <summary>
		/// Occurs when the server is about to process the Dav request
		/// </summary>
		protected event DavRequestEventHandler PreProcessDavRequest;

		/// <summary>
		/// Raises the PreprocessDavRequest event
		/// </summary>
		protected void OnPreProcessDavRequest(EventArgs e)
		{
			if (PreProcessDavRequest != null)
				PreProcessDavRequest(this, e);
		}

		/// <summary>
		/// Occurs when the server has completed processing the Dav request prior to 
		/// sending the ResponseXml.
		/// <seealso cref="ResponseXml"/>
		/// </summary>
		protected event DavRequestEventHandler PostProcessDavRequest;

		/// <summary>
		/// Raises the PostProcessDavRequest event
		/// </summary>
		protected void OnPostProcessDavRequest(EventArgs e)
		{
			if (PostProcessDavRequest != null)
				PostProcessDavRequest(this, e);
		}

		/// <summary>
		/// Dav Request process handler delegate
		/// </summary>
		protected delegate void DavProcessEventHandler(object sender, EventArgs e);

		/// <summary>
		/// Occurs when the server int has completed processing the Dav request
		/// </summary>
		protected event DavProcessEventHandler ProcessDavRequest;

		/// <summary>
		/// Raises the ProcessDavRequest event
		/// </summary>
		protected void OnProcessDavRequest(EventArgs e)
		{
			if (ProcessDavRequest != null)
				ProcessDavRequest(this, e);
		}

		/// <summary>
		/// Dav internal Request process handler delegate
		/// </summary>
		internal delegate int DavInternalProcessHandler(object sender, EventArgs e);

		/// <summary>
		/// Internal event... occurs when the server int has completed processing the Dav request
		/// </summary>
		internal event DavInternalProcessHandler InternalProcessDavRequest;

		/// <summary>
		/// Raises the InternalProcessDavRequest event
		/// </summary>
		internal int OnInternalProcessDavRequest(EventArgs e)
		{
			int _internalResponseCode = (int)ServerResponseCode.Ok;
			if (InternalProcessDavRequest != null)
				_internalResponseCode = InternalProcessDavRequest(this, e);

			return _internalResponseCode;
		}

		/// <summary>
		/// Dav internal Request validator delegate
		/// </summary>
		internal delegate int DavRequestValidator(object sender, EventArgs e);

		/// <summary>
		/// Occurs when the server has is validating the Dav request
		/// </summary>
		/// <remarks>Fires after the PreprocessDavRequest</remarks>
		internal event DavRequestValidator ValidateDavRequest;

		/// <summary>
		/// Raises the ValidateDavRequest event
		/// </summary>
		internal int OnValidateDavRequest(EventArgs e)
		{
			int _internalValidationCode = (int)ServerResponseCode.Ok;

			if (ValidateDavRequest != null)
				_internalValidationCode = ValidateDavRequest(this, e);

			return _internalValidationCode;
		}
		#endregion

		#region IDavRequest Members
		/// <summary>
		/// Keep the interface implementation hidden
		/// </summary>
		/// <returns></returns>
		int IDavRequest.ProcessRequest()
		{
			if (WebDavProcessor.DebugFilePath != null)
			{
				int _inputStreamLength = (int)HttpApplication.Request.InputStream.Length;
				if (_inputStreamLength > 0)
				{
					byte[] _requestInput = HttpApplication.Request.BinaryRead(_inputStreamLength);

					InternalFunctions.WriteDebugLog
					(
						"RequestXml:" + Environment.NewLine + System.Text.Encoding.ASCII.GetString(_requestInput)
					);

					HttpApplication.Request.InputStream.Position = 0;
				}
				else
					InternalFunctions.WriteDebugLog("RequestXml: [null]");
			}

			//Reset all the variables
			this.SetResponseXml("");
			this.__internalStatusCode = (int)ServerResponseCode.Ok;
			this.__abortStatusCode = (int)ServerResponseCode.Ok;
			this.__httpResponseCode = (int)ServerResponseCode.Ok;
			this.FireOnProcessDavRequest = true;

			//Notify the client the server is about to process the dav request
			this.OnPreProcessDavRequest(EventArgs.Empty);

			if (IsRequestAborted())
				InternalFunctions.WriteDebugLog("WebDAV request aborted... abort status code: " + this.__abortStatusCode);

			else
			{
				//Validate the Dav Request... this will always return ServerResponseCode.OK if successful
				this.__httpResponseCode = this.OnValidateDavRequest(EventArgs.Empty);

				InternalFunctions.WriteDebugLog("Validating WebDAV request: " + this.__httpResponseCode);
				if (this.__httpResponseCode == (int)ServerResponseCode.Ok)
				{
					//Fire the request event
					if (FireOnProcessDavRequest)
						this.OnProcessDavRequest(EventArgs.Empty);

					if (!IsRequestAborted())
						this.__httpResponseCode = this.OnInternalProcessDavRequest(EventArgs.Empty);
				}
				this.OnPostProcessDavRequest(EventArgs.Empty);

				if (!IsRequestAborted())
				{
					//Set the base response
					if (ResponseXml.Length != 0)
					{
						HttpApplication.Response.ContentEncoding = System.Text.Encoding.UTF8;
						HttpApplication.Response.ContentType = "text/xml";
						HttpApplication.Response.Write(ResponseXml);

						//For debugging
						InternalFunctions.WriteDebugLog("ResponseXml:" + Environment.NewLine + ResponseXml);
					}
					else
						InternalFunctions.WriteDebugLog("ResponseXml: [null]");
				}
			}

			return this.HttpResponseCode;
		}
		#endregion

		#region Enumerators
		/// <summary>
		/// WebDav Common Response Codes
		/// </summary>
		protected enum ServerResponseCode : int
		{
			/// <summary>
			/// 0: None
			/// </summary>
			/// <remarks>
			///		Default enumerator value
			/// </remarks>
			None = 0,

			/// <summary>
			///	200: Ok 
			/// </summary>
			Ok = 200,

			/// <summary>
			/// 207: Multi Status
			/// </summary>
			/// <remarks>Used by PropFind</remarks>
			MultiStatus = 207,

			/// <summary>
			/// 400: Bad Request
			/// </summary>
			BadRequest = 400,

			/// <summary>
			/// 404: Not Found
			/// </summary>
			NotFound = 404,

			/// <summary>
			/// 412: Precondition Failed
			/// </summary>
			/// <remarks></remarks>
			PreconditionFailed = 412,

			/// <summary>
			/// 501: Method Not Implemented
			/// </summary>
			MethodNotImplemented = 501
		}
		#endregion

		/// <summary>
		/// Returns the current HttpResponseCode
		/// </summary>
		protected int HttpResponseCode
		{
			get
			{
				int _responseCode;

				if (IsRequestAborted())
					_responseCode = this.__abortStatusCode;
				else if (this.__internalStatusCode != (int)ServerResponseCode.Ok)
					_responseCode = this.__internalStatusCode;
				else
					_responseCode = this.__httpResponseCode;

				return _responseCode;
			}
		}

		/// <summary>
		/// The value written to the HttpResponse response after the OnPostProcessDavRequest event has fired
		/// <seealso cref="OnPostProcessDavRequest"/>
		/// </summary>
		protected string ResponseXml
		{
			get
			{
				return this.__responseXml;
			}
		}

		/// <summary>
		/// Aborts the current request processing
		/// </summary>
		protected void AbortRequest(System.Enum responseCode)
		{
			if (responseCode == null)
				throw new ArgumentNullException("HttpApplication", InternalFunctions.GetResourceString("ArgumentNullException", "ResponseCode"));

			Type _enumType = responseCode.GetType();
			if (responseCode.GetTypeCode() == TypeCode.Int32)
				AbortRequest((int)System.Enum.Parse(_enumType, responseCode.ToString(), true));
			else
				throw new WebDavException(InternalFunctions.GetResourceString("InvalidEnumIntType"));
		}

		/// <summary>
		/// Aborts the current request processing
		/// </summary>
		/// <param name="responseCode">HttpResponse status code</param>
		protected internal void AbortRequest(int responseCode)
		{
			if (responseCode >= 200 && responseCode < 300)
				throw new WebDavException(InternalFunctions.GetResourceString("InvalidResponseCode"));

			this.__abortStatusCode = responseCode;
		}

		/// <summary>
		/// Check to see if the request has been aborted
		/// </summary>
		/// <returns></returns>
		private bool IsRequestAborted()
		{
			return (this.__abortStatusCode != (int)ServerResponseCode.Ok);
		}

		/// <summary>
		/// Method HttpApplication
		/// </summary>
		public HttpApplication HttpApplication
		{
			get
			{
				return this.__httpApplication;
			}
			set
			{
				this.__httpApplication = value;

				//base.HttpApplication.Response.AddHeader("Engine", "Sphorium.WebDAV.Server.Framework");

				//Microsoft Required header - Thanks to Michael Liebman for this tidbit!
				this.__httpApplication.Response.AddHeader("MS-Author-Via", "DAV");
			}
		}

		/// <summary>
		/// Returns the URL segment without the root information
		/// </summary>
		protected string RelativeRequestPath
		{
			get
			{
				if (HttpApplication == null)
					throw new ArgumentNullException("HttpApplication", InternalFunctions.GetResourceString("ArgumentNullException", "HttpApplication"));

				InternalFunctions.WriteDebugLog("DavMethodBase - RelativeRequestPath: " + this.HttpApplication.Request.FilePath);

				return InternalFunctions.GetRelativePath(this.HttpApplication, this.HttpApplication.Request.FilePath);
			}
		}

		/// <summary>
		/// HttpRequest Xml
		/// </summary>
		/// <remarks>
		///		This will return null if there is no RequestXml
		///	</remarks>
		protected XPathNavigator RequestXml
		{
			get
			{
				if (HttpApplication == null)
					throw new ArgumentNullException("HttpApplication", InternalFunctions.GetResourceString("ArgumentNullException", "HttpApplication"));

				if (this.__requestXmlNavigator == null && HttpApplication.Request.InputStream.Length > 0)
				{
					//XDocument _document = XDocument.Load(new StreamReader(HttpApplication.Request.InputStream));
					//var _other = _document.CreateNavigator();

					XPathDocument _requestXPathDoc = new XPathDocument(HttpApplication.Request.InputStream);
					this.__requestXmlNavigator = _requestXPathDoc.CreateNavigator();
				}

				return this.__requestXmlNavigator;
			}
		}

		/// <summary>
		/// HttpRequest Length
		/// </summary>
		protected long RequestLength
		{
			get
			{
				if (HttpApplication == null)
					throw new ArgumentNullException("HttpApplication", InternalFunctions.GetResourceString("ArgumentNullException", "HttpApplication"));

				return HttpApplication.Request.InputStream.Length;
			}
		}

		/// <summary>
		/// WebDav Requested Depth
		/// <seealso cref="DepthType"/>
		/// </summary>
		protected DepthType RequestDepth
		{
			get
			{
				//Based on the spec the default is infinity if a ["Depth"] header is not included
				DepthType _depth = DepthType.Infinity;

				if (HttpApplication == null)
					throw new ArgumentNullException("HttpApplication", InternalFunctions.GetResourceString("ArgumentNullException", "HttpApplication"));

				try
				{
					_depth = (DepthType)Enum.Parse(typeof(DepthType), HttpApplication.Request.Headers["Depth"], true);
				}
				catch (ArgumentException) { }
				return _depth;
			}
		}


		#region Internal Methods
		/// <summary>
		/// Enable base classes to selectively disable firing of the OnProcessDavRequest
		/// <seealso cref="DavLockBase"/>
		/// </summary>
		internal bool FireOnProcessDavRequest
		{
			get
			{
				return this.__fireOnProcessDavRequest;
			}
			set
			{
				this.__fireOnProcessDavRequest = value;
			}
		}

		/// <summary>
		/// Reads the stream object and sets ResponseXml 
		/// </summary>
		/// <param name="stream"></param>
		internal void SetResponseXml(Stream stream)
		{
			using (StreamReader _streamReader = new StreamReader(stream, Encoding.UTF8))
			{
				//Go to the begining of the stream
				_streamReader.BaseStream.Position = 0;
				this.SetResponseXml(_streamReader.ReadToEnd());
			}
		}

		/// <summary>
		/// Set the ResponseXml
		/// </summary>
		/// <param name="responseXml"></param>
		internal void SetResponseXml(string responseXml)
		{
			this.__responseXml = responseXml;
		}
		#endregion
	}
}
