//-----------------------------------------------------------------------
// <copyright file="IDavRequest.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Sphorium.WebDAV.Server.Framework.Interfaces
{
	/// <summary>
	/// Summary description for IDavRequest.
	/// </summary>
	internal interface IDavRequest
	{
		int ProcessRequest();
	}
}