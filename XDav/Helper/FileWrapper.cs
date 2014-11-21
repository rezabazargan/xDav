using Sphorium.WebDAV.Server.Framework.Classes;
using Sphorium.WebDAV.Server.Framework.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XDav.Helper
{
    public class FileWrapper
    {
        public static  FileWrapper Current
        {
            get
            {
                return CallContext.GetData("xDavFileWrapper") as FileWrapper;
            }
        }

        internal static void Create(HttpContext context)
        {
            CallContext.SetData("xDavFileWrapper", new FileWrapper(context));
        }

        private HttpContext _context;
        public FileWrapper(HttpContext context)
        {
            _context = context;
        }



        public string FullPath
        {
            get
            {
                return XDav.Config.ConfigManager.DavPath + "\\" + FileName;
            }
        }

        public string FileName
        {
            get
            {
                return _context.Request.Url.LocalPath.Replace(string.Format("/{0}/", XDav.Config.ConfigManager.XDavConfig.Name), "");
            }
        }

        public XDavFile File
        {
            get
            {
                return new XDavFile(FileName);
            }
        }

        #region [lock Info]
        public string GetLockInfoFilePath(string filePath)
        {
            string _lockFilePath = null;

            FileInfo _fileInfo = new FileInfo(filePath);
            if (_fileInfo.Exists)
                _lockFilePath = _fileInfo.FullName + "._.locked";

            return _lockFilePath;
        }
        private string GetLockInfoFilePath(FileInfo _fileInfo)
        {

            //lets see if file we asking about already exist?
            if (_fileInfo.Exists && _fileInfo.Extension.Equals(".locked"))
                return _fileInfo.FullName;

            string _lockFilePath = string.Empty;
            _lockFilePath = _fileInfo.FullName + "._.locked";

            FileInfo lockFile = new FileInfo(_lockFilePath);

            if (lockFile.Exists)
                return _lockFilePath;
            else
                return null;
        }

        /// <summary>
        /// Retrieve's a file's lock information or loads a locked file (*.locked)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public  DavLockProperty GetLockInfo(FileInfo _fileInfo)
        {
            DavLockProperty _lockProperty = null;
            //Try to deserialize the lock file
            string _lockFilePath = GetLockInfoFilePath(_fileInfo);
            if (_lockFilePath != null)
            {
                try
                {
                    FileInfo _lockFile = new FileInfo(_lockFilePath);
                    using (Stream _lockFileStream = _lockFile.Open(FileMode.OpenOrCreate))
                    {
                        BinaryFormatter _binaryFormatter = new BinaryFormatter();
                        _lockProperty = (DavLockProperty)_binaryFormatter.Deserialize(_lockFileStream);
                        _lockFileStream.Close();
                    }
                }
                catch (Exception)
                {
                    //Incase the deserialization fails
                }
            }

            return _lockProperty;
        }

        #endregion
        public DavLockProperty GetLockInfo(string filePath)
        {
            DavLockProperty _lockProperty = null;

            //Try to deserialize the lock file
            string _lockFilePath = GetLockInfoFilePath(filePath);
            if (_lockFilePath != null)
            {
                try
                {
                    FileInfo _lockFile = new FileInfo(_lockFilePath);
                    using (Stream _lockFileStream = _lockFile.Open(FileMode.Open))
                    {
                        BinaryFormatter _binaryFormatter = new BinaryFormatter();
                        _lockProperty = (DavLockProperty)_binaryFormatter.Deserialize(_lockFileStream);
                        _lockFileStream.Close();
                    }
                }
                catch (Exception)
                {
                    //Incase the deserialization fails
                }
            }

            return _lockProperty;
        }


        public string GetCustomPropertyInfoFilePath(string filePath)
        {
            string _propFilePath = null;

            FileInfo _fileInfo = new FileInfo(filePath);
            if (_fileInfo.Exists)
                _propFilePath = _fileInfo.FullName + "._.property";

            return _propFilePath;
        }
        public DavPropertyCollection GetCustomPropertyInfo(string filePath)
        {
            DavPropertyCollection _customProperties = null;

            //Try to deserialize the property file
            string _propFilePath = GetCustomPropertyInfoFilePath(filePath);
            if (_propFilePath != null)
            {
                try
                {
                    FileInfo _propFile = new FileInfo(_propFilePath);
                    if (_propFile.Exists)
                    {
                        using (Stream _propFileStream = _propFile.Open(FileMode.Open))
                        {
                            BinaryFormatter _binaryFormatter = new BinaryFormatter();
                            _customProperties = (DavPropertyCollection)_binaryFormatter.Deserialize(_propFileStream);
                            _propFileStream.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    //Incase the deserialization fails
                }
            }

            return _customProperties;
        }


        public HttpContext Context
        {
            get
            {
                return _context;
            }
        }

        public string ParseVersionFileName(FileInfo versionFileInfo)
        {
            string _versionFileName = versionFileInfo.Name;

            string _delimiter = "._.";
            int _prefixIndex = _versionFileName.IndexOf(_delimiter);
            if (_prefixIndex != -1)
                _versionFileName = _versionFileName.Substring(0, _prefixIndex);

            return _versionFileName;
        }

    }
}
