using System;
using System.Collections.Generic;
using System.IO;
using ConsoleApp1.Interfaces;

namespace ConsoleApp1.Services
{
    public class FileReader : IFileReader
    {
        private readonly ILogger _logger;
        public FileReader(ILogger logger)
        {
            _logger = logger;   
        }

        public IEnumerable<string> ReadLines(string path)
        {
            try
            {
                if (!File.Exists(path)) throw new FileNotFoundException($"File not found {path}");
                _logger.Info($"Đọc file {path}");
                return File.ReadLines(path);
            }
            catch (Exception e)
            {
                _logger.Error("Lỗi khi đọc file", e);
                throw;
            }
        }
    }
}