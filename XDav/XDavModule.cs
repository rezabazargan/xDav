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
                if (context.Request.Url.LocalPath.StartsWith(string.Format("/{0}/",XDav.Config.ConfigManager.XDavConfig.Name)))
                {
                    new XDav.RequestHandler.RequestManager().HandleRequest(context.Context);
                    context.CompleteRequest();
                }
            };

        }

    }
}
