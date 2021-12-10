using Allure.Commons;
using EnvDTE;
using EnvDTE80;
using Framework.Assemblies;
using Framework.Enums;
using Framework.Handlers;
using Framework.Pages;
using Framework.Services;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Tests
{
    [TestFixture]
    public class BaseTest
    {

        protected BrowserType _type;
        protected AuthenticationType _authType;
        private IWebDriver _driver;
        protected DriverFactory _driverFactoryInstance;
        protected Page _pages;
        protected Services _services;
        public TestLogger _testLoggerInstance;

        public BaseTest(BrowserType type) { BrowserTypeContext = type; }

        public BaseTest() { }

       
        [SetUp]
        public void Setup()
        {
            TestLogger.GetInstance().Info("Test Session started");

            var settings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string baseUrl = settings.GetSection("urlSettings").GetSection("url").Value;

            if (BrowserTypeContext != BrowserType.None)
            {
                // var url = ConfigurationService.Instance.GetUrlSettings();
                TestLogger.GetInstance().SetBrowserType(BrowserTypeContext);
                TestLogger.GetInstance().Info(String.Format("Starting test {0} using browser {1}", CurrentTestContext.Test.MethodName,
                    BrowserTypeContext.ToString()));
                DriverFactoryInstance = DriverFactory.getInstance();
                DriverFactoryInstance.setDriver(BrowserTypeContext);
                DriverFactoryInstance.getDriver();
                DriverFactoryInstance.getDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
                DriverFactoryInstance.getDriver().Manage().Window.Maximize();
                
                //DriverFactoryInstance.getDriver().Url = "http://www.google.com";
                DriverFactoryInstance.getDriver().Url = settings.GetSection("urlSettings").GetSection("url").Value;

                _pages = new Page(DriverFactoryInstance.getDriver());
                _pages.Register();
            }
            _services = new Services();
            _services.Register();
        }

        [TearDown]
        public void TearDown()
        {
            TestLogger.GetInstance().Info("Test session tearing down");

            TestLogger.GetInstance().Info(String.Format("Ending Test {0}", CurrentTestContext.Test.MethodName));
            TestLogger.GetInstance().Info(String.Format("Tearing down for test {0}", CurrentTestContext.Test.MethodName));

            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                TestLogger.GetInstance().Info("attaching screenshot due to failure");
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                var filename = TestContext.CurrentContext.Test.MethodName + "_screenshot_" + DateTime.Now.Ticks + ".png";
                var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\screenshots")) + "\\" + filename;
                screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
                TestContext.AddTestAttachment(path);
                AllureLifecycle.Instance.AddAttachment(filename, "image/png", path);
                if (BrowserTypeContext != BrowserType.None)
                {
                    DriverFactoryInstance.removeDriver();
                }
            }
            else if (BrowserTypeContext != BrowserType.None)
            {
                DriverFactoryInstance.removeDriver();
            }

            TestLogger.GetInstance().RemoveLogger();
        }

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            //Purge logs and reports here using Loghandler and reporthandler
        }


        [OneTimeTearDown]
        public void AfterAllTests()
        {
            TestLogger.GetInstance().Info("Wrapping up test sessions");
            if (BrowserTypeContext != BrowserType.None && BrowserTypeContext != BrowserType.Cloud)
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
            TestLogger.GetInstance().Info("Test sessions completely ended.");

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
        public TestLogger TestLoggerInstance
        {
            get
            {
                return _testLoggerInstance;
            }
            set
            {
                _testLoggerInstance = value;
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
        protected AuthenticationType AuthenticationContext
        {
            get
            {
                return _authType;
            }
            set
            {
                _authType = value;
            }
        }
        protected Page PagesContext
        {
            get
            {

                return _pages;
            }
        
        
        }
        protected Services ServiceContext
        {
            get
            {

                return _services;
            }


        }

    }

}
