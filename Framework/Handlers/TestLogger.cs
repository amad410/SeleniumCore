using Framework.Enums;
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
        private BrowserType _browserType;


        private TestLogger() { }

        public static TestLogger GetInstance()
        {

            if (_instance == null)
            {
                _instance = new TestLogger();

            }
            return _instance;
        }

        public void SetBrowserType(BrowserType browserType)
        {
            BrowserType = browserType;
        }
        public void RemoveLogger() // Quits the driver and closes the browser
        {
            TestLogger._logger = null;

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
            if (arg == null)
                GetLogger("logRules").Debug(message);
            else
                GetLogger("logRules").Debug(message, arg);
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
            if(BrowserType == BrowserType.Chrome)
            {
                if (arg == null)
                    GetLogger("chromeRules").Info(message);
                else
                    GetLogger("chromeRules").Info(message, arg);

            }
            else if (BrowserType == BrowserType.Edge)
            {
                if (arg == null)
                    GetLogger("edgeRules").Info(message);
                else
                    GetLogger("edgeRules").Info(message, arg);

            }
            else if (BrowserType == BrowserType.FireFox)
            {
                if (arg == null)
                    GetLogger("fireFoxRules").Info(message);
                else
                    GetLogger("fireFoxRules").Info(message, arg);

            }
            else if (BrowserType == BrowserType.InternetExplorer)
            {
                if (arg == null)
                    GetLogger("IERules").Info(message);
                else
                    GetLogger("IERules").Info(message, arg);

            }
            else
            {
                if (arg == null)
                    GetLogger("logRules").Info(message);
                else
                    GetLogger("logRules").Info(message, arg);

            }

        }
        public BrowserType BrowserType
        {
            get
            {
                return _browserType;
            }
            set
            {
                _browserType = value;
            }
        }
    }
            
}
