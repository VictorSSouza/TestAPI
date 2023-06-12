using System.Collections.Concurrent;

namespace CatalogAPI.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomLoggerProviderConfiguration loggerConfig;
        readonly ConcurrentDictionary<string, CustomLogger> loggerCache =
                new ConcurrentDictionary<string,CustomLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
        {
            loggerConfig = config;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return loggerCache.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig));
        }

        public void Dispose()
        {
            loggerCache.Clear();
        }
    }
}
