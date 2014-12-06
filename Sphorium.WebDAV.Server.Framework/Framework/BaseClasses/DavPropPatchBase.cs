//-----------------------------------------------------------------------
// <copyright file="DavPropPatchBase.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Sphorium.WebDAV.Server.Framework.Classes;
using Sphorium.WebDAV.Server.Framework.Collections;
using Sphorium.WebDAV.Server.Framework.Linq;

namespace Sphorium.WebDAV.Server.Framework.BaseClasses
{
	/// <summary>
	/// Dav Resource PROPPATCH Framework Base Class
	/// </summary>
	/// <remarks>
	///		RFC2518 Compliant
	///		
	///		<code>
	///		The ProcessDavRequest event must follow the following rules addressed in RFC2518
	///			http://www.webdav.org/specs/rfc2518.html#METHOD_PROPPATCH
	///			
	///			Instructions MUST either all be executed or none executed. Thus if any error 
	///			occurs during processing all executed instructions MUST be undone and a proper 
	///			error result returned.
	///		</code>
	///		
	///		<code>
	///			Returns ServerResponseCode.MultiStatus when successful
	///		</code>
	///		<seealso cref="DavMethodBase.AbortRequest(System.Enum)"/>
	/// </remarks>	
	public abstract class DavPropPatchBase : DavMethodBase
	{
		/// <summary>
		/// Dav Resource PROPPATCH Framework Base Class
		/// </summary>
		protected DavPropPatchBase()
		{
			this.ValidateDavRequest += new DavRequestValidator(DavPropPatchBase_ValidateDavRequest);
			this.InternalProcessDavRequest += new DavInternalProcessHandler(DavPropPatchBase_InternalProcessDavRequest);
		}

		/// <summary>
		/// WebDav PROPPATCH Response Codes
		/// </summary>
		protected enum DavPropPatchResponseCode : int
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
			/// <remarks>The command succeeded</remarks>
			Ok = 200,

			/// <summary>
			/// 403: Forbidden
			/// </summary>
			/// <remarks>
			/// The client cannot alter one of the properties
			/// </remarks>
			Forbidden = 403,

			/// <summary>
			/// 409: Conflict
			/// </summary>
			/// <remarks>
			///		The client has provided a value whose semantics are not appropriate for the property. 
			///		This includes trying to set read-only properties.
			/// </remarks>
			Conflict = 409,

			/// <summary>
			/// 423: Locked
			/// </summary>
			/// <remarks>
			///		The specified resource is locked and the client either is not a lock owner or 
			///		the lock type requires a lock token to be submitted and the client did not submit it.
			/// </remarks>
			Locked = 423,

			/// <summary>
			/// 424: FailedDependency
			/// </summary>
			/// <remarks>
			///		Implies the action would have succeeded by itself
			/// </remarks>
			FailedDependency = 424,

			/// <summary>
			/// 507: Insufficient Storage
			/// </summary>
			/// <remarks>
			///		The server did not have sufficient space to record the property
			/// </remarks>
			InsufficientStorage = 507
		}

		/// <summary>
		/// Dav resource to apply the property changes
		/// </summary>
		protected DavResourceBase PatchedResource { get; set; }

		/// <summary>
		/// Requested properties to delete
		/// </summary>
		protected DavPropertyCollection RequestDeleteProperties
		{
			get
			{
				DavPropertyCollection _davProperties = new DavPropertyCollection();

				if (base.RequestXml != null)
				{
					XPathNodeIterator _propDeleteNodeIterator = base.RequestXml.SelectDescendants("remove", "DAV:", false);
					if (_propDeleteNodeIterator.MoveNext())
					{

						//Move to the prop node if it exists
						_propDeleteNodeIterator = _propDeleteNodeIterator.Current.SelectDescendants("prop", "DAV:", false);
						if (_propDeleteNodeIterator.MoveNext())
						{
							XPathNodeIterator _nodeChildren = _propDeleteNodeIterator.Current.SelectChildren(XPathNodeType.All);
							while (_nodeChildren.MoveNext())
							{
								XPathNavigator _currentNode = _nodeChildren.Current;

								if (_currentNode.NodeType == XPathNodeType.Element)
									_davProperties.Add(new DavProperty(_currentNode));
							}
						}
					}
				}
				return _davProperties;
			}
		}

