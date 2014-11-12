using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Helper
{
    public static class GeneralHelper
    {
        /// <summary>
        ///	Retrieve's a file's custom property information file path (*.property)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetCustomPropertyInfoFilePath(string filePath)
        {
            string _propFilePath = null;

            FileInfo _fileInfo = new FileInfo(filePath);
            if (_fileInfo.Exists)
                _propFilePath = _fileInfo.FullName + "._.property";

            return _propFilePath;
        }
    }
}
