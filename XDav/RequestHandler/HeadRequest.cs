using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDav.Config;
using XDav.ResourceManagement;

namespace XDav.RequestHandler
{
    public class HeadRequest : RequestBase
    {
        internal override Func<System.Web.HttpContext, bool> Condition
        {
            get { return r => r.Request.HttpMethod.ToLower() == "head"; }
        }

        protected override void Handle()
        {


            DirectoryInfo _dirInfo = new DirectoryInfo(ConfigManager.DavPath);
            FileInfo _fileInfo = _dirInfo.GetFiles().First(f => f.Name == FileName); 

                if (_fileInfo != null)
                {
                    //TODO: handle versions

                    DavFile _davFile = new DavFile(_fileInfo.Name, Config.ConfigManager.DavPath);
                    _davFile.CreationDate = _fileInfo.CreationTime;
                    _davFile.LastModified = _fileInfo.LastWriteTime.ToUniversalTime();

                    //Check to see if there are any locks on the resource
                    DavLockProperty _lockInfo = RequestWrapper.GetLockInfo(_fileInfo.FullName);
                    if (_lockInfo != null)
                        _davFile.ActiveLocks.Add(_lockInfo);

                    //Check to see if there are any custom properties on the resource
                    DavPropertyCollection _customProperties = RequestWrapper.GetCustomPropertyInfo(_fileInfo.FullName);
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
