using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XDav.Helper;

namespace XDav.RequestHandler
{
    public class PutRequest : RequestBase
    {
        internal override Func<System.Web.HttpContext, bool> Condition
        {
            get { return r => r.Request.HttpMethod.ToLower() == "put"; }
        }


        protected override void Handle()
        {
            var ms = new MemoryStream();
            Context.Request.InputStream.CopyTo(ms);
            byte[] _requestInput = ms.ToArray();
            using (FileStream _newFile = new FileStream(base.FullPath, FileMode.OpenOrCreate))
            {
                _newFile.Position = 0;
                _newFile.Write(_requestInput, 0, _requestInput.Length);
                _newFile.Close();
            }
            Context.SetStatus(StatusCode.OK);
        }
    }
}
