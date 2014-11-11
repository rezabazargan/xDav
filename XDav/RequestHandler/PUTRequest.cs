using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XDav.RequestHandler
{
    public class PUTRequest : RequestBase
    {
        internal override Func<System.Web.HttpContext, bool> Condition
        {
            get { return r => r.Request.HttpMethod.ToLower() == "put"; }
        }


        protected override void Handle()
        {
            throw new NotImplementedException();
        }
    }
}
