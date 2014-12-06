using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Settings
{
    public class XdavOnProcessingEventArg : XdavEventArg
    {
        public XdavOnProcessingEventArg()
        {
            AllowContinue = true;
        }
        internal bool AllowContinue { get; private set; }
        public void CancelProcess()
        {
            AllowContinue = false;
        }
    }
}
