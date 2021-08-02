using NLog;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Framework.Handlers
{
    /// <summary>
    /// This is a class using singleton pattern that will act as an external logger using NLog as a Singleton
    /// </summary>
    public class TestLogger : ITestLogger
    {
        private static TestLogger _instance;
        private static Logger _logger;

        private TestLogger() { }

        public static TestLogger GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TestLogger();

            }
            return _instance;
        }

        private Logger GetLogger(String theLogger)
        {
            if (TestLogger._logger == null)
            {
                TestLogger._logger = LogManager.GetLogger(theLogger);

            }
            return TestLogger._logger;
        }

        public void Debug(String message, String arg = null)
        {
            if(arg == null)
                GetLogger("logRules").Debug(message);
            else
                GetLogger("logRules").Debug(message,arg);
        }

        public void Error(String message, String arg = null)
        {
            if (arg == null)
                GetLogger("logRules").Error(message);
            else
                GetLogger("logRules").Error(message, arg);
        }

        public void Info(String message, String arg = null)
        {
            if (arg == null)
                GetLogger("logRules").Info(message);
            else
                GetLogger("logRules").Info(message, arg);
        }
    }
}
