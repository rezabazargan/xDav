using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XDav
{
    public class XDavModule : IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (source,e) => {
                HttpApplication application = (HttpApplication)source;
                if (application.Request.Url.LocalPath.StartsWith("/XDav/"))
                {
                    //string url = XDav.Config.ConfigManager.XDavConfig.FileLocation.URL;
                    application.Context.Response.Write("<h1><font color=red>xDavModul: Beginning of Request for "+ XDav.Config.ConfigManager.DavPath +" </font></h1><hr>");
                    application.CompleteRequest();
                }
            };

        }

    }
}
