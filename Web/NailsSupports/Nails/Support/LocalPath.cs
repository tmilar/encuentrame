using System;
using System.IO;

namespace NailsFramework.Support
{
    public static class LocalPath
    {
        public static string From(string path)
        {
            return Path.IsPathRooted(path)
                       ? path
                       : Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                      path.Replace("/", "\\").TrimStart('~', '\\'));
        }
    }
}