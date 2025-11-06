using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Utils;

namespace ConsoleApp1;

public class Day5
{
    private static readonly string filePath = @"C:\Logs\biglog.txt";

    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== 🔍 DAY 5 – LOG ANALYZER TOOL ===");

        // SYNC
        PerformanceTimer.Measure("Đọc & phân tích (Sync)", () =>
        {
            var result = AnalyzeSync(filePath);
            PrintResult(result, "SYNC");
        }); 

        // ASYNC
        await PerformanceTimer.MeasureAsync("Đọc & phân tích (Async I/O)", async () =>
        {
            var result = await AnalyzeAsync(filePath);
            PrintResult(result, "ASYNC");
        });

        // PARALLEL
        PerformanceTimer.Measure("Đọc & phân tích (Parallel.ForEach)", () =>
        {
            var result = AnalyzeParallel(filePath);
            PrintResult(result, "PARALLEL");
        });
    }

    private static Dictionary<string, int> AnalyzeSync(string path)
    {
        var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in File.ReadLines(path))
        {
            string type = ExtractLogType(line);
            if (type == null) continue;

            dict[type] = dict.TryGetValue(type, out var count) ? count + 1 : 1;
        }

        return dict;
    }
    
    private static async Task<Dictionary<string, int>> AnalyzeAsync(string path)
    {
        var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        using var reader = new StreamReader(path);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            string type = ExtractLogType(line);
            if (type == null) continue;

            dict[type] = dict.TryGetValue(type, out var count) ? count + 1 : 1;
        }

        return dict;
    }

    private static Dictionary<string, int> AnalyzeParallel(string path)
    {
        var dict = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var lines = File.ReadLines(path);

        Parallel.ForEach(lines, line =>
        {
            string type = ExtractLogType(line);
            if (type == null) return;

            dict.AddOrUpdate(type, 1, (_, oldValue) => oldValue + 1);
        });

        return new Dictionary<string, int>(dict);
    }
    
    private static string ExtractLogType(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return null;

        // Giả sử log format: [ERROR] Something failed...
        if (line.StartsWith("[") && line.Contains("]"))
        {
            int end = line.IndexOf("]");
            return line.Substring(1, end - 1).Trim().ToUpperInvariant();
        }

        return "UNKNOWN";
    }

    private static void PrintResult(Dictionary<string, int> result, string label)
    {
        Console.WriteLine($"\n🔹 Thống kê ({label}):");
        foreach (var kv in result.OrderByDescending(k => k.Value))
            Console.WriteLine($"  {kv.Key,-10}: {kv.Value,8} dòng");
    }
}