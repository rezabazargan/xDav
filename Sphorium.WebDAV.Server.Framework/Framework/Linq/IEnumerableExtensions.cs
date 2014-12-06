//-----------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs" company="Sphorium Technologies">
//     Copyright (c) Sphorium Technologies. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Sphorium.WebDAV.Server.Framework.Linq
{
	/// <summary>
	/// IEnumerable Extensions
	/// </summary>
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Performs the specified action on each element of the <![CDATA[IEnumerable<T>]]>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="action">The <![CDATA[System.Action<T>]]> delegate to perform on each element of the <![CDATA[IEnumerable<T>]]>.</param>
		/// <returns>source</returns>
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T element in source) action(element);
			return source;
		}
	}
}
