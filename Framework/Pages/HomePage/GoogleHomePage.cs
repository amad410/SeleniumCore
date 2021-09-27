using Framework.Enums;
using Framework.Handlers;
using Framework.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Pages.HomePage
{
    public class GoogleHomePage : BasePage
    {

        public GoogleHomePage(IWebDriver driver) : base(driver) { }
        By searchBx = By.Name("q");
        public GoogleHomePage PerformSearch(String text)
        {
            
            FindVisibleElement(searchBx,TimeSpan.FromSeconds(8)).SendKeys(text);
            return this;
        }
        public GoogleHomePage PerformSearch2(String text)
        {
            FindVisibleElement(searchBx, TimeSpan.FromSeconds(8)).SendKeys(text);
            return this;
        }
    }
}
