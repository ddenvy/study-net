using System;

namespace ValueTypesExample
{
    // Пример структуры (значимый тип)
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Point({X}, {Y})";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Пример работы с простыми типами
            int number1 = 10;
            int number2 = number1; // Создается копия
            number2 = 20;
            Console.WriteLine($"number1: {number1}, number2: {number2}");

            // Пример работы со структурой
            Point p1 = new Point(10, 20);
            Point p2 = p1; // Создается копия
            p2.X = 30;
            Console.WriteLine($"p1: {p1}, p2: {p2}");

            // Пример работы с перечислением
            DayOfWeek today = DayOfWeek.Monday;
            Console.WriteLine($"Today is: {today}");

            // Пример работы с кортежем
            (int x, int y) coordinates = (10, 20);
            Console.WriteLine($"Coordinates: {coordinates.x}, {coordinates.y}");
        }
    }
} 