using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDav.Config;
using XDav.Helper;

namespace XDav.RequestHandler
{
    public class UnlockRequest : RequestBase
    {
        internal override Func<System.Web.HttpContext, bool> Condition
        {
            get { return r => r.Request.HttpMethod.ToLower() == "unlock"; }
        }

        protected override void Handle()
        {
            DirectoryInfo _dirInfo = new DirectoryInfo(ConfigManager.DavPath);

            Context.SetStatus(StatusCode.OK);
            using(FileStream fs = new FileStream(FullPath,FileMode.OpenOrCreate))
            {
                fs.Unlock(0, fs.Length);
            }
            Context.SetStatus(StatusCode.OK);
        }

     
    }



}
