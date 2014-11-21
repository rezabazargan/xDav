using Sphorium.WebDAV.Server.Framework.BaseClasses;
using Sphorium.WebDAV.Server.Framework.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using XDav.Config;
using XDav.Helper;

namespace XDav.RequestHandler
{
    public class DavLock : DavLockBase
    {
        public DavLock()
        {
            this.ProcessDavRequest += new DavProcessEventHandler(DavLock_ProcessDavRequest);
            this.RefreshLockDavRequest += new DavRefreshLockEventHandler(DavLock_RefreshLockDavRequest);
        }

        private void DavLock_ProcessDavRequest(object sender, EventArgs e)
        {
            //TODO: allow collection locking



            //Check to see if the resource exists
            FileInfo _fileInfo = FileWrapper.Current.File.FileInfo;
            if (_fileInfo == null)
            {// base.AbortRequest(HttpStatusCodes.Successful.OK);
            }
            else
            {
                //Check to see if a lock already exists
                if (FileWrapper.Current.GetLockInfo(_fileInfo.FullName) != null)
                    base.AbortRequest(DavLockResponseCode.Locked);
                else
                {
                    string _opaqueLock = System.Guid.NewGuid().ToString("D");

                    //Apply the requested lock information
                    base.ResponseLock.AddLockToken(_opaqueLock);

                    //Create the *.locked file
                    string _lockedFilePath = _fileInfo.DirectoryName + @"\" + _fileInfo.Name + "."
                        + _opaqueLock + ".locked";

                    //Serialize the lock information
                    FileInfo _lockFile = new FileInfo(_lockedFilePath);
                    using (Stream _lockFileStream = _lockFile.Open(FileMode.Create))
                    {
                        BinaryFormatter _binaryFormatter = new BinaryFormatter();
                        _binaryFormatter.Serialize(_lockFileStream, base.ResponseLock);
                        _lockFileStream.Close();
                    }
                }
            }
        }

        private void DavLock_RefreshLockDavRequest(object sender, DavRefreshEventArgs e)
        {
            //Check to see if the lock exists
            DirectoryInfo _dirInfo = FileWrapper.Current.File.FileInfo.Directory;

            FileInfo[] _lockFiles = _dirInfo.GetFiles("*." + e.LockToken + ".locked");
            if (_lockFiles.Length == 0)
                base.AbortRequest(DavLockResponseCode.PreconditionFailed);
            else
            {
                //Deserialize the lock information
                base.ResponseLock = FileWrapper.Current.GetLockInfo(_lockFiles[0]);

                //Set the requested lockTimeout or overwrite
                base.ResponseLock.LockTimeout = 10;
            }
        }

    }
}
