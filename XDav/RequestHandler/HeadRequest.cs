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
    public class HeadRequest : RequestBase
    {
        internal override Func<System.Web.HttpContext, bool> Condition
        {
            get { return r => r.Request.HttpMethod.ToLower() == "head"; }
        }

        protected override void Handle()
        {
            Context.Response.AddHeader("ContentType",File.ContentType);
            Context.Response.AddHeader("ContentLength", File.ContentLength.ToString());
            Context.Response.AddHeader("LastModified", File.LastModified);
            //this.Context.Response.Headers.ETag = new EntityTagHeaderValue("\"a\"");

            Context.Response.StatusCode = (int)StatusCode.OK;
            Context.Response.Status = StatusCode.OK.ToString();
        }
    }
}
