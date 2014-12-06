using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XDav.Helper;

namespace XDav.Settings
{
    public class XdavEventArg
    {
        public HttpVerb HttpVerp        { get; set; }
        public XDavFile File            { get; set; }
        public HttpApplication Context  { get; set; }
    }
}
