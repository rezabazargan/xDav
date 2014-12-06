using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XDav.Settings;

namespace XDav.Example.App_Start
{
    public class XdavConfig
    {
        public static void Register()
        {
            XDavSettings.Events(e => {
                e.OnProcessing(evt =>
                {
                    // Do this if you want cancel xdav process
                    //evt.CancelProcess();
                })
                .OnProcessed(evt =>
                {

                })
                .OnException(ex => { 

                });
            });
        }
    }
}