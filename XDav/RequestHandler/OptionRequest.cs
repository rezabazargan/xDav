using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDav.Helper;

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

            StringBuilder _allowedVerbs = new StringBuilder();
            foreach (string enumName in Enum.GetNames(typeof(HttpVerb)))
            {
                HttpVerb httpVerb = (HttpVerb)Enum.Parse(typeof(HttpVerb), enumName, true);
                _allowedVerbs.Append(enumName.ToUpper() + ", ");
            }

            base.Context.Response.Headers.Add("DAV", "1, 2, 3");
            base.Context.Response.Headers.Add("Public", _allowedVerbs.ToString());
            base.Context.Response.Headers.Add("Allow", _allowedVerbs.ToString());

            Context.SetStatus(StatusCode.OK);

        }
    }
}
