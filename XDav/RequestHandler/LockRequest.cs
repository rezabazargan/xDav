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
    public class LockRequest : RequestBase
    {
        internal override Func<System.Web.HttpContext, bool> Condition
        {
            get { return r => r.Request.HttpMethod.ToLower() == "lock"; }
        }

        protected override void Handle()
        {
            DirectoryInfo _dirInfo = new DirectoryInfo(ConfigManager.DavPath);
            FileInfo _fileInfo = _dirInfo.GetFiles().First(f => f.Name == FileName);
            UnicodeEncoding uniEncoding = new UnicodeEncoding();
            int recordNumber = 13;
            int byteCount = uniEncoding.GetByteCount(recordNumber.ToString());
            using (FileStream fileStream = new FileStream( FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                byte[] readText = new byte[fileStream.Length];
                try
                {
                    fileStream.Lock(0, fileStream.Length);
                }
                catch (IOException e)
                {
                   //TODO ExceptionHandle
                }
            }
            Context.SetStatus(StatusCode.OK);
        }
    }
}
