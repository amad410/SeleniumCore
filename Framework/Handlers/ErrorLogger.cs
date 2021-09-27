using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Handlers
{
    public interface IErrorLogger
    {
        void LogError(Exception ex, string infoMessage);
    }

    public class ErrorLogger : IErrorLogger
    {
        public void LogError(Exception ex, string infoMessage)
        {
            //Log the error to your error to NLog
        }
    }
}
