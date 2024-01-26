using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.CrossCutting.Logging
{
    public class DatabaseLoggerProvider : ILoggerProvider, IDisposable
    {
        public bool dispose = false;
        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger();
        }

        public void Dispose()
        {
            if (dispose == true)
            {
                GC.Collect();
                GC.SuppressFinalize(this);

            }
            // Cleanup resources if needed
        }
    }

    public class DatabaseLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, System.Exception? exception, Func<TState, System.Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
