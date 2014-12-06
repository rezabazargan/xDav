//-----------------------------------------------------------------------
// <copyright file="DavCopyMoveBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml.XPath;
using Sphorium.WebDAV.Server.Framework.Collections;
using Sphorium.WebDAV.Server.Framework.Interfaces;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource Common Copy/Move Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	/// </remarks>	
	public abstract class DavCopyMoveBase : DavMethodBase, IDavRequestProcessError
	{
		private List<string> __keepAliveURIs = new List<string>();

		/// <summary>
		/// Dav Resource Common Copy/Move Framework Base Class
		/// </summary>
		protected DavCopyMoveBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavCopyMoveMoveBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavCopyMoveMoveBase_InternalProcessDavRequest);

			this.RequestPropertyBehavior = PropertyBehavior.Omit;
			this.ProcessErrors = new DavProcessErrorCollection();
		}

		/// <summary>
		/// WebDav Property behavior
		/// <seealso cref="PropertyBehavior"/>
		/// </summary>
		protected PropertyBehavior RequestPropertyBehavior { get; private set; }


		/// <summary>
		/// Maintain all resource live properties
		/// </summary>
		protected bool KeepAliveAllProperties { get; private set; }


		/// <summary>
		/// Requested URI KeepAlive list
		/// </summary>
		protected List<string> GetKeepAliveURIs()
		{
			return this.__keepAliveURIs;
		}


		/// <summary>
		/// Specify if the destination resource was overwritten
		/// </summary>
		protected bool DestinationResourceOverwritten { get; set; }


		/// <summary>
		/// Requested destination
		/// </summary>
		protected string RequestDestination
		{
			get
			{
				StringBuilder _destination = new StringBuilder(base.HttpApplication.Request.Headers["Destination"]);

				//Remove :80 & :443
				_destination.Replace(":80/", "/");
				_destination.Replace(":443/", "/");

				return HttpUtility.UrlDecode(_destination.ToString().Trim('/'));
			}
		}


		/// <summary>
		/// Returns the URL segment without the root information
		/// </summary>
		protected string RelativeRequestDestination
		{
			get
			{
				return InternalFunctions.GetRelativePath(base.HttpApplication, this.RequestDestination);
			}
		}


		/// <summary>
		/// Check to see if an existing resource should be overwritten
		/// </summary>
		protected bool OverwriteExistingResource
		{
			get
			{
				bool _overwriteResource = true;

				if (base.HttpApplication.Request.Headers["Overwrite"] != null)
					_overwriteResource = !(base.HttpApplication.Request.Headers["Overwrite"] == "f");

				return _overwriteResource;
			}
		}

		/// <summary>
		/// WebDav Common Copy Move Response codes
		/// </summary>
		private enum DavCopyMoveResponseCode : int
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
			NoContent = 204
		}

		private int DavCopyMoveMoveBase_ValidateDavRequest(object sender, EventArgs e)
		{
			int _returnCode = (int)ServerResponseCode.Ok;

			if (base.RequestDepth != DepthType.ResourceOnly && base.RequestDepth != DepthType.Infinity)
				_returnCode = (int)ServerResponseCode.BadRequest;
			else if (base.HttpApplication.Request.Headers["Destination"] == null)
				_returnCode = (int)ServerResponseCode.BadRequest;
			else
			{
				if (base.RequestXml != null)
				{
					XPathNodeIterator _propertyBehaviorNodeIterator = base.RequestXml.SelectDescendants("propertybehavior", "DAV:", false);
					if (_propertyBehaviorNodeIterator.MoveNext())
					{

						XPathNodeIterator _keepAliveNodeIterator = _propertyBehaviorNodeIterator.Current.SelectDescendants("keepalive", "DAV:", false);
						if (_keepAliveNodeIterator.MoveNext())
						{
							this.RequestPropertyBehavior = PropertyBehavior.KeepAlive;

							XPathNodeIterator _keepAliveChildren = _keepAliveNodeIterator.Current.SelectChildren(XPathNodeType.All);
							while (_keepAliveChildren.MoveNext())
							{
								string _nodeValue = _keepAliveChildren.Current.Value;

								switch (_nodeValue)
								{
									case "*":
										this.KeepAliveAllProperties = true;
										break;

									default:
										this.__keepAliveURIs.Add(_nodeValue);
										break;
								}
							}
						}
					}
				}
			}

			return _returnCode;
		}


		private int DavCopyMoveMoveBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			int _responseCode = (int)DavCopyMoveResponseCode.Created;

			//Check to see if there were any process errors...
			Enum[] _errorResources = this.ProcessErrorResources;
			if (_errorResources.Length > 0)
			{
				base.SetResponseXml(InternalFunctions.ProcessErrorRequest(this.ProcessErrors));
				_responseCode = (int)ServerResponseCode.MultiStatus;
			}
			else if (this.DestinationResourceOverwritten)
				_responseCode = (int)DavCopyMoveResponseCode.NoContent;
			else
			{
				//base.HttpApplication.Response.AddHeader("Location", this.RequestDestination);
			}

			return _responseCode;
		}


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