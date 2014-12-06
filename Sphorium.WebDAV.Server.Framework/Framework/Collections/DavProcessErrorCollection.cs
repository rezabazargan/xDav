//-----------------------------------------------------------------------
// <copyright file="DavProcessErrorCollection.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;

using Sphorium.WebDAV.Server.Framework.BaseClasses;

namespace Sphorium.WebDAV.Server.Framework.Collections
{
	/// <summary>
	/// Summary description for ProcessErrorCollection.
	/// </summary>
	internal class DavProcessErrorCollection : DictionaryBase
	{
		internal DavProcessErrorCollection()
		{

		}

		/// <summary>
		/// Add a resource
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="errorCode"></param>
		/// <exception cref="WebDavException">Throw exception if the errorCode value is not a valid Int32</exception>
		internal void Add(DavResourceBase resource, Enum errorCode)
		{
			if (InternalFunctions.ValidateEnumType(errorCode))
			{
				if (base.Dictionary[errorCode] == null)
					base.Dictionary[errorCode] = new ArrayList();

				((ArrayList)base.Dictionary[errorCode]).Add(resource);
			}
		}

		/// <summary>
		/// Retrieve all the resources with a particular errorCode
		/// </summary>
		/// <exception cref="WebDavException">Throw exception if the errorCode value is not a valid Int32</exception>
		internal DavResourceBase[] this[Enum errorCode]
		{
			get
			{
				DavResourceBase[] _davResources = null;
				if (InternalFunctions.ValidateEnumType(errorCode))
				{
					ArrayList _errorResources = new ArrayList((ArrayList)base.Dictionary[errorCode]);
					_davResources = (DavResourceBase[])_errorResources.ToArray(typeof(DavResourceBase));
				}

				return _davResources;
			}
		}

		/// <summary>
		/// Retrieve all the resource errors
		/// </summary>
		internal Enum[] AllResourceErrors
		{
			get
			{
				ArrayList _resourceErrorCodes = new ArrayList(base.InnerHashtable.Keys);
				return (Enum[])_resourceErrorCodes.ToArray(typeof(Enum));
			}
		}
	}
}
