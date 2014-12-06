//-----------------------------------------------------------------------
// <copyright file="DavPropFindBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Xml.XPath;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource PROPFIND Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#METHOD_PROPFIND
	///		</code>
	///		
	///		<code>
	///			Returns ServerResponseCode.MultiStatus when successful
	///		</code>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>	
	public abstract class DavPropFindBase : DavResourceFindBase
	{
		/// <summary>
		/// Dav Resource PROPFIND Framework Base Class
		/// </summary>
		protected DavPropFindBase()
		{
			//Make sure the dav processing
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavPropFindBase_InternalProcessDavRequest);
			this.ValidateDavRequest += new DavRequestValidator(DavPropFindBase_ValidateDavRequest);

			this.RequestPropertyType = PropertyRequestType.None;
		}

		/// <summary>
		/// Property request types
		/// </summary>
		protected enum PropertyRequestType
		{
			/// <summary>
			/// No properties requested
			/// </summary>
			None,

			/// <summary>
			/// Specific properties requested
			/// <seealso cref="DavResourceFindBase.RequestProperties"/>
			/// </summary>
			NamedProperties,

			/// <summary>
			/// A summary of all the available properties
			/// </summary>
			PropertyNames,

			/// <summary>
			/// All properties requested
			/// </summary>
			AllProperties
		}

		/// <summary>
		/// Requested Property type
		/// <seealso cref="PropertyRequestType"/>
		/// </summary>
		protected PropertyRequestType RequestPropertyType { get; private set; }

		private int DavPropFindBase_ValidateDavRequest(object sender, EventArgs e)
		{
			int _returnCode = (int)ServerResponseCode.Ok;

			//Clear out all the collection resources
			base.FileResources.Clear();
			base.CollectionResources.Clear();

			//NOTE: An empty PROPFIND request body MUST be treated as a request for the names 
			//	and values of all properties.
			if (base.RequestXml == null)
				this.RequestPropertyType = PropertyRequestType.AllProperties;

			else
			{
				XPathNodeIterator _propFindNodeIterator = base.RequestXml.SelectDescendants("propfind", "DAV:", false);
				if (_propFindNodeIterator.MoveNext())
				{
					if (_propFindNodeIterator.Current.MoveToFirstChild())
					{
						switch (_propFindNodeIterator.Current.LocalName.ToLower(CultureInfo.InvariantCulture))
						{
							case "propnames":
								this.RequestPropertyType = PropertyRequestType.PropertyNames;
								break;
							case "allprop":
								this.RequestPropertyType = PropertyRequestType.AllProperties;
								break;
							default:
								this.RequestPropertyType = PropertyRequestType.NamedProperties;
								break;
						}
					}
					else
						_returnCode = (int)ServerResponseCode.BadRequest;
				}
				else
					_returnCode = (int)ServerResponseCode.BadRequest;
			}

			//Write to the debug log
			InternalFunctions.WriteDebugLog("DavPropFindBase: PropertyRequestType - " + this.RequestPropertyType);
			return _returnCode;
		}


		private int DavPropFindBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			int _returnCode = (int)ServerResponseCode.BadRequest;

			switch (this.RequestPropertyType)
			{
				case PropertyRequestType.AllProperties:
					////Fix for XP Web Folder mini-redirector
					//if (this.CollectionResources.Count == 0 && base.HttpApplication.Request.UserAgent == "Microsoft Data Access Internet Publishing Provider DAV")
					//{
					//    DavFolder _rootFolder = new DavFolder("", "/");
					//    this.CollectionResources.Add(_rootFolder);
					//}

					if (this.ResourceCount == 0)
						_returnCode = (int)ServerResponseCode.NotFound;
					else
						_returnCode = base.LoadNodes(null);
					break;

				case PropertyRequestType.NamedProperties:
					////Fix for XP Web Folder mini-redirector
					//if (this.CollectionResources.Count == 0 && base.HttpApplication.Request.UserAgent == "Microsoft Data Access Internet Publishing Provider DAV")
					//{
					//    DavFolder _rootFolder = new DavFolder("", "/");
					//    this.CollectionResources.Add(_rootFolder);
					//}

					if (this.ResourceCount == 0)
						_returnCode = (int)ServerResponseCode.NotFound;
					else
						_returnCode = base.LoadNodes(base.RequestProperties);
					break;

				case PropertyRequestType.PropertyNames:
					_returnCode = base.LoadNodePropertyNames();
					break;
			}

			return _returnCode;
		}
	}
}