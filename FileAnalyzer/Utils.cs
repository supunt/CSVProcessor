namespace FileAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Utils
    {
        public static string GetFileNameFromPath(string path)
        {
            string[] pathSplits = path.Split('\\');
            return pathSplits[pathSplits.Length - 1];
        }
    }
}
