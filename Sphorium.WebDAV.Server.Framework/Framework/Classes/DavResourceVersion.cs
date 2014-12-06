//-----------------------------------------------------------------------
// <copyright file="DavResourceVersion.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Sphorium.WebDAV.Server.Framework.Classes
{

	/// <summary>
	/// WebDav Lock Property.
	/// </summary>
	[Serializable]
	public class DavResourceVersion : ICloneable
	{
		private string __comment = "";

		/// <summary>
		/// WebDav Resource Version
		/// </summary>
		public DavResourceVersion() { }

		/// <summary>
		/// Resource comment
		/// </summary>
		public string Comment
		{
			get
			{
				return this.__comment;
			}
			set
			{
				this.__comment = value;
			}
		}


		//
		//			
		//			/// <summary>
		//			/// Obtain the Xml representation of the lock information
		//			/// </summary>
		//			internal XmlNode ActiveLock {
		//				get {
		//					//TODO: Replace this so we don't use XmlDoc
		//
		//					//Build the XmlDocument and retrieve the requested node
		//					XmlDocument _xmlDocument = new XmlDocument();
		//					XmlNode _activeLockNode = _xmlDocument.CreateNode(XmlNodeType.Element, _xmlDocument.GetPrefixOfNamespace("DAV:"), "activelock", "DAV:");
		//					XmlElement _lockTypeElement = _xmlDocument.CreateElement("locktype", "DAV:");
		//					switch (this.LockType) {
		//						case LockType.Read:
		//							_lockTypeElement.AppendChild(_xmlDocument.CreateElement("read", "DAV:"));
		//							break;
		//
		//						case LockType.Write:
		//							_lockTypeElement.AppendChild(_xmlDocument.CreateElement("write", "DAV:"));
		//							break;
		//					}
		//
		//					XmlElement _lockScopeElement = _xmlDocument.CreateElement("lockscope", "DAV:");
		//					switch (this.LockScope) {
		//						case LockScope.Exclusive:
		//							_lockScopeElement.AppendChild(_xmlDocument.CreateElement("exclusive", "DAV:"));
		//							break;
		//
		//						case LockScope.Shared:
		//							_lockScopeElement.AppendChild(_xmlDocument.CreateElement("shared", "DAV:"));
		//							break;
		//					}
		//
		//					_activeLockNode.AppendChild(_lockScopeElement);
		//					_activeLockNode.AppendChild(_lockTypeElement);
		//
		//					//Append the depth
		//					XmlElement _depthElement = _xmlDocument.CreateElement("depth", "DAV:");
		//
		//					if (LockDepth == DepthType.Infinity) 
		//						_depthElement.InnerText = _lockDepth.ToString();
		//					else
		//						_depthElement.InnerText = (string)System.Enum.Parse(LockDepth.GetType(), LockDepth.ToString(), true);
		//					_activeLockNode.AppendChild(_depthElement);
		//
		//					//Append the owner
		//					XmlElement _lockOwner = _xmlDocument.CreateElement("owner", "DAV:");
		//					switch (this.LockOwnerType) {
		//						case LockOwnerType.User:
		//							_lockOwner.InnerText = this.LockOwner;
		//							break;
		//						case LockOwnerType.Href:
		//							XmlElement _hrefLockTokenElement = _xmlDocument.CreateElement("href", "DAV:");
		//							_hrefLockTokenElement.InnerText = this.LockOwner;
		//							_lockOwner.AppendChild(_hrefLockTokenElement);
		//							break;
		//					}
		//					_activeLockNode.AppendChild(_lockOwner);
		//
		//					//Append the timeout
		//					XmlElement _timeoutElement = _xmlDocument.CreateElement("timeout", "DAV:");
		//					_timeoutElement.InnerText = "Second-" + this.LockTimeout;
		//					_activeLockNode.AppendChild(_timeoutElement);
		//
		//					//Append the lockToken
		//					XmlElement _lockToken = _xmlDocument.CreateElement("locktoken", "DAV:");
		//					foreach (string _token in this.GetLockTokens()) {
		//						XmlElement _opaqueLockTokenElement = _xmlDocument.CreateElement("href", "DAV:");
		//						_opaqueLockTokenElement.InnerText = "opaquelocktoken:" + _token;
		//						_lockToken.AppendChild(_opaqueLockTokenElement);
		//					}
		//					_activeLockNode.AppendChild(_lockToken);
		//
		//					return _activeLockNode;
		//				}
		//			}



		#region ICloneable Members
		// Explicit interface method impl
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// DavResourceVersion Clone
		/// </summary>
		/// <remarks>Deep copy</remarks>
		/// <returns></returns>
		public DavResourceVersion Clone()
		{
			// Start with a flat, memberwise copy
			DavResourceVersion _davResourceVersion = (DavResourceVersion)this.MemberwiseClone();
			return _davResourceVersion;
		}
		#endregion

	}
}


