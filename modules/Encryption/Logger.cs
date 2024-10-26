using System;

public class Logger
{
    public void LogEncryptionActivity(string activity)
    {
        var timestamp = DateTime.Now.ToString("o");
        Console.WriteLine($"[{timestamp}] ENCRYPTION: {activity}");
    }

    public void LogError(Exception error)
    {
        var timestamp = DateTime.Now.ToString("o");
        Console.WriteLine($"[{timestamp}] ERROR: {error.Message}");
    }
}
