using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Pages
{
    /// <summary>
    /// This class is the core navigation class, which can be extended to include common 
    /// navigation within the application.
    /// </summary>
    public class Navigation
    {
        public Navigation(){}

        public void Back(IWebDriver driver)
        {
            driver.Navigate().Back();

        }
        public void Forward(IWebDriver driver)
        {
            driver.Navigate().Forward();

        }

    }
}
