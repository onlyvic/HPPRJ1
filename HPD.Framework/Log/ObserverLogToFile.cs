using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HPD.Framework.Log
{
    /// <summary>
    /// Class that writes log events to a local file.
    /// </summary>
    /// <remarks>
    /// GoF Design Pattern: Observer.
    /// The Observer Design Pattern allows this class to attach itself to an
    /// the logger and 'listen' to certain events and be notified of the event. 
    /// </remarks>
    public class ObserverLogToFile : ILog
    {
        private string _fileName;

        /// <summary>
        /// Constructor of ObserverLogToFile.
        /// </summary>
        /// <param name="fileName">File log to.</param>
        public ObserverLogToFile(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Write a log request to a file.
        /// </summary>
        /// <param name="sender">Sender of the log request.</param>
        /// <param name="e">Parameters of the log request.</param>
        public void Log(object sender, LogEventArgs e)
        {
            string message = "[" + e.Date.ToString() + "] " +
                e.SeverityString + ": " + e.Message;
            
            FileStream fileStream;

            // Create directory, if necessary
            try
            {
                fileStream = new FileStream(_fileName, FileMode.Append);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory((new FileInfo(_fileName)).DirectoryName);
                fileStream = new FileStream(_fileName, FileMode.Append);
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(10);
                fileStream = new FileStream(_fileName, FileMode.Append);
            }

            // NOTE: Be sure you have write privileges to folder
            StreamWriter writer = new StreamWriter(fileStream);
            try
            {
                writer.WriteLine(message);
            }
            catch { /* do nothing for now */}
            finally
            {
                try
                {
                    writer.Close();
                }
                catch { /* do nothing for now */}
            }
        }
    }
}
