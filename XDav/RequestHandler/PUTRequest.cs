using Sphorium.WebDAV.Server.Framework.BaseClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XDav.Helper;

namespace XDav.RequestHandler
{
    public class DavPut : DavPutBase
    {
        public DavPut()
        {
            this.ProcessDavRequest += new DavProcessEventHandler(DavPut_ProcessDavRequest);
        }

        private void DavPut_ProcessDavRequest(object sender, EventArgs e)
        {
            FileInfo _fileInfo = FileWrapper.Current.File.FileInfo;
            //Check to see if a lock already exists
            DirectoryInfo _dirInfo = _fileInfo.Directory;
            try
            {
                if (_dirInfo == null)
                {// base.AbortRequest(HttpStatusCodes.ClientErrors.NotFound);
                }
                else
                {
                    if (!base.OverwriteExistingResource)
                    {
                        //Check to see if the resource already exists
                        //if (FileWrapper.Current.File.FileInfo != null)
                        //    base.AbortRequest(DavPutResponseCode.Conflict);
                        //else
                        SaveFile();
                    }
                    else
                        SaveFile();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SaveFile()
        {
            byte[] _requestInput = base.GetRequestInput();
            using (FileStream _newFile = new FileStream(FileWrapper.Current.FullPath, FileMode.OpenOrCreate))
            {
                _newFile.Position = 0;
                _newFile.Write(_requestInput, 0, _requestInput.Length);
                _newFile.Close();
            }

            DeleteLockFile(FileWrapper.Current.File.FileInfo);
        }

        private void DeleteLockFile(FileInfo fileInfo)
        {
            fileInfo.Directory.GetFiles().ToList().ForEach(f => { 
                if(f.Name.StartsWith(fileInfo.Name) && f.Name.EndsWith(".locked"))
                {
                    f.Delete();
                }
            });
        }
    }
}
