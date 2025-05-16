using System;

namespace DataTypesExample
{
    // Пользовательская структура (значимый тип)
    struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Пример 1: Работа с целочисленными типами
            byte smallNumber = 255;
            int mediumNumber = 1000000;
            long bigNumber = 9000000000000000000;

            Console.WriteLine($"Byte: {smallNumber}");
            Console.WriteLine($"Int: {mediumNumber}");
            Console.WriteLine($"Long: {bigNumber}");

            // Пример 2: Работа с числами с плавающей точкой
            float pi = 3.14159f;
            double e = 2.718281828459045;
            decimal precise = 1.234567890123456789m;

            Console.WriteLine($"Float: {pi}");
            Console.WriteLine($"Double: {e}");
            Console.WriteLine($"Decimal: {precise}");

            // Пример 3: Преобразование типов
            int intValue = 100;
            double doubleValue = intValue; // Неявное преобразование
            int backToInt = (int)doubleValue; // Явное преобразование

            Console.WriteLine($"Int to Double: {doubleValue}");
            Console.WriteLine($"Double to Int: {backToInt}");

            // Пример 4: Nullable типы
            int? nullableInt = null;
            DateTime? nullableDate = null;

            Console.WriteLine($"Nullable Int: {nullableInt ?? 0}");
            Console.WriteLine($"Nullable Date: {nullableDate?.ToString() ?? "Not set"}");

            // Пример 5: Работа со структурами
            Point p1 = new Point(10, 20);
            Point p2 = p1; // Копирование структуры

            p2.X = 30;
            Console.WriteLine($"P1: ({p1.X}, {p1.Y})");
            Console.WriteLine($"P2: ({p2.X}, {p2.Y})");

            // Пример 6: Строки и символы
            char symbol = 'A';
            string text = "Hello, World!";

            Console.WriteLine($"Char: {symbol}");
            Console.WriteLine($"String: {text}");
            Console.WriteLine($"String Length: {text.Length}");
            Console.WriteLine($"First Char: {text[0]}");
        }
    }
} 