namespace ConsoleApp1;
using System;
using System.Diagnostics; //dùng Stopwatch để đo thời gian 
using System.Linq;
using System.Threading.Tasks;

public class Day2
{
    public static void Run()
    {
        const int N = 1_000_000; //số lượng phần tử
        int[] data = Enumerable.Range(1, N).ToArray();

        Console.WriteLine("🔹 Bắt đầu benchmark...\n");

        //chạy theo kiểu tuần tự
        Measure("Tuần tự", () =>
        {
            long sum = 0;
            foreach (var x in data) sum += x * x;
            return sum;
        });

        //chạy song song bằng Parallel.ForEach
        Measure("Parallel.For", () =>
        {
            long sum = 0;
            object lockObj = new();// dùng lock để tránh race condition
            Parallel.ForEach(data, x =>
            {
                long val = x * x;
                lock (lockObj) sum += val; //cập nhật sum an toàn
            });
            return sum;
        });

        //chạy async với nhiều Task
        MeasureAsync("Async (Task.Run)", async () =>
        {
            //tạo 1 triệu task, mỗi task xử lý 1 phần tử
            var tasks = data.Select(x => Task.Run(() => (long)x * x));
            var results = await Task.WhenAll(tasks);
            return results.Sum();
        }).GetAwaiter().GetResult();//chạy async trong Main sync
    }

    //Hàm đo thời gian cho code đồng bộ
    private static void Measure(string label, Func<long> action)
    {
        var sw = Stopwatch.StartNew();
        var result = action();
        sw.Stop();
        Console.WriteLine($"{label,-15}: {sw.ElapsedMilliseconds,5} ms (KQ={result})");
    }

    //Hàm đo thời gian cho code bất đồng bộ
    private static async Task MeasureAsync(string label, Func<Task<long>> func)
    {
        var sw = Stopwatch.StartNew();
        var result = await func();
        sw.Stop();
        Console.WriteLine($"{label,-15}: {sw.ElapsedMilliseconds,5} ms (KQ={result})");
    }
}