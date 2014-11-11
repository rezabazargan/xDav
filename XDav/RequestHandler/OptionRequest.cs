using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.RequestHandler
{
    public class OptionRequest : RequestBase
    {
        internal override Func<System.Web.HttpContext, bool> Condition
        {
            get { return r => r.Request.HttpMethod.ToLower() == "option"; }
        }

        protected override void Handle()
        {
            throw new NotImplementedException();
        }
    }
}
