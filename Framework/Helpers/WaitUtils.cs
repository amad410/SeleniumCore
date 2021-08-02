using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Framework.Helpers
{
    /// <summary>
    /// This is a utility class that models different custom wait methods. This can be extended
    /// as well to incorporate new wait functionality to handle synchronization within the app.
    /// </summary>
    public static class WaitUtils
    {
        private static WebDriverWait _wait;
        public static bool WaitUntilLoaded(IWebDriver driver, TimeSpan timeout)
        {
            bool readyState = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                IJavaScriptExecutor jsExecutor = driver as IJavaScriptExecutor;

                readyState = _wait.Until((x) =>
                {
                    return (jsExecutor).ExecuteScript("return document.readyState").Equals("complete");
                });
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Unable to get browser");
            }
            catch (WebDriverException ex)
            {
                throw new Exception("Unable to connect");
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Page was not loaded within {0} seconds", timeout.ToString()));
            }

            return readyState;
        }
        public static IWebElement WaitForElementPresent(IWebDriver driver,By locator, TimeSpan timeout)
        {
            IWebElement element = null;
            try
            {

                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                _wait.Until(drv => drv.FindElements(locator).Any());
                element = driver.FindElement(locator);
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new Exception(String.Format("Unable to find element using locator {0} within the duration {1}", locator.ToString(), timeout.ToString()));
            }
            return element;
        }
        public static ReadOnlyCollection<IWebElement> WaitForElementsPresent(IWebDriver driver, By locator, TimeSpan timeout)
        {
            ReadOnlyCollection<IWebElement> elements = null; ;
            try
            {

                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if(IsElementPresent(driver,locator, timeout) == true)
                {
                    elements = driver.FindElements(locator);
                }
                
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new Exception(String.Format("Unable to find elements using locator {0} within the duration {1}", locator.ToString(), timeout.ToString()));
            }
            return elements;
        }
        public static ReadOnlyCollection<IWebElement> WaitForElementsVisible(IWebDriver driver, By locator, TimeSpan timeout)
        {
            ReadOnlyCollection<IWebElement> elements = null; ;
            try
            {

                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    elements = driver.FindElements(locator);
                }

            }
            catch (WebDriverTimeoutException ex)
            {
                throw new Exception(String.Format("Unable to find visible elements using locator {0} within the duration {1}", locator.ToString(), timeout.ToString()));
            }
            return elements;
        }
        public static Boolean IsElementPresent(IWebDriver driver, By locator, TimeSpan timeout)
        {
            Boolean isPresent = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                isPresent = _wait.Until(drv => drv.FindElements(locator).Any());
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isPresent;
        }
        public static Boolean IsElementNotPresent(IWebDriver driver, By locator, TimeSpan timeout)
        {
            Boolean isNotPresent = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                isNotPresent = _wait.Until(drv => !drv.FindElements(locator).Any());
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isNotPresent;
        }
        public static Boolean IsClickable(IWebDriver driver, By locator, TimeSpan timeout)
        {
            Boolean isClickable = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    if (IsEnabled(driver, locator, timeout) == true)
                    {
                        isClickable = true;
                    }
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isClickable;
        }
        public static IWebElement WaitForElementClickable(IWebDriver driver, By locator, TimeSpan timeout)
        {
            IWebElement element = null;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    if (IsEnabled(driver, locator, timeout) == true)
                    {
                        element = driver.FindElement(locator);
                    }
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new Exception(String.Format("Unable to find clickable element using locator {0} within the duration {1}", locator.ToString(), timeout.ToString()));
            }
            return element;
        }
        public static Boolean IsElementContainingText(IWebDriver driver, By locator, String text, TimeSpan timeout)
        {
            Boolean containsText = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    if (driver.FindElement(locator).Text.Contains(text) == true)
                    {
                        containsText = true;
                    }
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return containsText;
        }
        public static Boolean IsElementNotContainingText(IWebDriver driver, By locator, String text, TimeSpan timeout)
        {
            Boolean textNotAvailable = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    if (driver.FindElement(locator).Text.Contains(text) == false)
                    {
                        textNotAvailable = true;
                    }
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return textNotAvailable;
        }
        public static IWebElement WaitForElementContainingText(IWebDriver driver, By locator, String text, TimeSpan timeout)
        {
            IWebElement element = null;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if (IsElementContainingText(driver, locator,text, timeout) == true)
                {
                    element = driver.FindElement(locator);
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new Exception(String.Format("Unable to find element containing text {0} using locator {1} within the duration {2}", text,locator.ToString(), timeout.ToString()));
            }
            return element;
        }
        public static Boolean IsElementVisible(IWebDriver driver, By locator, TimeSpan timeout)
        {
            Boolean isVisible = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                isVisible = _wait.Until(drv => drv.FindElement(locator).Displayed);
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isVisible;
        }
        public static Boolean IsElementNotVisible(IWebDriver driver, By locator, TimeSpan timeout)
        {
            Boolean isNotVisible = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                isNotVisible = _wait.Until(drv => !drv.FindElement(locator).Displayed);
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isNotVisible;
        }
        public static Boolean IsEnabled(IWebDriver driver, By locator, TimeSpan timeout)
        {
            Boolean isEnabled = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                if(IsElementVisible(driver,locator,timeout) == true)
                {
                    isEnabled = _wait.Until(drv => drv.FindElement(locator).Enabled);
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isEnabled;
        }
        public static Boolean IsElementSelected(IWebDriver driver, By locator, TimeSpan timeout)
        {
            Boolean isSelected = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    isSelected = _wait.Until(drv => drv.FindElement(locator).Selected);
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isSelected;
        }
        public static Boolean IsElementNotSelected(IWebDriver driver, By locator, TimeSpan timeout)
        {
            Boolean isNotSelected = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    isNotSelected = _wait.Until(drv => !drv.FindElement(locator).Selected);
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isNotSelected;
        }
        public static Boolean IsAttributePresent(IWebDriver driver, By locator, String attribute, TimeSpan timeout)
        {
            Boolean isAttributePresent = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                if (IsElementPresent(driver, locator, timeout) == true)
                {
                    isAttributePresent = _wait.Until(drv => drv.FindElement(locator).GetAttribute(attribute).Equals("true"));
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isAttributePresent;
        }
        public static Boolean IsAttributeValueFound(IWebDriver driver, By locator, String attribute, String value, TimeSpan timeout)
        {
            Boolean isValueFound = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                if (IsElementPresent(driver, locator, timeout) == true)
                {
                    isValueFound = _wait.Until(drv => drv.FindElement(locator).GetAttribute(attribute).Contains(value));
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isValueFound;
        }
        public static Boolean IsAttributeValueNotFound(IWebDriver driver, By locator, String attribute, String value, TimeSpan timeout)
        {
            Boolean isValueNotFound = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                if (IsElementPresent(driver, locator, timeout) == true)
                {
                    isValueNotFound = _wait.Until(drv => !drv.FindElement(locator).GetAttribute(attribute).Contains(value));
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isValueNotFound;
        }
        public static Boolean IsAttributeClassFound(IWebDriver driver, By locator, String attribute, String value, TimeSpan timeout)
        {
            Boolean isValueFound = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                if (IsElementPresent(driver, locator, timeout) == true)
                {
                    isValueFound = _wait.Until(drv => drv.FindElement(locator).GetAttribute("class").Contains(value));
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isValueFound;
        }
        public static Boolean IsAttributeClassNotFound(IWebDriver driver, By locator, String attribute, String value, TimeSpan timeout)
        {
            Boolean isValueNotFound = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                if (IsElementPresent(driver, locator, timeout) == true)
                {
                    isValueNotFound = _wait.Until(drv => !drv.FindElement(locator).GetAttribute("class").Contains(value));
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isValueNotFound;
        }
        public static Boolean IsDisabled(IWebDriver driver, By locator, TimeSpan timeout)
        {
            Boolean isDisabled = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    isDisabled = _wait.Until(drv => !drv.FindElement(locator).Enabled);
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                return false;
            }
            return isDisabled;
        }
        public static IWebElement WaitForElementDisplayed(IWebDriver driver, By locator, TimeSpan timeout)
        {
            IWebElement element = null;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                _wait.Until(drv => drv.FindElement(locator).Displayed == true);
                element = driver.FindElement(locator);
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new Exception(String.Format("Unable to find element that is visible using locator {0} within the duration {1}", locator.ToString(), timeout.ToString()));
            }
            return element;
        }
        public static Boolean IsAdditionalTabDisplayed(IWebDriver driver, TimeSpan timeout)
        {
            Boolean isDisplayed = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if(_wait.Until(drv => drv.WindowHandles.Count >1)==true)
                {
                    isDisplayed = true;
                }                
            }
            catch (Exception ex)
            {
                return false;
            }
            return isDisplayed;
        }
        public static Boolean IsDropDownTextAvailable(IWebDriver driver, By locator, String textOption, TimeSpan timeout)
        {
            Boolean isDisplayed = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    SelectElement oSelect = new SelectElement(WaitForElementDisplayed(driver, locator, timeout));
                    if(oSelect.Options.Any(option=>option.Text.Equals(textOption)) == true)
                    {
                        isDisplayed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return isDisplayed;
        }
        public static Boolean IsDropDownPopulated(IWebDriver driver, By locator, String textOption, TimeSpan timeout)
        {
            Boolean isDisplayed = false;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    SelectElement oSelect = new SelectElement(WaitForElementDisplayed(driver, locator, timeout));
                    if (oSelect.Options.Count >=1)
                    {
                        isDisplayed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return isDisplayed;
        }
        public static SelectElement WaitForDropDownPopulated(IWebDriver driver, By locator, TimeSpan timeout)
        {
            SelectElement oSelect = null;
            try
            {
                _wait = new WebDriverWait(driver, timeout);
                _wait.PollingInterval = TimeSpan.FromMilliseconds(500);
                _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

                if (IsElementVisible(driver, locator, timeout) == true)
                {
                    oSelect = new SelectElement(WaitForElementDisplayed(driver, locator, timeout));
                    if (oSelect.Options.Count >= 1)
                    {
                        return oSelect;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Unable to find dropdown element using locator {0} within the duration {1}", locator.ToString(), timeout.ToString()));
            }
            return oSelect;
        }
    }
}
