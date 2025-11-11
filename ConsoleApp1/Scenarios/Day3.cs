using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class Day3
{
    const int N = 1_000_000;

    public static void Run()
    {
        int[] data = Enumerable.Range(1, N).ToArray();
        Console.WriteLine("🔹 Bắt đầu benchmark...\n");

        // 1️⃣ CPU-bound tuần tự
        Measure("CPU Sequential", () =>
        {
            long sum = 0;
            foreach (var x in data) sum += (long)x * x;
            return sum;
        });

        // 2️⃣ CPU-bound Parallel.ForEach (thread-local sum)
        Measure("CPU Parallel.For", () =>
        {
            long total = 0;
            Parallel.ForEach(data,
                () => 0L, // thread-local sum
                (x, loopState, localSum) => localSum + (long)x * x,
                localSum => Interlocked.Add(ref total, localSum)
            );
            return total;
        });

        // 3️⃣ CPU-bound PLINQ
        Measure("CPU PLINQ", () =>
        {
            return data.AsParallel().Sum(x => (long)x * x);
        });

        // 4️⃣ Async I/O giả lập (batch để tránh tạo 1 triệu Task)
        MeasureAsync("Async I/O Task.Delay", async () =>
        {
            long total = 0;
            const int batchSize = 10000;

            for (int i = 0; i < data.Length; i += batchSize)
            {
                var batch = data.Skip(i).Take(batchSize)
                                .Select(async x =>
                                {
                                    await Task.Delay(1); // giả lập I/O
                                    return (long)x * x;
                                });
                var results = await Task.WhenAll(batch);
                total += results.Sum();
            }
            return total;
        }).GetAwaiter().GetResult();
    }

    // Code đồng bộ
    private static void Measure(string label, Func<long> action)
    {
        var sw = Stopwatch.StartNew();
        var result = action();
        sw.Stop();
        Console.WriteLine($"{label,-20}: {sw.ElapsedMilliseconds,5} ms (KQ={result})");
    }

    // Code bất đồng bộ
    private static async Task MeasureAsync(string label, Func<Task<long>> func)
    {
        var sw = Stopwatch.StartNew();
        var result = await func();
        sw.Stop();
        Console.WriteLine($"{label,-20}: {sw.ElapsedMilliseconds,5} ms (KQ={result})");
    }
}
