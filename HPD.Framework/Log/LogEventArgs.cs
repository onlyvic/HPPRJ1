using System;
using System.Collections.Generic;
using System.Text;

namespace HPD.Framework.Log
{
    /// <summary>
    /// Contains log specific event data for log request events.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        private LogSeverity _severity;
        private string _message;
        private Exception _exception;
        private DateTime _date;

        /// <summary>
        /// Constructor of LogEventArgs.
        /// </summary>
        /// <param name="severity">Log severity.</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Inner exception.</param>
        /// <param name="date">Log date.</param>
        public LogEventArgs(LogSeverity severity, string message,
                             Exception exception, DateTime date)
        {
            this._severity = severity;
            this._message = message;
            this._exception = exception;
            this._date = date;
        }

        /// <summary>
        /// Gets and sets the log severity.
        /// </summary>        
        public LogSeverity Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        /// <summary>
        /// Gets and sets the log message.
        /// </summary>        
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// Gets and sets the optional inner exception.
        /// </summary>        
        public Exception Exception
        {
            get { return _exception; }
            set { _exception = value; }
        }

        /// <summary>
        /// Gets and sets the log date and time.
        /// </summary>        
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// Friendly string that represents the severity.
        /// </summary>
        public String SeverityString
        {
            get { return Severity.ToString("G"); }
        }

        /// <summary>
        /// LogEventArgs as a string represenation.
        /// </summary>
        /// <returns>String representation of the LogEventArgs.</returns>
        public override String ToString()
        {
            return "" + Date
                + " - " + SeverityString
                + " - " + Message
                + " - " + Exception.ToString();
        }
    }
}
