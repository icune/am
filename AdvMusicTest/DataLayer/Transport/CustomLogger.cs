using System.Collections;
using Microsoft.Extensions.Logging;

namespace AdvMusicTest.DataLayer.Transport;

public class CustomLogger : ILogger
{

    public IDisposable BeginScope<TState>(TState state)
    {
        return new NoopDisposable();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        string message = "";
        if (formatter != null)
        {
            message += formatter(state, exception);
        }
        Console.WriteLine($"{logLevel.ToString()} - {message}");
    }

    private class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}