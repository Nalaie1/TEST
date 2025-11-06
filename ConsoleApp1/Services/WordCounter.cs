using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Interfaces;

namespace ConsoleApp1.Services
{
    public class WordCounter : IWordCounter
    {
        private readonly ILogger _logger;
        public  WordCounter(ILogger logger)
        {
            _logger = logger;
        }

        public IDictionary<string, int> CountWords(IEnumerable<string> lines)
        {
            var wordCounts = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            try
            {
                _logger.Info("Bắt đầu xử lý song song...");
                Parallel.ForEach(lines, line =>
                {
                    var words = line.Split(
                        new char[]
                        {
                            ' ', '\t', ',', '.', ';', ':', '-', '_', '/', '\\', '"', '\'', '(', ')', '[', ']', '{', '}'
                        },
                        StringSplitOptions.RemoveEmptyEntries);

                    foreach (var word in words)
                        wordCounts.AddOrUpdate(word, 1, (_, old) => old + 1);
                });
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                    _logger.Error("Lỗi trong xử lý song song", e);
            }

            return wordCounts 
                .OrderByDescending(Kvp => Kvp.Value)
                .Take(10)
                .ToDictionary(Kvp => Kvp.Key, Kvp => Kvp.Value);
        }
    }
}

