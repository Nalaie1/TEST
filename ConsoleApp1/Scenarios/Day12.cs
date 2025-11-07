using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Utils;

namespace ConsoleApp1;

public class Day12
{
    public static void Run()
    {
        string filePath = @"C:\Users\Admin\RiderProjects\ConsoleApp1\ConsoleApp1\Logs\biglog.txt"; // file log lớn
        var wordCounts = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        Console.WriteLine("🔹 Bắt đầu đếm tần suất từ...\n");

        // Đọc file theo từng dòng (không load hết vào RAM)
        var lines = File.ReadLines(filePath);

        // Xử lý song song từng dòng và đo hiệu năng
        PerformanceTimer.Measure("Xử lý song song (Parallel.ForEach)", () =>
        {
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
        });

        // Lấy 10 từ xuất hiện nhiều nhất và đo hiệu năng
        var top10 = PerformanceTimer.Measure("Sắp xếp và lấy Top 10", () =>
        {
            return wordCounts
                .OrderByDescending(kvp => kvp.Value)
                .Take(10)
                .ToList();
        });

        Console.WriteLine("\n📊 Top 10 từ xuất hiện nhiều nhất:");
        foreach (var kvp in top10)
        {
            Console.WriteLine($"  {kvp.Key,-20}: {kvp.Value,8} lần");
        }
    }
}   