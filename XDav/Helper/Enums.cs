using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Helper
{
    /// <summary>
    /// Lock owner type
    /// </summary>
    public enum LockOwnerType : int
    {
        /// <summary>
        /// User
        /// </summary>
        User,

        /// <summary>
        /// URI Lock owner
        /// </summary>
        Href
    }

    /// <summary>
    /// Lock Type
    /// </summary>
    public enum LockType : int
    {
        /// <summary>
        /// Read lock
        /// </summary>
        Read,

        /// <summary>
        /// Write lock
        /// </summary>
        Write
    }

    /// <summary>
    /// Lock Scope
    /// </summary>
    public enum LockScope : int
    {
        /// <summary>
        /// Shared lock
        /// </summary>
        Shared,

        /// <summary>
        /// Exclusive lock
        /// </summary>
        Exclusive
    }

    public enum DepthType : int
    {
        /// <summary>
        /// The method is applied only to the resource
        /// </summary>
        ResourceOnly = 0,

        /// <summary>
        /// The method is applied to the resource and to its immediate children
        /// </summary>
        ResourceChildren = 1,

        /// <summary>
        /// The method is applied to the resource and to all of its children
        /// </summary>
        Infinity = 2
    }

    public enum ResourceType : int
    {
        /// <summary>
        /// Collection Resource
        /// </summary>
        Collection,

        /// <summary>
        /// File Resource
        /// </summary>
        Resource
    }
}
