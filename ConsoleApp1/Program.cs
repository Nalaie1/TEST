using System;
using ConsoleApp1;

class Program
{
    static void Main(string[] args)
    {
        // Hiển thị menu cho người dùng chọn bài muốn chạy
        Console.WriteLine("Chọn chế độ chạy:");
        Console.WriteLine("1. Đếm tần suất từ (Parallel.ForEach)");
        Console.WriteLine("2. Benchmark xử lý 10^6 record");
        Console.Write("Nhập lựa chọn (1/2): ");

        string? choice = Console.ReadLine(); // Nhận lựa chọn từ bàn phím

        switch (choice)
        {
            case "1":
                // Gọi hàm đếm tần suất từ (bài Day 1–2)
                Day1.Run();
                break;
            case "2":
                // Gọi hàm benchmark (bài Day 3)
                Day2.Run();
                break;
            default:
                Console.WriteLine("Lựa chọn không hợp lệ.");
                break;
        }
    }
}