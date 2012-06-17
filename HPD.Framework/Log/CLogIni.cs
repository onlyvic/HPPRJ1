using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace HPD.Framework.Log
{
    public class CLogIni
    {
        #region Initialize Ini Log

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        /// Write Data to the INI File
        /// </summary>       
        private static void IniWriteValue(string Section, string Key, string Value, string FileName)
        {
            if (!IsLog)
                return;
            WritePrivateProfileString(Section, Key, Value, FileName);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        private static string IniReadValue(string Section, string Key, string FileName)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, FileName);
            return temp.ToString();
        }
        #endregion

        #region Write file
        private static bool IsLog = true;
        private static string UrlLog = "";

        public static string PathLog
        {
            get
            {
                string today = DateTime.Now.ToString("yyyyMMdd");
                //System.AppDomain.CurrentDomain.BaseDirectory + "/Logs"
                string logDir = System.Configuration.ConfigurationManager.AppSettings["Framework.Log.LogDirectory"];
                if (logDir != null)
                {
                    UrlLog = logDir;
                }
                else
                {
                    UrlLog = System.AppDomain.CurrentDomain.BaseDirectory + "/Logs";
                }
                return UrlLog + "\\" + today + "\\";
            }
        }

        public static void Write(string session, string value, string fileName)
        {
            try
            {
                DirectoryInfo dir = Directory.GetParent(PathLog);
                if (!dir.Exists)
                    Directory.CreateDirectory(dir.FullName);
                string file = PathLog + DateTime.Now.ToString("HH") + "-" + fileName + ".log";
                IniWriteValue(session, DateTime.Now.ToString("yyyyMMdd HH:mm:ss fffff"), value, file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Write(string session, string key, string value, string fileName)
        {
            try
            {
                DirectoryInfo dir = Directory.GetParent(PathLog);
                if (!dir.Exists)
                    Directory.CreateDirectory(dir.FullName);
                string file = PathLog + DateTime.Now.ToString("HH") + "-" + fileName + ".log";
                IniWriteValue(session, key, value, file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Read(string session, string key, string filename)
        {
            string result;
            try
            {
                string file = PathLog + filename + ".log";
                result = IniReadValue(session, key, file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static string ReadFile(string session, string key, string pathname)
        {
            string result;
            try
            {
                result = IniReadValue(session, key, pathname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion
    }
}
