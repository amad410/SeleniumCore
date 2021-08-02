using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Handlers
{
    /// <summary>
    /// Custom Exception handler to throw custom exceptions. 
    /// </summary>
    public class ExceptionHandler
    {

        public void throwException(Exception exception)
        {
            try
            {
                throw exception;
            }
            
            catch (ElementNotFoundException ex)
            {
                throw new ElementNotFoundException(ex.Message, ex.InnerException);
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new ElementNotFoundException(ex.Message, ex.InnerException);
            }
            catch (ElementNotVisibleException ex)
            {
                throw new ElementNotFoundException(ex.Message, ex.InnerException);
            }
            catch (ElementNotInteractableException ex)
            {
                throw new ElementNotFoundException(ex.Message, ex.InnerException);
            }
            catch (ElementNotSelectableException ex)
            {
                throw new ElementNotFoundException(ex.Message, ex.InnerException);
            }
            catch (WebDriverException ex)
            {

            }
        }
       
    }
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException()
        {

        }
        public ElementNotFoundException(String message): base(message)
        {

        }
        public ElementNotFoundException(String message, Exception inner) : base(message,inner)
        {

        }

    }
}
