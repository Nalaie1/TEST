using System;
using ConsoleApp1;
using ConsoleApp1.Interfaces;
using System;
using System.Threading.Tasks;
using ConsoleApp1.Utils;
using ConsoleApp1.Services;


class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        // Hiển thị menu cho người dùng chọn bài muốn chạy
        Console.WriteLine("Chọn chế độ chạy:");
        Console.WriteLine("1. Đếm tần suất từ (Parallel.ForEach)");
        Console.WriteLine("2. Benchmark xử lý 10^6 record");
        Console.WriteLine("3. Log AnalYzer Tool");
        Console.Write("Nhập lựa chọn: ");

        string? choice = Console.ReadLine(); // Nhận lựa chọn từ bàn phím

        switch (choice)
        {
            case "1":
                Day12.Run();
                break;
            case "2":
                Day3.Run();
                break;
            case "3":
                Day5.RunAsync();
                break;
            default:
                Console.WriteLine("Lựa chọn không hợp lệ.");
                break;
        }
    }
}