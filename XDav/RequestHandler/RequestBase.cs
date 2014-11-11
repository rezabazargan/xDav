using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XDav.RequestHandler
{
    public abstract class RequestBase
    {

        internal HttpContext Context;

        internal abstract Func<HttpContext, bool> Condition { get; }
        
        public void HandelRequest(HttpContext context)
        {
            Context = context;
            Handle();
        }
        
        protected abstract void Handle();

        protected string FullPath
        {
            get
            {
                return XDav.Config.ConfigManager.DavPath + FileName;
            }
        }

        protected string FileName
        {
            get
            {
                return Context.Request.Url.LocalPath.Replace(string.Format("/{0}/", XDav.Config.ConfigManager.XDavConfig.Name), "");
            }
        }
    }
}
