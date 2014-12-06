//-----------------------------------------------------------------------
// <copyright file="DavReportBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Xml.XPath;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Version Report Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC3253 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC3253
	///			http://www.webdav.org/specs/rfc3253.html#METHOD_REPORT
	///		
	///		
	///		//TODO: update this	
	///		-	If a request body is included, it MUST be a DAV:version-control XML element.
	///		
	///		- If a response body for a successful request is included, it MUST be a 
	///			DAV:version-control-response XML element.
	///		
	///		NOTE: There is no way to undo a VERSION-CONTROL command.
	///		</code>
	///		
	///		<code>
	///			Returns ServerResponseCode.Ok when successful
	///		</code>
	///		<seealso cref="DavMethodBase.ServerResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>
	public abstract class DavReportBase : DavResourceFindBase
	{
		/// <summary>
		/// Dav Version Control Framework Base Class
		/// </summary>
		protected DavReportBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavReportBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavReportBase_InternalProcessDavRequest);

			this.RequestVersionReportType = VersionReportType.None;
		}

		/// <summary>
		/// Property request types
		/// </summary>
		protected enum VersionReportType
		{
			/// <summary>
			/// No report requested
			/// </summary>
			None,

			/// <summary>
			/// Specific properties requested
			/// </summary>
			VersionTree,

			/// <summary>
			/// A summary of all the available properties
			/// </summary>
			ExpandProperty
		}

		/// <summary>
		/// Requested Version report type
		/// <seealso cref="VersionReportType"/>
		/// </summary>
		protected VersionReportType RequestVersionReportType { get; private set; }

		private int DavReportBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			return base.LoadNodes(base.RequestProperties);
		}

		private int DavReportBase_ValidateDavRequest(object sender, EventArgs e)
		{
			int _returnCode = (int)ServerResponseCode.Ok;

			if (base.RequestXml == null)
				_returnCode = (int)ServerResponseCode.BadRequest;

			else
			{
				XPathNavigator _requestXPathNavigator = base.RequestXml;
				if (_requestXPathNavigator.MoveToFirstChild())
				{
					switch (_requestXPathNavigator.LocalName.ToLower(CultureInfo.InvariantCulture))
					{
						case "version-tree":
							this.RequestVersionReportType = VersionReportType.VersionTree;
							break;
						case "expand-property":
							this.RequestVersionReportType = VersionReportType.ExpandProperty;
							break;
					}
				}
				else
					_returnCode = (int)ServerResponseCode.BadRequest;
			}

			return _returnCode;
		}
	}
}