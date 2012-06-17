using System;
using System.Collections.Generic;
using System.Text;

namespace HPD.Framework.Log
{
    /// <summary>
    /// Class that writes log events to a database.
    /// </summary>
    /// <remarks>
    /// GoF Design Pattern: Observer.
    /// The Observer Design Pattern allows this class to attach itself to an
    /// the logger and 'listen' to certain events and be notified of the event. 
    /// </remarks>
    public class ObserverLogToDatabase : ILog
    {
        /// <summary>
        /// Writes a log request to the database.
        /// </summary>
        /// <remarks>
        /// Actual database insert statements are commented out.
        /// You can activate this if you have the proper database 
        /// configuration and access privileges.
        /// </remarks>
        /// <param name="sender">Sender of the log request.</param>
        /// <param name="e">Parameters of the log request.</param>
        public void Log(object sender, LogEventArgs e)
        {
            // Skeleton code of how you could log to database
            // Note: A better approach is to use Data Access Object
            string message = "Date = " + e.Date.ToString() +
                " Severity = " + e.SeverityString +
                " Message = " + e.Message;

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO LOG (message) ");
            sql.Append("VALUES('" + message + "')");

            // Commented out for now. You need database to store log values. 
            //Db.Update(sql.ToString());
        }
    }
}
