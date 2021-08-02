using Framework.Enums;
using Framework.Factory;
using Framework.Handlers;
using Framework.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class BaseTest
    {

        protected BrowserType _type;
        private IWebDriver _driver;
        protected DriverFactory _driverFactoryInstance;
        protected Page _pages;

        public BaseTest(BrowserType type) { BrowserTypeContext = type; }

        public BaseTest() { }
       

        [SetUp]
        public void Setup()
        {
            TestLogger.GetInstance().Info(String.Format("Starting test {0} using browser {1}", CurrentTestContext.Test.MethodName, 
                BrowserTypeContext.ToString()));
            DriverFactoryInstance = DriverFactory.getInstance();
            DriverFactoryInstance.setDriver(BrowserTypeContext);
            DriverFactoryInstance.getDriver();
            DriverFactoryInstance.getDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            DriverFactoryInstance.getDriver().Manage().Window.Maximize();
            DriverFactoryInstance.getDriver().Url = "http://www.google.com";
            _pages = new Page(DriverFactoryInstance.getDriver());
            _pages.Register();
        }

        [TearDown]
        public void TearDown()
        {
            TestLogger.GetInstance().Info(String.Format("Ending Test {0}", CurrentTestContext.Test.MethodName));
            TestLogger.GetInstance().Info(String.Format("Tearing down for test {0}", CurrentTestContext.Test.MethodName));
            DriverFactoryInstance.removeDriver();
        }

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            //Purge logs and reports here using Loghandler and reporthandler
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            TestLogger.GetInstance().Info(String.Format("Killing executable for browser {0}", BrowserTypeContext.ToString()));
            //kill all browser executables if any
            System.Diagnostics.ProcessStartInfo p;

            switch (BrowserTypeContext)
            {
                case BrowserType.Chrome:
                    p = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C " + "taskkill /f /im chromedriver.exe");
                    break;
                case BrowserType.FireFox:
                    p = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C " + "taskkill /f /im geckodriver.exe");
                    break;
                default:
                    p = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C " + "taskkill /f /im internetexplorerdriver.exe");
                    break;
            }
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = p;
            proc.Start();
            proc.WaitForExit();
            proc.Close();

        }

        protected IWebDriver Driver
        {
            get
            {
                return _driver;
            }
            set
            {
                _driver = value;
            }

        }
        protected DriverFactory DriverFactoryInstance
        {
            get
            {
                return _driverFactoryInstance;
            }
            set
            {
                _driverFactoryInstance = value;
            }
        }

        protected TestContext CurrentTestContext
        {
            get
            {
                return TestContext.CurrentContext;
            }
        }
        protected BrowserType BrowserTypeContext
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        protected Page PagesContext
        {
            get
            {

                return _pages;
            }
        
        
        }



    }
}
