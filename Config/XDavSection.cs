using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Config
{
    public class XDavConfig : ConfigurationSection
    {
        [ConfigurationProperty("FileLocation")]
        public FileLocation FileLocation
        {
            get
            {
                return (FileLocation)this["FileLocation"];
            }
            set
            {
                this["FileLocation"] = value;
            }
        }
    }
}
