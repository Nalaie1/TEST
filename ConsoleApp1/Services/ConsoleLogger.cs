using System;
namespace ConsoleApp1.Services;

public class ConsoleLogger
{
    public void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[INFO] {"DateTime.Now:HH:mm:ss"} - {message}");
        Console.ResetColor();
    }
    public void Error(string message, Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {"DateTime.Now:HH:mm:ss"} - {message}");
        Console.WriteLine($"→ {ex.Message}");
        Console.ResetColor();
    }
    public void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARN] {"DateTime.Now:HH:mm:ss"} - {message}");
        Console.ResetColor();
    }
}