		/// <summary>
		/// Requested properties to add / modify
		/// </summary>
		protected DavPropertyCollection RequestModifyProperties
		{
			get
			{
				DavPropertyCollection _davProperties = new DavPropertyCollection();

				if (base.RequestXml != null)
				{
					XPathNodeIterator _propModifyNodeIterator = base.RequestXml.SelectDescendants("set", "DAV:", false);
					if (_propModifyNodeIterator.MoveNext())
					{
						//Move to the prop node if it exists
						_propModifyNodeIterator = _propModifyNodeIterator.Current.SelectDescendants("prop", "DAV:", false);
						if (_propModifyNodeIterator.MoveNext())
						{
							XPathNodeIterator _nodeChildren = _propModifyNodeIterator.Current.SelectChildren(XPathNodeType.All);
							while (_nodeChildren.MoveNext())
							{
								XPathNavigator _currentNode = _nodeChildren.Current;

								if (_currentNode.NodeType == XPathNodeType.Element)
									_davProperties.Add(new DavProperty(_currentNode));
							}
						}
					}
				}
				return _davProperties;
			}
		}

		private int DavPropPatchBase_ValidateDavRequest(object sender, EventArgs e)
		{
			if (base.RequestLength == 0)
				return (int)ServerResponseCode.BadRequest;

			return (int)DavPropPatchResponseCode.Ok;
		}

		private int DavPropPatchBase_InternalProcessDavRequest(object sender, EventArgs e)
		{
			if (this.PatchedResource == null)
				base.AbortRequest(ServerResponseCode.NotFound);
			else
			{
				using (Stream _responseStream = new MemoryStream())
				{
					XmlTextWriter _xmlWriter = new XmlTextWriter(_responseStream, new UTF8Encoding(false));

					_xmlWriter.Formatting = Formatting.Indented;
					_xmlWriter.IndentChar = '\t';
					_xmlWriter.Indentation = 1;
					_xmlWriter.WriteStartDocument();

					//Set the Multistatus
					_xmlWriter.WriteStartElement("D", "multistatus", "DAV:");

					//Open the response element
					_xmlWriter.WriteStartElement("response", "DAV:");

					//Load the valid items HTTP/1.1 200 OK
					_xmlWriter.WriteElementString("href", "DAV:", this.PatchedResource.ResourcePath);

					//Check to see what properties have been added / removed
					this.RequestDeleteProperties
						.Concat(this.RequestModifyProperties)
						.ForEach
						(
							_prop =>
							{
								//Open the propstat element section
								_xmlWriter.WriteStartElement("propstat", "DAV:");

								//Open the prop element section
								_xmlWriter.WriteStartElement("prop", "DAV:");

								_xmlWriter.WriteStartElement(_prop.Name, _prop.Namespace);
								_xmlWriter.WriteEndElement();

								//Close the prop element section
								_xmlWriter.WriteEndElement();

								int _count = this.PatchedResource.CustomProperties
													.Count
													(
														p => p.Name == _prop.Name &&
																p.Namespace == _prop.Namespace
													);

								if (_count == 0)
									_xmlWriter.WriteElementString("status", "DAV:", InternalFunctions.GetEnumHttpResponse(ServerResponseCode.NotFound));
								else
									_xmlWriter.WriteElementString("status", "DAV:", InternalFunctions.GetEnumHttpResponse(ServerResponseCode.Ok));

								//Close the propstat element section
								_xmlWriter.WriteEndElement();
							}
						);

					//Close the response element
					_xmlWriter.WriteEndElement();

					//Close the Multistatus element
					_xmlWriter.WriteEndElement();
					_xmlWriter.WriteEndDocument();
					_xmlWriter.Flush();

					base.SetResponseXml(_responseStream);
					_xmlWriter.Close();
				}
			}

			return (int)ServerResponseCode.MultiStatus;
		}
	}
}