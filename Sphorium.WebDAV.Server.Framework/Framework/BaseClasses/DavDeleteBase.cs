//-----------------------------------------------------------------------
// <copyright file="DavDeleteBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Sphorium.WebDAV.Server.Framework.Collections;
using Sphorium.WebDAV.Server.Framework.Interfaces;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource Delete Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#METHOD_DELETE
	///		</code>
	///		
	///		<code>
	///			Returns DavDeleteResponseCode.NoContent when successful
	///		</code>
	///		<seealso cref="DavDeleteResponseCode"/>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>	
	public abstract class DavDeleteBase : DavMethodBase, IDavRequestProcessError
	{
		/// <summary>
		/// Dav Resource Delete Framework Base Class
		/// </summary>
		protected DavDeleteBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavDeleteBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavDeleteBase_InternalProcessDavRequest);

			this.ProcessErrors = new DavProcessErrorCollection();
		}

		/// <summary>
		/// WebDav DELETE Response Codes
		/// </summary>
		protected enum DavDeleteResponseCode : int
		{
			/// <summary>
			/// 0: None
			/// </summary>
			/// <remarks>
			///		Default enumerator value
			/// </remarks>
			None = 0,

			/// <summary>
			/// 102: Processing
			/// </summary>
			Processing = 102,

			/// <summary>
			/// 204: NoContent
			/// </summary>
			/// <remarks>
			///		The delete command completed successfully
			///	</remarks>
			NoContent = 204,

			/// <summary>
			/// 423: Locked
			/// </summary>
			/// <remarks>
			///		The resource is locked
			///	</remarks>
			Locked = 423
		}

		/// <summary>
		/// Ensure the Dav request is valid
		/// </summary>
		/// <returns></returns>
		private int DavDeleteBase_ValidateDavRequest(object sender, EventArgs e)
		{
			int _status = (int)ServerResponseCode.Ok;

			if (base.RequestDepth != DepthType.Infinity)
				_status = (int)ServerResponseCode.BadRequest;

			if (base.RequestLength != 0)
				_status = (int)ServerResponseCode.BadRequest;

			return _status;
		}

		private int DavDeleteBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			int _responseCode = (int)DavDeleteResponseCode.NoContent;

			//Check to see if there were any process errors...
			Enum[] _errorResources = this.ProcessErrorResources;
			if (_errorResources.Length > 0)
			{
				base.SetResponseXml(InternalFunctions.ProcessErrorRequest(this.ProcessErrors));
				_responseCode = (int)ServerResponseCode.MultiStatus;
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