using ConsoleApp1.Interfaces;
using ConsoleApp1.Services;
using ConsoleApp1.Utils;
    
namespace ConsoleApp1.Services
{
    public class LogProcessor
    {
        private readonly ILogger _logger;
        private readonly IFileReader _fileReader;
        private readonly IWordCounter _wordCounter;

        public LogProcessor(ILogger logger, IFileReader fileReader, IWordCounter wordCounter)
        {
            _logger = logger;
            _fileReader = fileReader;
            _wordCounter = wordCounter;
        }

        public void Run(string filePath)
        {
            var lines = _fileReader.ReadLines(filePath);
            var top10 = PerformanceTimer.Measure("Đếm từ", () => _wordCounter.CountWords(lines));

            _logger.Info("Top 10 từ xuất hiện nhiều nhất:");
            foreach (var kv in top10)
                Console.WriteLine($"{kv.Key}: {kv.Value}");
        }
    }
}

