using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Handlers
{
    /// <summary>
    /// Interface contract for logging messages using NLog
    /// </summary>
    public interface ITestLogger
    {
        void Debug(String message, String arg= null);
        void Info(String message, String arg = null);
        void Error(String message, String arg = null);

    }
}
