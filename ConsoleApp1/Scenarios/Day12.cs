using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1;

public class Day12
{
    public static void Run()
    {
        string filePath = @"C:\Logs\biglog.txt"; // file log lớn
        var wordCounts = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // Đọc file theo từng dòng (không load hết vào RAM)
        var lines = File.ReadLines(filePath);

        // Xử lý song song từng dòng
        Parallel.ForEach(lines, line =>
        {
            var words = line
                .Split(new char[] { ' ', '\t', ',', '.', ';', ':', '-', '_', '/', '\\', '\"', '\'', '(', ')', '[', ']', '{', '}' },
                    StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                wordCounts.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1);
            }
        });

        // Lấy 10 từ xuất hiện nhiều nhất
        var top10 = wordCounts
            .OrderByDescending(kvp => kvp.Value)
            .Take(10);

        Console.WriteLine("Top 10 từ xuất hiện nhiều nhất:");
        foreach (var kvp in top10)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}   