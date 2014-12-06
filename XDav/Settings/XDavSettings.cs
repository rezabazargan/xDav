using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Settings
{
    public class XDavSettings
    {
        public static void Events(Action<XdavEventFactory> e)
        {
            e(new XdavEventFactory());
        }
    }
}
