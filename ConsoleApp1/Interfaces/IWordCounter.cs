using System.Collections.Generic;
namespace ConsoleApp1.Interfaces;

public interface IWordCounter
{
    IDictionary<string, int> CountWords(IEnumerable<string> lines);
}