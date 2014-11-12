using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Helper
{
    public static class Extensions
    {
        public static string GetValue(this NameValueCollection header, string name)
        {
           return header.GetValues(name).Any() ? header.GetValues(name).First() : string.Empty;
        }
    }
}
