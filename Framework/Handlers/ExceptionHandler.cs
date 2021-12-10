using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Framework.Handlers
{
    /// <summary>
    /// Custom Exception handler to throw custom exceptions. 
    /// </summary>
    public class ExceptionHandler : Exception
    {
        private Exception exception;
        private string message = null;
        private string fileName = null;
        private Exception innerException;

        public ExceptionHandler(string initialMessage, Exception ex)
        {
            this.message = initialMessage;
            exception = ex;
            Invoke(exception);
        }

        public ExceptionHandler(string initialMessage, string file, Exception ex)
        {
            this.message = initialMessage;
            this.fileName = file;
            exception = ex;
            Invoke(exception);

        }

        public ExceptionHandler(string initialMessage, string file, Exception ex, Exception innerException)
        {
            this.message = initialMessage;
            this.fileName = file;
            exception = ex;
            this.innerException = innerException;
            Invoke(exception);
        }

        public void Invoke(Exception ex)
        {
            switch (ex)
            {
                case FileNotFoundException FileNotFoundException:
                    throw new FilePathException(message, fileName, innerException);
                case NullReferenceException NullReferenceException:
                    throw new NullException(message, ex);
                case InvalidSelectorException InvalidSelectorException:
                    throw new SelectorException(message, ex);
                case NoSuchElementException NoSuchElementException:
                    throw new ElementNotFoundException(message, ex);
                case ElementNotVisibleException ElementNotVisibleException:
                    throw new ElementVisibilityException(message, ex);
                case WebDriverTimeoutException WebDriverTimeoutException:
                    throw new WebTimeOutException(message, ex);
                default:
                    break;

            }
        }
    }
    public class ElementNotFoundException : NotFoundException
    {
        public ElementNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

            throw new Exception(String.Format("There was an issue finding the element: \r\n {0} \r\n", message), innerException.InnerException);

        }
    }
    public class NullException : NullReferenceException
    {
        public NullException(string message, Exception innerException) : base(message, innerException)
        {

            throw new NullReferenceException(String.Format("Object instance or variable was not created or set: \r\n {0} \r\n", message), innerException.InnerException);

        }
    }
    public class FilePathException : FileNotFoundException
    {
        public FilePathException(string message, string file, Exception innerException) : base(message, file, innerException)
        {

            throw new FileNotFoundException(String.Format("File Path Issue: {0} \r\n {1} \r\n", message, file), innerException);

        }

    }
    public class SelectorException : InvalidSelectorException
    {
        public SelectorException(string message, Exception innerException) : base(message, innerException)
        {

            throw new InvalidSelectorException(String.Format("Invalid locator used: \r\n {0} \r\n", message), innerException);

        }

    }
    public class ElementVisibilityException : ElementNotVisibleException
    {
        public ElementVisibilityException(string message, Exception innerException) : base(message, innerException)
        {

            throw new InvalidSelectorException(String.Format("Element cannot be interacted with due to being in a hidden state: \r\n {0} \r\n", message), innerException);

        }

    }
    public class WebTimeOutException : WebDriverTimeoutException
    {
        public WebTimeOutException(string message, Exception innerException) : base(message, innerException)
        {

            throw new WebDriverTimeoutException(String.Format("Finding the element reached the maximum time alotted: \r\n {0} \r\n", message), innerException);

        }

    }
}
