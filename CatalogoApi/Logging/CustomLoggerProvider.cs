using System.Collections.Concurrent;

namespace CatalogoApi.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    readonly CustomLoggerProviderConfiguration loggerConfig;

    readonly ConcurrentDictionary<string, CustomerLogger> loggers =
        new ConcurrentDictionary<string, CustomerLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration configuration)
    {
        loggerConfig = configuration;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
    }

    public void Dispose()
    {
        loggers.Clear();
    }
}
