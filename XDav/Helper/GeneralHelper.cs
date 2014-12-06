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

        internal static HttpVerb GetVerb(string verb)
        {
            switch (verb.ToUpper())
            {
                case "POST" :
                    return HttpVerb.POST;
                case "GET" :
                    return HttpVerb.GET;
                case "PUT":
                    return HttpVerb.PUT;
                case "DELETE" :
                   return HttpVerb.DELETE;
                case "LOCK":
                    return HttpVerb.LOCK;
                case "UNLOCK":
                    return HttpVerb.UNLOCK;
                case "OPTIONS":
                    return HttpVerb.OPTIONS;
                case "HEAD":
                    return HttpVerb.HEAD;
                default:
                    return HttpVerb.GET;
            }
        }
    }
}
