using Framework.Handlers;
using Framework.Pages.HomePage;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Assemblies
{
    /// <summary>
    /// This wrapper class is to serve in support of creating a Domain Specific Language (DSL) to access
    /// additional page objects. All page objects will be registered and accessed via their page class properties
    /// here.
    /// </summary>
    public class Page
    {
        GoogleHomePage _googleHomePage;
        IWebDriver _driver;
        public Page(IWebDriver driver)
        {
            Driver = driver;
        }

        public void Register()
        {
            _googleHomePage = new GoogleHomePage(Driver);
        }

        public GoogleHomePage GoogleHomePage
        {
            get
            {
                return _googleHomePage;

            }
           
        }
        public IWebDriver Driver
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
      
    }
}
