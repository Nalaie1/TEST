using System.Collections.Generic;
namespace ConsoleApp1.Interfaces;

public interface IFileReader
{
    IEnumerable<string> ReadLines(string path);
}