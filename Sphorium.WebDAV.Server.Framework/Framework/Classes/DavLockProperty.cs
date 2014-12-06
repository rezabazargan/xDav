//-----------------------------------------------------------------------
// <copyright file="DavLockProperty.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;

namespace Sphorium.WebDAV.Server.Framework.Classes
{
	/// <summary>
	/// WebDav Lock Property.
	/// </summary>
	[Serializable]
	public class DavLockProperty : ICloneable
	{
		private bool __readOnly;
		private string __lockOwner = "";
		private int __lockTimeout = 180;
		private LockType __lockType = LockType.Read;
		private List<string> __lockTokens = new List<string>();
		private LockScope __lockScope = LockScope.Exclusive;
		private DepthType __lockDepth = DepthType.Infinity;
		private LockOwnerType __lockOwnerType = LockOwnerType.User;

		/// <summary>
		/// WebDav Lock Property.
		/// </summary>
		public DavLockProperty() { }

		/// <summary>
		/// Check to see if this object is read only
		/// </summary>
		/// <returns></returns>
		private bool IsReadOnly()
		{
			return IsReadOnly(true);
		}

		/// <summary>
		/// Check to see if this object is read only
		/// </summary>
		/// <returns></returns>
		private bool IsReadOnly(bool throwError)
		{
			bool _isReadOnly = false;
			if (this.__readOnly)
			{
				_isReadOnly = true;

				if (throwError)
					throw new WebDavException(InternalFunctions.GetResourceString("ReadOnlyException"));
			}

			return _isReadOnly;
		}

		#region Properties
			/// <summary>
			/// WebDav Lock Type.
			/// </summary>
			public LockType LockType
			{
				get
				{
					return this.__lockType;
				}
				set
				{
					if (!IsReadOnly())
						this.__lockType = value;
				}
			}

			/// <summary>
			/// WebDav Lock Scope
			/// </summary>
			public LockScope LockScope
			{
				get
				{
					return this.__lockScope;
				}
				set
				{
					if (!IsReadOnly())
						this.__lockScope = value;
				}
			}

			/// <summary>
			/// Lock depth
			/// </summary>
			public DepthType LockDepth
			{
				get
				{
					return this.__lockDepth;
				}
				set
				{
					if (!IsReadOnly())
						this.__lockDepth = value;
				}
			}

			/// <summary>
			/// Lock owner
			/// </summary>
			public string LockOwner
			{
				get
				{
					return this.__lockOwner;
				}
				set
				{
					if (!IsReadOnly())
						this.__lockOwner = value;
				}
			}

			/// <summary>
			/// Lock owner type
			/// </summary>
			public LockOwnerType LockOwnerType
			{
				get
				{
					return this.__lockOwnerType;
				}
				set
				{
					if (!IsReadOnly())
						this.__lockOwnerType = value;
				}
			}

			/// <summary>
			/// Lock timeout in seconds
			/// </summary>
			public int LockTimeout
			{
				get
				{
					return this.__lockTimeout;
				}
				set
				{
					if (!IsReadOnly())
						this.__lockTimeout = value;
				}
			}

			/// <summary>
			/// Lock token
			/// </summary>
			public string[] GetLockTokens()
			{
				return this.__lockTokens.ToArray();
			}
		#endregion

		#region Methods
			/// <summary>
			/// Clear the current lock tokens
			/// </summary>
			public void ClearLockTokens()
			{
				if (!IsReadOnly())
					this.__lockTokens.Clear();
			}

			/// <summary>
			/// Add a new lock token 
			/// </summary>
			/// <param name="opaqueLockToken"></param>
			public void AddLockToken(string opaqueLockToken)
			{
				if (!IsReadOnly())
				{
					//Ensure the opaqueLockToken is not already added
					if (!this.__lockTokens.Contains(opaqueLockToken))
						this.__lockTokens.Add(opaqueLockToken);
				}
			}
		#endregion

		#region Internal Methods
			/// <summary>
			/// Toggles the write capabilities on this object
			/// </summary>
			internal bool ReadOnly
			{
				get
				{
					return this.__readOnly;
				}
				set
				{
					this.__readOnly = value;
				}
			}

