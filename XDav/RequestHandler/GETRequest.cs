using Sphorium.WebDAV.Server.Framework.BaseClasses;
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
    public class DavGet : DavGetBase
    {
        public DavGet()
        {
            this.ProcessDavRequest += new DavProcessEventHandler(DavGet_ProcessDavRequest);
        }

        private void DavGet_ProcessDavRequest(object sender, EventArgs e)
        {
            FileInfo _fileInfo = FileWrapper.Current.File.FileInfo;

            using (var s = new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {

                var ms = new MemoryStream();
                s.CopyTo(ms);
                s.Position = 0;
                this.ResponseOutput = ms.ToArray();
                //s.CopyTo(FileWrapper.Current.Context.Response.OutputStream);
            }
        }
    }
}
