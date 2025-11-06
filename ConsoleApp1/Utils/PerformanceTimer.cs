using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleApp1.Utils
{
    public class PerformanceTimer : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _label;

        public PerformanceTimer(string label)
        {
            _label = label;
            _stopwatch = Stopwatch.StartNew();
            Console.WriteLine($"Bắt đầu: {_label}");
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Console.WriteLine($"Kết thúc {_label} - Thời gian: {_stopwatch.ElapsedMilliseconds} ms\n");
        }

        // 🔹 1️⃣ Đo thời gian cho hành động không trả về giá trị
        public static void Measure(string label, Action action)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"▶️ {label}...");
            action(); // Chạy hành động
            sw.Stop();
            Console.WriteLine($"⏱️ {label} hoàn thành trong {sw.ElapsedMilliseconds} ms\n");
        }

        // 🔹 2️⃣ Đo thời gian cho hành động có giá trị trả về
        public static T Measure<T>(string label, Func<T> func)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"▶️ {label}...");
            var result = func(); // chạy hàm và lấy kết quả trả về
            sw.Stop();
            Console.WriteLine($"⏱️ {label} hoàn thành trong {sw.ElapsedMilliseconds} ms\n");
            return result;
        }

        // 🔹 3️⃣ Đo thời gian cho hành động bất đồng bộ (async)
        public static async Task MeasureAsync(string label, Func<Task> func)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"▶️ {label} (async)...");
            await func();
            sw.Stop();
            Console.WriteLine($"⏱️ {label} hoàn thành trong {sw.ElapsedMilliseconds} ms\n");
        }
    }
}