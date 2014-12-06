using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Settings
{
    public class XdavOnExceptionEventArg : XdavEventArg
    {
        public Exception Exception { get; internal set; }
    }
}
