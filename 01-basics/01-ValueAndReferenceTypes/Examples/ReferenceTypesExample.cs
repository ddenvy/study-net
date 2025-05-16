using System;

namespace ReferenceTypesExample
{
    // Пример класса (ссылочный тип)
    public class Rectangle
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double GetArea()
        {
            return Width * Height;
        }

        public override string ToString()
        {
            return $"Rectangle({Width}, {Height})";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Пример работы с классом
            Rectangle rect1 = new Rectangle(10, 20);
            Rectangle rect2 = rect1; // Копируется ссылка
            rect2.Width = 30;
            Console.WriteLine($"rect1: {rect1}, rect2: {rect2}");

            // Пример работы со строкой
            string str1 = "Hello";
            string str2 = str1; // Строки в C# иммутабельны
            str2 = "World";
            Console.WriteLine($"str1: {str1}, str2: {str2}");

            // Пример работы с массивом
            int[] arr1 = new int[] { 1, 2, 3 };
            int[] arr2 = arr1; // Копируется ссылка
            arr2[0] = 10;
            Console.WriteLine($"arr1[0]: {arr1[0]}, arr2[0]: {arr2[0]}");

            // Пример работы с делегатом
            Action<string> printMessage = (message) => Console.WriteLine(message);
            printMessage("Hello from delegate!");
        }
    }
} 