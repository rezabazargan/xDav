using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDav.Config;

namespace XDav.Helper
{
    public sealed class XDavFile
    {
        FileInfo _fileInfo;
        public XDavFile(string fileName)
        {
            DirectoryInfo _dirInfo = new DirectoryInfo(ConfigManager.DavPath);
            _fileInfo = _dirInfo.GetFiles().First(f => f.Name == fileName);
        }

        public string ContentType
        {
            get
            {
                string mimeType = "application/unknown";
                string ext = System.IO.Path.GetExtension(_fileInfo.Name).ToLower();
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                if (regKey != null && regKey.GetValue("Content Type") != null)
                    mimeType = regKey.GetValue("Content Type").ToString();
                return mimeType;
            }
        }

        public long ContentLength
        {
            get
            {
                return _fileInfo.Length;
            }
        }

        public string LastModified
        {
            get
            {
                return _fileInfo.LastWriteTime.ToUniversalTime().ToString();
            }
        }
        public FileInfo FileInfo
        {
            get
            {
                return _fileInfo;
            }
        }

    }
}
