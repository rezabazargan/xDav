using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.ResourceManagement
{
    [Serializable]
    public class DavResourceVersion : ICloneable
    {
        private string _comment = "";

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
                return this._comment;
            }
            set
            {
                this._comment = value;
            }
        }





        #region ICloneable Members
        // Explicit interface method impl
        object ICloneable.Clone()
        {
            return this.Clone();
        }


        public DavResourceVersion Clone()
        {
            // Start with a flat, memberwise copy
            DavResourceVersion _davResourceVersion = (DavResourceVersion)this.MemberwiseClone();
            return _davResourceVersion;
        }
        #endregion

    }
}
