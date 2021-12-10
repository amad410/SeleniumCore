using Framework.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Framework.Assemblies
{
    public class DriverFactory
    {
        private BrowserType _browserType;
        private DriverFactory()
        {
            //Do-nothing..Do not allow to initialize this class from outside
        }
        private static DriverFactory instance = new DriverFactory();
        protected static ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();

        public static DriverFactory getInstance()
        {
            return instance;
        }

      
        public static ThreadLocal<IWebDriver> IE = new ThreadLocal<IWebDriver>(() =>
        {
            return new InternetExplorerDriver();
        });

        public static ThreadLocal<IWebDriver> Chrome = new ThreadLocal<IWebDriver>(() =>
        {
            return new ChromeDriver();
        });

        public static ThreadLocal<IWebDriver> FireFox = new ThreadLocal<IWebDriver>(() =>
        {
            return new FirefoxDriver();
        });

        public static ThreadLocal<IWebDriver> Edge = new ThreadLocal<IWebDriver>(() =>
        {

            string startupPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).
            Parent.Parent.FullName + @"\..\Framework\ExternalDrivers\";
             var service = EdgeDriverService.CreateDefaultService(startupPath, "msedgedriver.exe");
            service.UseVerboseLogging = true;
            service.UseSpecCompliantProtocol = true;
            service.Start();
            var options = new EdgeOptions();
            
            return new RemoteWebDriver(service.ServiceUrl,options);
        });

        public void setDriver(BrowserType type)
        {
            TypeBrowser = type;
            switch (type)
            {
                case BrowserType.Chrome:
                    driver.Value = Chrome.Value;
                    break;
                case BrowserType.InternetExplorer:
                    driver.Value = IE.Value;
                    break;
                case BrowserType.FireFox:
                    driver.Value = FireFox.Value;
                    break;
                default:
                    driver.Value = Edge.Value;
                    break;
            }

        }
        public IWebDriver getDriver() // call this method to get the driver object and launch the browser
        {
            return driver.Value;
        }

        public void removeDriver() // Quits the driver and closes the browser
        {
            getDriver().Close();
           
        }

        private BrowserType TypeBrowser
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
