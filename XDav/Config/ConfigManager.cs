using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XDav.Config
{
    public class ConfigManager
    {
        public static XDavConfig XDavConfig
        {
            get
            {
                return (XDavConfig)System.Configuration.ConfigurationManager.GetSection("XDavConfig");
            }
        }

        public static string DavPath
        {
            get
            {
                var location = XDavConfig.FileLocation;
                switch (location.PathType)
                {
                    case PathType.Server:
                        return location.URL;
                    case PathType.Local:
                        return HttpContext.Current.Server.MapPath(location.URL);
                    default:
                        return location.URL;
                }
            }
        }
    }
}
