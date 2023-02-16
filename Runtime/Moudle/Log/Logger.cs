using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    class Logger : ILogHandler
    {
        private ILogHandler defaultLogHandler;
        private Exception exception;

        public Logger(ILogHandler logHandler, Exception exception)
        {
            this.defaultLogHandler = logHandler;
            this.exception = exception;
        }

        public void LogException(System.Exception exception, UnityEngine.Object context)
        {
            this.exception.SendException(exception);
            defaultLogHandler.LogException(exception, context);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            DateTime dateTime = DateTime.Now;
            string date = string.Format("{0: HH:mm:ss} ", dateTime);
            string message = string.Format(format, args);
            string str = string.Concat(date, message);

            defaultLogHandler.LogFormat(logType, context, "{0}", str);
        }

        public ILogHandler GetDefualtLogHandler()
        {
            return defaultLogHandler;
        }
    }
}
