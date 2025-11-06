using System;
namespace ConsoleApp1.Interfaces;

public interface ILogger
{
    void Info(string message);
    void Warn(string message);
    void Error(string message, Exception ex);
}