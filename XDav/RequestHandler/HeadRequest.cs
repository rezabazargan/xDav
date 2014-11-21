using Sphorium.WebDAV.Server.Framework.BaseClasses;
using Sphorium.WebDAV.Server.Framework.Classes;
using Sphorium.WebDAV.Server.Framework.Collections;
using Sphorium.WebDAV.Server.Framework.Resources;
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
    public class DavHead : DavHeadBase
    {
        public DavHead()
        {
            this.ProcessDavRequest += DavHead_ProcessDavRequest;
        }

        private void DavHead_ProcessDavRequest(object sender, EventArgs e)
        {

            FileInfo _fileInfo = FileWrapper.Current.File.FileInfo;

            if (_fileInfo != null)
            {
                //TODO: handle versions

                DavFile _davFile = new DavFile(_fileInfo.Name, FileWrapper.Current.FullPath);
                _davFile.CreationDate = _fileInfo.CreationTime;
                _davFile.LastModified = _fileInfo.LastWriteTime.ToUniversalTime();

                //Check to see if there are any locks on the resource
                DavLockProperty _lockInfo = FileWrapper.Current.GetLockInfo(_fileInfo.FullName);
                if (_lockInfo != null)
                    _davFile.ActiveLocks.Add(_lockInfo);

                //Check to see if there are any custom properties on the resource
                DavPropertyCollection _customProperties = FileWrapper.Current.GetCustomPropertyInfo(_fileInfo.FullName);
                if (_customProperties != null)
                    _davFile.CustomProperties.Copy(_customProperties);

                _davFile.SupportsExclusiveLock = true;
                _davFile.SupportsSharedLock = true;
                _davFile.ContentLength = (int)_fileInfo.Length;

                //Set the resource
                base.Resource = _davFile;
            }

        }
    }
}
