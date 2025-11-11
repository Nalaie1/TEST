using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Day12
    {
        public static void Run()
        {
            string filePath = @"C:\Users\Admin\RiderProjects\ConsoleApp1\ConsoleApp1\Logs\biglog.txt";

            Console.WriteLine("🔹 Bắt đầu đếm tần suất từ trong file log lớn...\n");

            // Đọc từng dòng, không load hết vào RAM (hiệu quả cho file 5GB)
            var lines = File.ReadLines(filePath);

            // =============================
            // 🧩 1️⃣ XỬ LÝ TUẦN TỰ (SEQUENTIAL)
            // =============================
            var sequentialWordCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var swSequential = Stopwatch.StartNew();

            foreach (var line in lines)
            {
                var words = line.Split(new char[]
                {
                    ' ', '\t', ',', '.', ';', ':', '-', '_', '/', '\\', '\"', '\'', '(', ')', '[', ']', '{', '}'
                }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    if (sequentialWordCounts.ContainsKey(word))
                        sequentialWordCounts[word]++;
                    else
                        sequentialWordCounts[word] = 1;
                }
            }

            swSequential.Stop();
            Console.WriteLine($"⏱ Xử lý tuần tự: {swSequential.ElapsedMilliseconds} ms, tổng từ: {sequentialWordCounts.Count}");

            // =============================
            // 🧩 2️⃣ XỬ LÝ SONG SONG (Parallel.ForEach)
            // =============================
            var parallelWordCounts = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var swParallel = Stopwatch.StartNew();

            Parallel.ForEach(File.ReadLines(filePath), line =>
            {
                var words = line.Split(new char[]
                {
                    ' ', '\t', ',', '.', ';', ':', '-', '_', '/', '\\', '\"', '\'', '(', ')', '[', ']', '{', '}'
                }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    parallelWordCounts.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1);
                }
            });

            swParallel.Stop();
            Console.WriteLine($"⚡ Xử lý song song: {swParallel.ElapsedMilliseconds} ms, tổng từ: {parallelWordCounts.Count}");

            // =============================
            // 🧩 3️⃣ SO SÁNH KẾT QUẢ
            // =============================
            Console.WriteLine("\n📊 So sánh tốc độ:");
            Console.WriteLine($"   - Tuần tự : {swSequential.ElapsedMilliseconds} ms");
            Console.WriteLine($"   - Song song: {swParallel.ElapsedMilliseconds} ms");
            Console.WriteLine($"   ➤ Nhanh hơn khoảng: {Math.Round((double)swSequential.ElapsedMilliseconds / swParallel.ElapsedMilliseconds, 2)}x");

            // =============================
            // 🧩 4️⃣ TOP 10 TỪ XUẤT HIỆN NHIỀU NHẤT
            // =============================
            var top10 = parallelWordCounts
                .OrderByDescending(kvp => kvp.Value)
                .Take(10)
                .ToList();

            Console.WriteLine("\n🔥 Top 10 từ xuất hiện nhiều nhất:");
            foreach (var kvp in top10)
            {
                Console.WriteLine($"   {kvp.Key,-20}: {kvp.Value,8} lần");
            }

            Console.WriteLine("\n✅ Hoàn tất!");
        }
    }
}
