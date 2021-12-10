using Framework.Enums;
using Framework.Handlers;
using Framework.Helpers;
using Framework.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Framework.Assemblies
{
    /// <summary>
    /// This class is the base page that all other page objects inherit from. Includes
    /// common functionality that can be performed on an application page, and makes
    /// use of the navigation class for common navigation functionality.
    /// </summary>
    public class BasePage
    {
        public IWebDriver _driver;
        public Navigation _navigation;


        public BasePage(IWebDriver driver) {
            Driver = driver;
            _navigation = new Navigation();
        }
        public void NavigateBack()
        {
            _navigation.Back(Driver);

        }
        public void NavigateForward()
        {
            _navigation.Forward(Driver);
        }
        public IWebElement FindVisibleElement(By locator, TimeSpan timeout)
        {
            IWebElement elem = null;
            try
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
            }
            catch(StaleElementReferenceException)
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Unable to find visible element matching {0} within the duration {1}" + "\r\n" + ex.StackTrace, locator.ToString(),timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }
            return elem;
        }
        
        public IWebElement FindElement(By locator, TimeSpan timeout)
        {
            IWebElement elem = null;
            try
            {
                elem = WaitHelper.WaitForElementPresent(Driver, locator, timeout);

            }
            catch (StaleElementReferenceException)
            {
                elem = WaitHelper.WaitForElementPresent(Driver, locator, timeout);
            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Unable to find element in DOM matching {0} within the duration {1}" + "\r\n" + ex.StackTrace, locator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }

            return elem;
        }
        public void EnterText(By locator, String text,TimeSpan timeout)
        {
            IWebElement elem = null;
            try
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
                elem.Clear();
                elem.SendKeys(text);

            }
            catch (StaleElementReferenceException)
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
                elem.Clear();
                elem.SendKeys(text);
            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Issue entering text in element matching locator {0} within the duration {1}" + "\r\n" + ex.StackTrace, locator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }
        }
        public void Click(By locator, TimeSpan timeout)
        {
            IWebElement elem = null;
            try
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
                elem.Click();

            }
            catch(ElementNotInteractableException)
            {
                JavaScriptClick(locator, timeout);
            }
            catch (StaleElementReferenceException)
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
                elem.Click();
            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Issue clicking element matching locator {0} within the duration {1}" + "\r\n" + ex.StackTrace, locator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }
        }
        public void SelectDropDown(By locator,String optionText, TimeSpan timeout)
        {
            SelectElement oSelect = null;
            try
            {
                oSelect = WaitHelper.WaitForDropDownPopulated(Driver, locator,timeout);
                oSelect.SelectByText(optionText);

            }
            catch (StaleElementReferenceException)
            {
                oSelect = WaitHelper.WaitForDropDownPopulated(Driver, locator, timeout);
                oSelect.SelectByText(optionText);
            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Issue selecting drop down text {0} using element matching locator {1} within the duration {2}" + "\r\n" + ex.StackTrace, optionText,locator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }
        }
        public void SelectDropDownByIndex(By locator, int index, TimeSpan timeout)
        {
            SelectElement oSelect = null;
            try
            {
                oSelect = WaitHelper.WaitForDropDownPopulated(Driver, locator, timeout);
                oSelect.SelectByIndex(index);

            }
            catch (StaleElementReferenceException)
            {
                oSelect = WaitHelper.WaitForDropDownPopulated(Driver, locator, timeout);
                oSelect.SelectByIndex(index);
            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Issue selecting drop down index {0} using element matching locator {1} within the duration {2}" + "\r\n" + ex.StackTrace, index.ToString(), locator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }
        }
        public ReadOnlyCollection<IWebElement> FindElements(By locator, TimeSpan timeout)
        {
            ReadOnlyCollection<IWebElement> elems = null;
            try
            {
                elems = WaitHelper.WaitForElementsPresent(Driver, locator, timeout);

            }
            catch (StaleElementReferenceException)
            {
                elems = WaitHelper.WaitForElementsPresent(Driver, locator, timeout);
            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Unable to find elements in DOM matching {0} within the duration {1}" + "\r\n" + ex.StackTrace, locator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }

            return elems;
        }
        public ReadOnlyCollection<IWebElement> FindVisibleElements(By locator, TimeSpan timeout)
        {
            ReadOnlyCollection<IWebElement> elems = null;
            try
            {
                elems = WaitHelper.WaitForElementsVisible(Driver, locator, timeout);

            }
            catch (StaleElementReferenceException)
            {
                elems = WaitHelper.WaitForElementsVisible(Driver, locator, timeout);
            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Unable to find visible elements matching {0} within the duration {1}" + "\r\n" + ex.StackTrace, locator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }

            return elems;
        }
        public void SwitchTab(TimeSpan timeout)
        {
            try
            {
                if(WaitHelper.IsAdditionalTabDisplayed(Driver, timeout) == true)
                {
                    Driver.SwitchTo().Window(Driver.WindowHandles[1]);     
                }
            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Unable to switch to new tab window within the duration {0}" + "\r\n" + ex.StackTrace, timeout.ToString()));
                throw new ExceptionHandler(String.Format("Unable to switch to new tab window within the duration {0}", timeout.ToString()), ex.InnerException);
            }
        }

      
        public void ClickAndWaitForElementDisplayed(By locator, By targetedLocator, int numOfAttempts, TimeSpan timeout)
        {
            IWebElement elem = null;
            try
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
                
                for(int i = 0; i <=numOfAttempts; i++)
                {
                    elem.Click();
                    if (WaitHelper.IsElementVisible(Driver, targetedLocator, timeout) == true)
                        break;
                }

            }
            catch(ElementNotInteractableException)
            {
                JavaScriptClickForElementDisplayed(locator, targetedLocator, numOfAttempts, timeout);
            }
            catch (StaleElementReferenceException)
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
               
                for (int i = 0; i <= numOfAttempts; i++)
                {
                    elem.Click();
                    if (WaitHelper.IsElementVisible(Driver, targetedLocator, timeout) == true)
                        break;
                }

            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Issue clicking element matching locator {0}  and waiting for element displayed matching locator {1} within the duration {2}" + "\r\n" + ex.StackTrace, locator.ToString(), targetedLocator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }


        }
        public void ClickAndWaitForElementPresent(By locator, By targetedLocator, int numOfAttempts, TimeSpan timeout)
        {
            IWebElement elem = null;
            try
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
                
                for (int i = 0; i <= numOfAttempts; i++)
                {
                    elem.Click();
                    if (WaitHelper.IsElementPresent(Driver, targetedLocator, timeout) == true)
                        break;
                }

            }
            catch (ElementNotInteractableException)
            {
                JavaScriptClickForElementPresent(locator, targetedLocator, numOfAttempts, timeout);
            }
            catch (StaleElementReferenceException)
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
                
                for (int i = 0; i <= numOfAttempts; i++)
                {
                    elem.Click();
                    if (WaitHelper.IsElementPresent(Driver, targetedLocator, timeout) == true)
                        break;
                }

            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Issue clicking element matching locator {0}  and waiting for element present matching locator {1} within the duration {2}" + "\r\n" + ex.StackTrace, locator.ToString(), targetedLocator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }


        }

        public void JavaScriptClick(By locator, TimeSpan timeout)
        {
            IWebElement elem = null;
            try
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                jsExecutor.ExecuteScript("arguments[0].click()", elem);
            }
            catch (StaleElementReferenceException)
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                jsExecutor.ExecuteScript("arguments[0].click()", elem);

            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Issue clicking element matching locator {0} within the duration {1}" + "\r\n" + ex.StackTrace, locator.ToString(),  timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }

        }
        public void JavaScriptClickForElementDisplayed(By locator, By targetedLocator, int numOfAttempts, TimeSpan timeout)
        {
            IWebElement elem = null;
            try
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);
               
                for (int i = 0; i <= numOfAttempts; i++)
                {
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                    jsExecutor.ExecuteScript("arguments[0].click()", elem);
                    if (WaitHelper.IsElementVisible(Driver, targetedLocator, timeout) == true)
                        break;
                }

            }
            catch (StaleElementReferenceException)
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);

                for (int i = 0; i <= numOfAttempts; i++)
                {
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                    jsExecutor.ExecuteScript("arguments[0].click()", elem);
                    if (WaitHelper.IsElementVisible(Driver, targetedLocator, timeout) == true)
                        break;
                }

            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Issue clicking element matching locator {0}  and waiting for element displayed matching locator {1} within the duration {2}" + "\r\n" + ex.StackTrace, locator.ToString(), targetedLocator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }
        }
        public void JavaScriptClickForElementPresent(By locator, By targetedLocator, int numOfAttempts,TimeSpan timeout)
        {
            IWebElement elem = null;
            try
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);

                for (int i = 0; i <= numOfAttempts; i++)
                {
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                    jsExecutor.ExecuteScript("arguments[0].click()", elem);
                    if (WaitHelper.IsElementPresent(Driver, targetedLocator, timeout) == true)
                        break;
                }

            }
            catch (StaleElementReferenceException)
            {
                elem = WaitHelper.WaitForElementDisplayed(Driver, locator, timeout);

                for (int i = 0; i <= numOfAttempts; i++)
                {
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                    jsExecutor.ExecuteScript("arguments[0].click()", elem);
                    if (WaitHelper.IsElementPresent(Driver, targetedLocator, timeout) == true)
                        break;
                }

            }
            catch (Exception ex)
            {
                TestLogger.GetInstance().Error(String.Format("Issue clicking element matching locator {0}  and waiting for element present matching locator {1} within the duration {2}" + "\r\n" + ex.StackTrace, locator.ToString(), targetedLocator.ToString(), timeout.ToString()));
                throw new ExceptionHandler(ex.Message, ex.InnerException);
            }
        }

        public IWebDriver Driver   
        {
            get { return _driver; }
            set { _driver = value; }
        }

        public BrowserType GetBrowserName
        {
            get
            {
                ICapabilities capabilities = ((RemoteWebDriver)Driver).Capabilities;
                String Name = capabilities.GetCapability(CapabilityType.BrowserName).ToString();

                switch (Name)
                {
                    case "chrome":
                        return BrowserType.Chrome;
                    case "edge":
                        return BrowserType.Edge;
                    case "firefox":
                        return BrowserType.FireFox;
                    default:
                        return BrowserType.InternetExplorer;

                }

            }
        }
      
    }
}
