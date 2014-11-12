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
    public class UnlockRequest : RequestBase
    {
        internal override Func<System.Web.HttpContext, bool> Condition
        {
            get { return r => r.Request.HttpMethod.ToLower() == "unlock"; }
        }

        protected override void Handle()
        {
            DirectoryInfo _dirInfo = new DirectoryInfo(ConfigManager.DavPath);
            FileInfo[] _lockedFiles = _dirInfo.GetFiles("*." + GetUnlockToken() + ".locked");


            if (_lockedFiles.Length == 0)
                throw new Exception("FileNot Found"); // TODO : Exception Handle
            else
            {
                //Delete the locked files
                _lockedFiles[0].Delete();

                //Original file path 
                string _sandBoxFileName = _lockedFiles[0].Name.Replace("." + GetUnlockToken() + ".locked", "");
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
                if (File.Exists(Path.Combine(ConfigManager.DavPath, _sourceFileName.ToString())))
                {
                    string _tempFilePath = Path.Combine(ConfigManager.DavPath, "temp", _sandBoxFileName);

                    //Check and clear out the property file
                    string _propertyFile = GeneralHelper.GetCustomPropertyInfoFilePath(_tempFilePath);
                    if (File.Exists(_propertyFile))
                        File.Delete(_propertyFile);

                    File.Copy(_tempFilePath, Path.Combine(ConfigManager.DavPath, _sourceFileName.ToString()), true);
                    File.Delete(_tempFilePath);
                }
            }
        }

        private string GetUnlockToken()
        {
            var inputString = Context.Request.Headers.GetValue("Lock-Token");
            string _opaqueLockToken = "";

            if (inputString != null)
            {
                string _prefixTag = "<opaquelocktoken:";
                int _prefixIndex = inputString.IndexOf(_prefixTag);
                if (_prefixIndex != -1)
                {
                    int _endIndex = inputString.IndexOf('>', _prefixIndex);
                    if (_endIndex > _prefixIndex)
                        _opaqueLockToken = inputString.Substring(_prefixIndex + _prefixTag.Length, _endIndex - (_prefixIndex + _prefixTag.Length));
                }
            }

            return _opaqueLockToken;
        }
    }



}
