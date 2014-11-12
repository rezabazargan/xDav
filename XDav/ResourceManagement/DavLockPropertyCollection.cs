using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.ResourceManagement
{
    [Serializable]
    public class DavLockPropertyCollection : Collection<DavLockProperty>, ICloneable
    {
        /// <summary>
        /// WebDav Lock Property Collection.
        /// </summary>
        public DavLockPropertyCollection() { }

        #region ICloneable Members
        // Explicit interface method impl
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// DavLockPropertyCollection Clone
        /// </summary>
        /// <remarks>Deep copy</remarks>
        /// <returns></returns>
        public DavLockPropertyCollection Clone()
        {
            // Start with a flat, memberwise copy
            DavLockPropertyCollection _davLockPropertyCollection = new DavLockPropertyCollection();

            // Then deep-copy everything that needs the 
            foreach (DavLockProperty _davLockProperty in this)
                _davLockPropertyCollection.Add(_davLockProperty.Clone());

            return _davLockPropertyCollection;
        }
        #endregion
    }
}
