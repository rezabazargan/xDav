using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Settings
{
    public class XdavOnProcessedEventArg : XdavEventArg
    {
        public int StatusCode { get; internal set; }
    }
}
