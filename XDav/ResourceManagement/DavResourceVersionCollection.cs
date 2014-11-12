using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.ResourceManagement
{
    [Serializable]
    public class DavResourceVersionCollection : Collection<DavResourceVersion>, ICloneable
    {
        /// <summary>
        /// WebDav Lock Property Collection.
        /// </summary>
        public DavResourceVersionCollection() { }

        #region ICloneable Members
        // Explicit interface method impl
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// DavResourceVersionCollection Clone
        /// </summary>
        /// <remarks>Deep copy</remarks>
        /// <returns></returns>
        public DavResourceVersionCollection Clone()
        {
            // Start with a flat, memberwise copy
            DavResourceVersionCollection _davResourceVersionCollection = new DavResourceVersionCollection();

            // Then deep-copy everything that needs the 
            foreach (DavResourceVersion _davResourceVersion in this)
                _davResourceVersionCollection.Add(_davResourceVersion.Clone());

            return _davResourceVersionCollection;
        }
        #endregion
    }
}
