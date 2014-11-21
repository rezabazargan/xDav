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
    public class DavUnlock : DavUnlockBase
    {
        public DavUnlock()
        {
            this.ProcessDavRequest += new DavProcessEventHandler(DavUnlock_ProcessDavRequest);
        }

        private void DavUnlock_ProcessDavRequest(object sender, EventArgs e)
        {
            FileInfo _fileInfo = FileWrapper.Current.File.FileInfo;
            //Check to see if a lock already exists
            DirectoryInfo _dirInfo = _fileInfo.Directory;

            FileInfo[] _lockedFiles = _dirInfo.GetFiles("*." + base.LockToken + ".locked");
            if (_lockedFiles.Length == 0)
                base.AbortRequest(DavUnlockResponseCode.PreconditionFailed);
            else
            {
                //Delete the locked files
                _lockedFiles[0].Delete();

                //Original file path 
                string _sandBoxFileName = _lockedFiles[0].Name.Replace("." + base.LockToken + ".locked", "");
                string[] _pathInfo = _lockedFiles[0].Name.Split('_');

                //Ignore the last two
                StringBuilder _sourceFileName = new StringBuilder();
                for (int i = 0; i < _pathInfo.Length - 2; i++)
                {
                    if (_sourceFileName.Length != 0)
                        _sourceFileName.Append("_");

                    _sourceFileName.Append(_pathInfo[i]);
                }

                //Now append the extension
                string[] _extInfo = _pathInfo[_pathInfo.Length - 1].Split('.');
                _sourceFileName.Append("." + _extInfo[1]);

                //Attempt to merge this file back to the original location

                if (File.Exists(Path.Combine(FileWrapper.Current.FullPath, _sourceFileName.ToString())))
                {
                    string _tempFilePath = Path.Combine(FileWrapper.Current.FullPath, "temp", _sandBoxFileName);

                    //Check and clear out the property file
                    string _propertyFile = FileWrapper.Current.GetCustomPropertyInfoFilePath(_tempFilePath);
                    if (File.Exists(_propertyFile))
                        File.Delete(_propertyFile);

                    File.Copy(_tempFilePath, Path.Combine(FileWrapper.Current.FullPath, _sourceFileName.ToString()), true);
                    File.Delete(_tempFilePath);
                }
            }
        }
    }



}
