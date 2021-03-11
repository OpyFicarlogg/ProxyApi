using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyApi.Extension
{
    public static class ExtensionString
    {
        public static string RemoveValue(this string value, string toRemove){
            int index = value.IndexOf(toRemove);
            return index != -1? value.Remove(index,toRemove.Length) : value;
        }
    }
}
