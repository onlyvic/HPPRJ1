using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
namespace PMSA.Framework.Utils
{
    public class CFile
    {
        public static string Read(string filePath)
        {
            string sContent = "";
            if (File.Exists(filePath))
            {
                StreamReader oStream = File.OpenText(filePath);
                sContent = oStream.ReadToEnd();
                oStream.Close();
                oStream.Dispose();
            }
            return sContent;
        }
        public static bool Write(string filePath, string content)
        {
            Boolean ret = true;
            try
            {
                StreamWriter oStream = File.CreateText(filePath);
                oStream.Write(content);
                oStream.Close();
                oStream.Dispose();
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
    }
}