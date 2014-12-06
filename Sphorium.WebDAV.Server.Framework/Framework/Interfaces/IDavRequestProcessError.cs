//-----------------------------------------------------------------------
// <copyright file="IDavRequestProcessError.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

using Sphorium.WebDAV.Server.Framework.BaseClasses;

namespace Sphorium.WebDAV.Server.Framework.Interfaces
{
	/// <summary>
	/// Summary description for IDavRequestProcessError.
	/// </summary>
	internal interface IDavRequestProcessError
	{
		/// <summary>
		/// Clear the process errors
		/// </summary>
		void ClearProcessErrors();

		/// <summary>
		/// Add a new process error
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="errorCode"></param>
		void AddProcessErrorResource(DavResourceBase resource, System.Enum errorCode);

		/// <summary>
		/// Retrieve the current process errors
		/// </summary>
		Enum[] ProcessErrorResources
		{
			get;
		}
	}
}