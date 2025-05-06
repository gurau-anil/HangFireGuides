namespace Hangfire.Services.Services
{
    public interface ILoggingService
    {
        Task LogMessageAsync(string message);
    }

    public class LoggingService: ILoggingService
    {
        public async Task LogMessageAsync(string message)
        {
            Console.WriteLine($"[Job Started] Processing log for: {message} at {DateTime.Now}");
            // Simulate a time-consuming operation (e.g., 5 seconds)
            await Task.Delay(15000);

            string logFilePath = "processed.log"; //  Make sure this directory is writeable!
            string logEntry = $"{DateTime.UtcNow}: [Processed] {message}{Environment.NewLine}";
            File.AppendAllText(logFilePath, logEntry); // Simple file logging
            Console.WriteLine($"[Job Completed] Logged: {message} at {DateTime.Now}");
        }
    }
}
