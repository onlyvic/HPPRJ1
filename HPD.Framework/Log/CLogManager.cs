using System;
using System.Collections.Generic;
using System.Text;

namespace HPD.Framework.Log
{
    public class CLogManager
    {
        public static void Init(string logPath)
        {
            TLog.LogByHour = true;
            TLog.LogDir = AppDomain.CurrentDomain.BaseDirectory + "/Logs";

            string logDir = System.Configuration.ConfigurationManager.AppSettings["Framework.Log.LogDirectory"];
            string LogByHour = System.Configuration.ConfigurationManager.AppSettings["Framework.Log.LogByHour"];
            string isLog = System.Configuration.ConfigurationManager.AppSettings["Framework.Log.IsLog"];

            if (LogByHour != null)
            {
                TLog.LogByHour = LogByHour == "1" ? true : false;
            }
            if (isLog != null)
            {
                TLog.IsLog = isLog == "1" ? true : false;
            }
            if (logPath != null && logPath != "")
            {
                TLog.LogDir = logPath;
            }
            else if (logDir != null)
            {
                TLog.LogDir = logDir;
            }
        }
        public static void Init()
        {
            string urlPhysical = AppDomain.CurrentDomain.BaseDirectory + "/Logs";
            Init(urlPhysical);
        }
        public CLogManager(string virtualURL)
        {
            Init();
        }
        public CLogManager()
        {
            Init();
        }

        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="logTitle"></param>
        /// <param name="logDetail"></param>
        /// <param name="group"></param>
        public static void Write(string logTitle, string logDetail, string group)
        {
            TLog.Write(logTitle, logDetail, group);
        }

        /// <summary>
        /// Write log - Data Access Layer
        /// </summary>
        /// <param name="logTitle"></param>
        /// <param name="logDetail"></param>
        public static void WriteDAL(string logTitle, string logDetail)
        {
            TLog.Write(logTitle, logDetail, "DAL");
        }

        /// <summary>
        /// Write log - Business Access Layer
        /// </summary>
        /// <param name="logTitle"></param>
        /// <param name="logDetail"></param>
        public static void WriteBAL(string logTitle, string logDetail)
        {
            TLog.Write(logTitle, logDetail, "BAL");
        }

        /// <summary>
        /// Write log - Presentation Layer
        /// </summary>
        /// <param name="logTitle"></param>
        /// <param name="logDetail"></param>
        public static void WritePL(string logTitle, string logDetail)
        {
            TLog.Write(logTitle, logDetail, "PL");
        }

        /// <summary>
        /// Write log - Framework
        /// </summary>
        /// <param name="logTitle"></param>
        /// <param name="logDetail"></param>
        public static void WriteFW(string logTitle, string logDetail)
        {
            TLog.Write(logTitle, logDetail, "FW");
        }

        /// <summary>
        /// Write log - Service Layer
        /// </summary>
        /// <param name="logTitle"></param>
        /// <param name="logDetail"></param>
        public static void WriteSL(string logTitle, string logDetail)
        {
            TLog.Write(logTitle, logDetail, "SL");
        }
        /// <summary>
        /// Write log - JavaScript
        /// </summary>
        /// <param name="logTitle"></param>
        /// <param name="logDetail"></param>
        public static void WriteJS(string logTitle, string logDetail)
        {
            TLog.Write(logTitle, logDetail, "JS");
        }
    }

    public class LogManagerSingleton
    {
        public static bool IsLogError = true;
        public static bool IsLogBug = true;
        public static bool IsLogInfo = true;

        static string LogPath;
        public static void InitLog()
        {
            ILog log = new ObserverLogToFile(@"DoFactory.log");
            SingletonLogger.Instance.Attach(log);
        }
        public static void InitLog(string logPath)
        {
            LogPath = logPath;
            ILog log = new ObserverLogToFile(LogPath + @"\MarketService.log");
            SingletonLogger.Instance.Attach(log);
        }
        public static void WriteError(string intro, string detail)
        {
            if (IsLogError)
                SingletonLogger.Instance.Error(intro + " - " + detail);
        }

        public static void WriteBug(string intro, string detail)
        {
            if (IsLogBug)
                SingletonLogger.Instance.Error(intro + " - " + detail);
        }
        public static void WriteBug(string detail)
        {
            if (IsLogBug)
                SingletonLogger.Instance.Error(detail);
        }

        public static void WriteInfo(string intro, string detail)
        {
            if (IsLogInfo)
                SingletonLogger.Instance.Error(intro + " - " + detail);
        }
        public static void WriteInfo(string detail)
        {
            if (IsLogInfo)
                SingletonLogger.Instance.Error(detail);
        }
    }
}
