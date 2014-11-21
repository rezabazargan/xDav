using Sphorium.WebDAV.Server.Framework.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDav.Helper;

namespace XDav.RequestHandler
{
    public class DavOptions : DavOptionsBase
    {
        public DavOptions()
        {
            this.ProcessDavRequest += new DavProcessEventHandler(DavOptions_ProcessDavRequest);
        }
        private void DavOptions_ProcessDavRequest(object sender, EventArgs e)
        {
            //Provide support for all
            base.SupportedHttpMethods = HttpMethods.All;
        }
    }
}
