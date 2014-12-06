//-----------------------------------------------------------------------
// <copyright file="DavVersionControlBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Xml.XPath;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Version Control Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC3253 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC3253
	///			http://www.webdav.org/specs/rfc3253.html#METHOD_VERSION-CONTROL
	///			
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
	public abstract class DavVersionControlBase : DavMethodBase
	{
		/// <summary>
		/// Dav Version Control Framework Base Class
		/// </summary>
		protected DavVersionControlBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavVersionControlBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavVersionControlBase_InternalProcessDavRequest);
		}

		private int DavVersionControlBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			return (int)ServerResponseCode.Ok;
		}

		private int DavVersionControlBase_ValidateDavRequest(object sender, EventArgs e)
		{
			int _responseCode = (int)ServerResponseCode.Ok;

			if (base.RequestXml != null)
			{
				//Ensure the request is a version-control element
				XPathNodeIterator _propFindNodeIterator = base.RequestXml.SelectDescendants("version-control", "DAV:", false);
				if (!_propFindNodeIterator.MoveNext())
					_responseCode = (int)ServerResponseCode.BadRequest;
			}
			return _responseCode;
		}
	}
}