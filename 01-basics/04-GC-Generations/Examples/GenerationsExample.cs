using System;

namespace GenerationsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            object obj = new object();
            Console.WriteLine($"Поколение нового объекта: {GC.GetGeneration(obj)}");

            // Создаем много объектов, чтобы вызвать сборку мусора
            for (int i = 0; i < 100000; i++)
            {
                var temp = new object();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Поколение объекта после GC.Collect: {GC.GetGeneration(obj)}");

            // Количество сборок для каждого поколения
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                Console.WriteLine($"Сборок для поколения {i}: {GC.CollectionCount(i)}");
            }

            // Крупный объект
            byte[] largeArray = new byte[100_000];
            Console.WriteLine($"Поколение крупного объекта: {GC.GetGeneration(largeArray)}");
        }
    }
} 