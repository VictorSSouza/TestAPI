namespace CatalogAPI.Logging
{
    public class CustomLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerName = name;
            loggerConfig = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            WriteTextInFile(message);
        }

        private void WriteTextInFile(string message)
        {
            string filePathLog = @"c:\temp\log\Victor_log.txt";
            using (StreamWriter sw = new StreamWriter(filePathLog, true)) 
            {
                try
                {
                    sw.WriteLine(message);
                    sw.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
