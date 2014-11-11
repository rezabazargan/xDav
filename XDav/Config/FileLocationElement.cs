using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Config
{
    public class FileLocation : ConfigurationElement
    {
        [ConfigurationProperty("PathType", DefaultValue = PathType.Server, IsRequired = true)]
        public PathType PathType {
            get
            {
                return (Config.PathType)this["PathType"];
            }
            set
            {
                this["PathType"] = value;
            }
        }


         [ConfigurationProperty("URL", DefaultValue = "/xDav/", IsRequired = true)]
        public string URL
        {
            get
            {
                return (string)this["URL"];
            }
            set
            {
                this["URL"] = value;
            }
        }

        
    }


    public enum PathType
    {
        Server,
        Local
    }

}
