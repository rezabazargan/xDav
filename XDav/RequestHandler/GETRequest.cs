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
    public class GetRequest :RequestBase
    {
        internal override Func<System.Web.HttpContext, bool> Condition
        {
            get { return r => r.Request.HttpMethod.ToLower() == "get"; }
        }
        

        protected override void Handle()
        {
            DirectoryInfo _dirInfo = new DirectoryInfo(ConfigManager.DavPath);
            FileInfo _fileInfo = _dirInfo.GetFiles().First(f => f.Name == FileName); 

            using (var s = new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                var ms = new MemoryStream();
                s.CopyTo(ms);
                s.Position = 0;
                s.CopyTo(base.Context.Response.OutputStream);
            }
        }

       
    }
}