			/// <summary>
			/// Copies an existing LockProperty
			/// </summary>
			/// <param name="lockProperty"></param>
			internal void Copy(DavLockProperty lockProperty)
			{
				//Clone the properties
				this.LockDepth = lockProperty.LockDepth;
				this.LockOwner = lockProperty.LockOwner;
				this.LockOwnerType = lockProperty.LockOwnerType;
				this.LockScope = lockProperty.LockScope;
				this.LockTimeout = lockProperty.LockTimeout;
				this.LockType = lockProperty.LockType;
				this.ReadOnly = lockProperty.ReadOnly;

				//Clone the collections
				this.ClearLockTokens();

				foreach (string _token in lockProperty.GetLockTokens())
					this.AddLockToken(_token);
			}

			/// <summary>
			/// Obtain the Xml representation of the lock information
			/// </summary>
			internal XmlNode ActiveLock
			{
				get
				{
					//TODO: Replace this so we don't use XmlDoc

					//Build the XmlDocument and retrieve the requested node
					XmlDocument _xmlDocument = new XmlDocument();
					
					//XmlNode _activeLockNode = _xmlDocument.CreateNode(XmlNodeType.Element, _xmlDocument.GetPrefixOfNamespace("DAV:"), "activelock", "DAV:");
					XmlElement _activeLockNode = _xmlDocument.CreateElement("D", "activelock", "DAV:");
					XmlElement _lockTypeElement = _xmlDocument.CreateElement("D", "locktype", "DAV:");

					switch (this.LockType)
					{
						case LockType.Read:
							_lockTypeElement.AppendChild(_xmlDocument.CreateElement("D", "read", "DAV:"));
							break;

						case LockType.Write:
							_lockTypeElement.AppendChild(_xmlDocument.CreateElement("D", "write", "DAV:"));
							break;
					}

					XmlElement _lockScopeElement = _xmlDocument.CreateElement("D", "lockscope", "DAV:");
					switch (this.LockScope)
					{
						case LockScope.Exclusive:
							_lockScopeElement.AppendChild(_xmlDocument.CreateElement("D", "exclusive", "DAV:"));
							break;

						case LockScope.Shared:
							_lockScopeElement.AppendChild(_xmlDocument.CreateElement("D", "shared", "DAV:"));
							break;
					}

					_activeLockNode.AppendChild(_lockScopeElement);
					_activeLockNode.AppendChild(_lockTypeElement);

					//Append the depth
					XmlElement _depthElement = _xmlDocument.CreateElement("D", "depth", "DAV:");

					if (LockDepth == DepthType.Infinity)
						_depthElement.InnerText = this.__lockDepth.ToString();
					else
						_depthElement.InnerText = (string)System.Enum.Parse(LockDepth.GetType(), LockDepth.ToString(), true);
					_activeLockNode.AppendChild(_depthElement);

					//Append the owner
					XmlElement _lockOwner = _xmlDocument.CreateElement("D", "owner", "DAV:");
					switch (this.LockOwnerType)
					{
						case LockOwnerType.User:
							_lockOwner.InnerText = this.LockOwner;
							break;
						case LockOwnerType.Href:
							XmlElement _hrefLockTokenElement = _xmlDocument.CreateElement("D", "href", "DAV:");
							_hrefLockTokenElement.InnerText = this.LockOwner;
							_lockOwner.AppendChild(_hrefLockTokenElement);
							break;
					}
					_activeLockNode.AppendChild(_lockOwner);

					//Append the timeout
					XmlElement _timeoutElement = _xmlDocument.CreateElement("D", "timeout", "DAV:");
					_timeoutElement.InnerText = "Second-" + this.LockTimeout;
					_activeLockNode.AppendChild(_timeoutElement);

					//Append the lockToken
					XmlElement _lockToken = _xmlDocument.CreateElement("D", "locktoken", "DAV:");
					foreach (string _token in this.GetLockTokens())
					{
						XmlElement _opaqueLockTokenElement = _xmlDocument.CreateElement("D", "href", "DAV:");
						_opaqueLockTokenElement.InnerText = "opaquelocktoken:" + _token;
						_lockToken.AppendChild(_opaqueLockTokenElement);
					}
					_activeLockNode.AppendChild(_lockToken);

					return _activeLockNode;
				}
			}
		#endregion

		#region ICloneable Members
			// Explicit interface method impl
			object ICloneable.Clone()
			{
				return this.Clone();
			}

			/// <summary>
			/// DavLockProperty Clone
			/// </summary>
			/// <remarks>Deep copy</remarks>
			/// <returns></returns>
			public DavLockProperty Clone()
			{
				// Start with a flat, memberwise copy
				DavLockProperty _davLockProperty = (DavLockProperty)this.MemberwiseClone();
				_davLockProperty.__lockTokens.AddRange(this.__lockTokens);

				return _davLockProperty;
			}
		#endregion
	}
}


