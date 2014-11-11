using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
        }
    }
}
