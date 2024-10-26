using System;
using System.Collections.Generic;

namespace ErrorHandling
{
    // Define Error Types
    public enum ErrorType
    {
        ClientError,
        ServerError,
        NetworkError,
        WebSocketError
    }

    // Define Severity Levels
    public enum SeverityLevel
    {
        Critical,
        High,
        Medium,
        Low
    }

    // Error Class
    public class ErrorDetails
    {
        public ErrorType ErrorType { get; set; }
        public SeverityLevel Severity { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? ErrorCode { get; set; }
        public string? ModuleName { get; set; }
        public string? FunctionName { get; set; }
        public int LineNumber { get; set; }
        public string? RequestData { get; set; }
        public string? UserDetails { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Environment { get; set; }
        public string? ServerIdentifier { get; set; }
    }

    // Main ErrorHandler Class
    public class ErrorHandler
    {
        private readonly ErrorLogger _logger;
        private readonly ErrorNotifier _notifier;

        public ErrorHandler()
        {
            _logger = new ErrorLogger();
            _notifier = new ErrorNotifier();
        }

        public void LogError(ErrorDetails error)
        {
            _logger.LogError(error);
            if (error.Severity == SeverityLevel.Critical)
            {
                _notifier.SendAlert($"Critical Error: {error.Message}");
            }
        }
    }

    // Logger Service
    public class ErrorLogger
    {
        public void LogError(ErrorDetails error)
        {
            Console.WriteLine($"[{error.Timestamp}] [{error.Severity}] {error.ErrorType} - {error.Message}");
        }
    }

    // Notifier Service
    public class ErrorNotifier
    {
        public void SendAlert(string message)
        {
            Console.WriteLine($"Notification: {message}");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var errorHandler = new ErrorHandler();

            // Simulate capturing an error
            var error = new ErrorDetails
            {
                ErrorType = ErrorType.ServerError,
                Severity = SeverityLevel.Critical,
                Message = "Null reference exception.",
                StackTrace = "at Program.Main()",
                ErrorCode = "500",
                ModuleName = "WebSocketProxyModule",
                FunctionName = "Connect",
                LineNumber = 42,
                RequestData = "Request data here.",
                UserDetails = "UserID: 12345",
                Timestamp = DateTime.Now,
                Environment = "Production",
                ServerIdentifier = "Server1"
            };

            // Log the error
            errorHandler.LogError(error);
        }
    }
}

