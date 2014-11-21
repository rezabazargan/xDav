using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XDav.Example.Controllers
{
    public class FileController : Controller
    {
        public ActionResult FileContent()
        {
            var diInfo = new DirectoryInfo(XDav.Config.ConfigManager.DavPath).GetFiles().Select(f=> f.Name);
            return PartialView(diInfo);
        }
    }
}