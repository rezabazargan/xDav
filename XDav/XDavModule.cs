using Sphorium.WebDAV.Server.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XDav.Helper;

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
                if (context.Request.Url.LocalPath.StartsWith(string.Format("/{0}/",XDav.Config.ConfigManager.XDavConfig.Name)))
                {
                    FileWrapper.Create(context.Context);
                    WebDavProcessor p = new WebDavProcessor(Assembly.GetExecutingAssembly());
                    p.ProcessRequest(context);
                }
            };

        }

    }
}
