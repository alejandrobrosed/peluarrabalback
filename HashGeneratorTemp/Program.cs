using System;

class Program
{
    static void Main()
    {
        string hash = BCrypt.Net.BCrypt.HashPassword("password");
        Console.WriteLine(hash);
    }
}