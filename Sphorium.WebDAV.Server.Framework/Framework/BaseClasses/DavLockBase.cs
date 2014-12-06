//-----------------------------------------------------------------------
// <copyright file="DavLockBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Sphorium.WebDAV.Server.Framework.Classes;
using Sphorium.WebDAV.Server.Framework.Collections;
using Sphorium.WebDAV.Server.Framework.Interfaces;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource Lock Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#METHOD_LOCK
	///			
	///		- The scope of a lock is the entire state of the resource, including its body and 
	///		associated properties. As a result, a lock on a resource MUST also lock the resource's 
	///		properties. For collections, a lock also affects the ability to add or remove members. 
	///		The nature of the effect depends upon the type of access control involved.
	///		
	///		</code>
	///		
	///		<code>
	///			Returns DavLockResponseCode.OK when successful
	///		</code>
	///		<seealso cref="DavLockResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>
	public abstract class DavLockBase : DavMethodBase, IDavRequestProcessError
	{
		/// <summary>
		/// Dav Resource Lock Framework Base Class
		/// </summary>
		protected DavLockBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavLockBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavLockBase_InternalProcessDavRequest);

			this.ProcessErrors = new DavProcessErrorCollection();
			this.RequestLock = new DavLockProperty();
			this.ResponseLock = new DavLockProperty();
		}

		#region Delegate Section
		/// <summary>
		/// Dav Refresh Lock handler delegate
		/// </summary>
		protected delegate void DavRefreshLockEventHandler(object sender, DavRefreshEventArgs e);

		/// <summary>
		/// Occurs when the server is requesting a lock refresh
		/// </summary>
		protected event DavRefreshLockEventHandler RefreshLockDavRequest;

		/// <summary>
		/// Raises the LockRefreshDavRequest event
		/// </summary>
		protected void OnRefreshLockDavRequest(DavRefreshEventArgs e)
		{
			if (RefreshLockDavRequest != null)
				RefreshLockDavRequest(this, e);
		}
		#endregion

		#region Enumerations
		/// <summary>
		/// WebDav LOCK Response Codes
		/// </summary>
		protected enum DavLockResponseCode : int
		{
			/// <summary>
			/// 0: None
			/// </summary>
			/// <remarks>
			///		Default enumerator value
			/// </remarks>
			None = 0,

			/// <summary>
			/// 200: Ok
			/// </summary>
			/// <remarks>
			///		The lock request succeeded and the value of the lockdiscovery 
			///		property is included in the body.
			/// </remarks>
			Ok = 200,

			/// <summary>
			/// 400: Bad Request
			/// </summary>
			/// <example>If a Depth of 1 is requested</example>
			BadRequest = 400,

			/// <summary>
			/// 412: Precondition Failed
			/// </summary>
			/// <remarks>
			///		The included lock token was not enforceable on this resource or 
			///		the server could not satisfy the request in the lockinfo XML element.
			/// </remarks>
			PreconditionFailed = 412,

			/// <summary>
			/// 423: Resource Locked
			/// </summary>
			/// <remarks>
			///		If the resource is already locked with an exclusive lock or if the resource
			///		is already locked with a shared lock and the client requests and exclusive lock
			/// </remarks>
			Locked = 423,

			/// <summary>
			/// 424: FailedDependency
			/// </summary>
			/// <remarks>
			///		Implies the action would have succeeded by itself
			/// </remarks>
			FailedDependency = 424
		}
		#endregion

		#region Properties
		/// <summary>
		/// Requested lock information.
		/// </summary>
		/// <remarks>
		///		The returned object is readonly
		///	</remarks>
		protected DavLockProperty RequestLock { get; private set; }

		/// <summary>
		/// The lock response
		/// </summary>
		/// <remarks>
		///		The values will be defaulted to the RequestLock information
		/// </remarks>
		protected DavLockProperty ResponseLock { get; set; }
		#endregion

		#region Internal Functions
		/// <summary>
		///		<![CDATA[Parses the HttpApplication.Request.Headers["Timeout"]]]>
		/// </summary>
		/// <returns>LockTimeout</returns>
		private int ParseTimeoutHeader()
		{
			int _lockTimeout = 180;

			if (base.HttpApplication.Request.Headers["Timeout"] != null)
			{
				//Parse the Timeout lock request
				string _timeoutHeader = base.HttpApplication.Request.Headers["Timeout"];
				string[] _timeoutInfo = _timeoutHeader.Split('-');

				//There should only be 2 segments
				if (_timeoutInfo.Length == 2)
				{
					try
					{
						_lockTimeout = Convert.ToInt32(_timeoutInfo[1], CultureInfo.InvariantCulture);
					}
					catch (InvalidCastException)
					{
						//Incase the value cannot be cast
					}
				}
			}

			return _lockTimeout;
		}


		private int DavLockBase_ValidateDavRequest(object sender, EventArgs e)
		{
			//Allow the lock to be written
			this.RequestLock.ReadOnly = false;

			int _responseCode = (int)ServerResponseCode.Ok;
			if (base.RequestDepth == DepthType.ResourceChildren)
				_responseCode = (int)DavLockResponseCode.BadRequest;

			else if (base.HttpApplication.Request.Headers["If"] != null)
			{

				string _lockTokenHeader = InternalFunctions.ParseOpaqueLockToken(base.HttpApplication.Request.Headers["If"]);
				this.RequestLock.AddLockToken(_lockTokenHeader);
				this.RequestLock.LockTimeout = ParseTimeoutHeader();

				//A lock refresh was requested... Timeout header is not required for a lock refresh
				base.FireOnProcessDavRequest = false;
			}
			//Not sure if this is needed
			//else if (base.HttpApplication.Request.Headers["Timeout"] == null)
			//	_responseCode = (int)ServerResponseCode.BadRequest;

			else if (base.RequestXml == null)
				_responseCode = (int)ServerResponseCode.BadRequest;

			else
			{
				//Load the valid properties
				XPathNodeIterator _lockInfoNodeIterator = base.RequestXml.SelectDescendants("lockinfo", "DAV:", false);
				if (!_lockInfoNodeIterator.MoveNext())
					_responseCode = (int)ServerResponseCode.BadRequest;
				else
				{
					this.RequestLock.LockTimeout = ParseTimeoutHeader();

					//Get the lock type
					XPathNodeIterator _lockTypeNodeIterator = _lockInfoNodeIterator.Current.SelectDescendants("locktype", "DAV:", false);
					if (_lockTypeNodeIterator.MoveNext())
					{
						XPathNavigator _currentNode = _lockTypeNodeIterator.Current;
						if (_currentNode.MoveToFirstChild())
						{
							switch (_currentNode.LocalName.ToLower(CultureInfo.InvariantCulture))
							{
								case "read":
									this.RequestLock.LockType = LockType.Read;
									break;

								case "write":
									this.RequestLock.LockType = LockType.Write;
									break;
							}
						}
					}

					//Get the lock type
					XPathNodeIterator _lockScopeNodeIterator = _lockInfoNodeIterator.Current.SelectDescendants("lockscope", "DAV:", false);
					if (_lockScopeNodeIterator.MoveNext())
					{
						XPathNavigator _currentNode = _lockScopeNodeIterator.Current;
						if (_currentNode.MoveToFirstChild())
						{
							switch (_currentNode.LocalName.ToLower(CultureInfo.InvariantCulture))
							{
								case "shared":
									this.RequestLock.LockScope = LockScope.Shared;
									break;

								case "exclusive":
									this.RequestLock.LockScope = LockScope.Exclusive;
									break;
							}
						}
					}

					//Get the lock owner
					XPathNodeIterator _lockOwnerNodeIterator = _lockInfoNodeIterator.Current.SelectDescendants("owner", "DAV:", false);
					if (_lockOwnerNodeIterator.MoveNext())
					{
						XPathNavigator _currentNode = _lockOwnerNodeIterator.Current;

						if (_currentNode.NodeType == XPathNodeType.Text)
						{
							this.RequestLock.LockOwnerType = LockOwnerType.User;
						}
						else
						{
							if (_currentNode.MoveToFirstChild())
							{
								//TODO: Expand this to other LockOwnerTypes

								switch (_currentNode.LocalName.ToLower(CultureInfo.InvariantCulture))
								{
									case "href":
										this.RequestLock.LockOwnerType = LockOwnerType.Href;
										break;
								}
							}
						}

						this.RequestLock.LockOwner = _currentNode.Value;
					}
				}
			}

			//Copy the properties
			this.ResponseLock.Copy(this.RequestLock);

			//Set the object to readonly
			this.RequestLock.ReadOnly = true;
			return _responseCode;
		}


		private int DavLockBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			int _responseCode = (int)DavLockResponseCode.Ok;

			string[] _lockTokens = this.RequestLock.GetLockTokens();

			//Check to see if a lock refresh was requested
			if (base.HttpApplication.Request.Headers["If"] != null)
			{
				if (_lockTokens.Length == 1)
				{
					DavRefreshEventArgs _refreshEventArgs = new DavRefreshEventArgs(_lockTokens[0], this.RequestLock.LockTimeout);
					OnRefreshLockDavRequest(_refreshEventArgs);
				}

				base.HttpApplication.Response.AppendHeader("Timeout", "Second-" + this.ResponseLock.LockTimeout);
			}
			else
			{
				//New lock request
				StringBuilder _opaquelockTokens = new StringBuilder();
				foreach (string _lockToken in _lockTokens)
					_opaquelockTokens.Append("<opaquelocktoken:" + _lockToken + ">");

				base.HttpApplication.Response.AppendHeader("Lock-Token", _opaquelockTokens.ToString());
			}

			//Check to see if there were any process errors...
			Enum[] _errorResources = this.ProcessErrorResources;
			if (_errorResources.Length > 0)
			{
				//Append a response node
				XmlDocument _xmlDocument = new XmlDocument();
				XmlNode _responseNode = _xmlDocument.CreateNode(XmlNodeType.Element, _xmlDocument.GetPrefixOfNamespace("DAV:"), "response", "DAV:");

				//Add the HREF
				XmlElement _requestLockHrefElement = _xmlDocument.CreateElement("href", "DAV:");
				_requestLockHrefElement.InnerText = base.RelativeRequestPath;
				_responseNode.AppendChild(_requestLockHrefElement);


				//Add the propstat
				XmlElement _propstatElement = _xmlDocument.CreateElement("propstat", "DAV:");
				XmlElement _propElement = _xmlDocument.CreateElement("prop", "DAV:");
				XmlElement _lockDiscoveryElement = _xmlDocument.CreateElement("lockdiscovery", "DAV:");
				_propElement.AppendChild(_lockDiscoveryElement);
				_propstatElement.AppendChild(_propElement);

				XmlElement _statusElement = _xmlDocument.CreateElement("status", "DAV:");
				_statusElement.InnerText = InternalFunctions.GetEnumHttpResponse(DavLockResponseCode.FailedDependency);
				_propstatElement.AppendChild(_statusElement);

				_responseNode.AppendChild(_propstatElement);

				base.SetResponseXml(InternalFunctions.ProcessErrorRequest(this.ProcessErrors, _responseNode));
				_responseCode = (int)ServerResponseCode.MultiStatus;
			}
			else
			{
				//No issues
				using (Stream _responseStream = new MemoryStream())
				{
					XmlTextWriter _xmlWriter = new XmlTextWriter(_responseStream, new UTF8Encoding(false));

					_xmlWriter.Formatting = Formatting.Indented;
					_xmlWriter.IndentChar = '\t';
					_xmlWriter.Indentation = 1;
					_xmlWriter.WriteStartDocument();

					//Open the prop element section
					_xmlWriter.WriteStartElement("D", "prop", "DAV:");
					_xmlWriter.WriteStartElement("lockdiscovery", "DAV:");
					this.ResponseLock.ActiveLock.WriteTo(_xmlWriter);
					_xmlWriter.WriteEndElement();
					_xmlWriter.WriteEndElement();

					_xmlWriter.WriteEndDocument();
					_xmlWriter.Flush();

					base.SetResponseXml(_responseStream);
					_xmlWriter.Close();
				}
			}

			return _responseCode;
		}
		#endregion

		#region IDavRequestProcessError Members
		/// <summary>
		/// Processing errors 
		/// </summary>
		private DavProcessErrorCollection ProcessErrors { get; set; }

		/// <summary>
		/// Clear the current processing errors
		/// </summary>
		public void ClearProcessErrors()
		{
			this.ProcessErrors.Clear();
		}

		/// <summary>
		/// Add a resource to the error list
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="errorCode"></param>
		public void AddProcessErrorResource(DavResourceBase resource, Enum errorCode)
		{
			this.ProcessErrors.Add(resource, errorCode);
		}

		/// <summary>
		/// Retrieve all the process errors
		/// </summary>
		public Enum[] ProcessErrorResources
		{
			get
			{
				return this.ProcessErrors.AllResourceErrors;
			}
		}
		#endregion
	}
}