using System;
using System.Collections.Generic;
using NailsFramework.Logging;

namespace NailsFramework.Tests.Logging
{
    public class MockLog : ILog
    {
        public MockLog()
        {
            Lines = new List<string>();
        }

        public List<string> Lines { get; private set; }

        #region ILog Members

        public bool IsDebugEnabled { get; set; }

        public bool IsInfoEnabled { get; set; }

        public bool IsWarnEnabled { get; set; }

        public bool IsErrorEnabled { get; set; }

        public bool IsFatalEnabled { get; set; }

        public void Debug(string message)
        {
            Log("debug", message);
        }

        public void Debug(string message, Exception exception)
        {
            Log("debug", message + " - " + exception.Message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Log("debug", string.Format(format, args));
        }

        public void Info(string message)
        {
            Log("Info", message);
        }

        public void Info(string message, Exception exception)
        {
            Log("Info", message + " - " + exception.Message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Log("Info", string.Format(format, args));
        }

        public void Warn(string message)
        {
            Log("Warn", message);
        }

        public void Warn(string message, Exception exception)
        {
            Log("Warn", message + " - " + exception.Message);
        }

        public void WarnFormat(string format, params object[] args)
        {
            Log("Warn", string.Format(format, args));
        }

        public void Error(string message)
        {
            Log("Error", message);
        }

        public void Error(string message, Exception exception)
        {
            Log("Error", message + " - " + exception.Message);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Log("Error", string.Format(format, args));
        }

        public void Fatal(string message)
        {
            Log("Fatal", message);
        }

        public void Fatal(string message, Exception exception)
        {
            Log("Fatal", message + " - " + exception.Message);
        }

        public void FatalFormat(string format, params object[] args)
        {
            Log("Fatal", string.Format(format, args));
        }

        public string LogName { set; get; }

        #endregion

        private void Log(string level, string message)
        {
            Lines.Add(string.Format("[{0}] {1}", level.ToUpper(), message));
        }
    }
}