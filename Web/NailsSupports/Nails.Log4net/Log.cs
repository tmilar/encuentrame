using System;
using log4net;

namespace NailsFramework.Logging
{
    public class Log : ILog
    {
        private log4net.ILog log;

        #region ILog Members

        public string LogName
        {
            set { log = LogManager.GetLogger(value); }
        }

        public void Debug(string message)
        {
            log.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        public void Info(string message)
        {
            log.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            log.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public void Warn(string message)
        {
            log.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        public void Error(string message)
        {
            log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }

        public bool IsDebugEnabled
        {
            get { return log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return log.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return log.IsFatalEnabled; }
        }

        #endregion
    }
}