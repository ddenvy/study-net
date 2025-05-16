using System;

namespace GCBasicsExample
{
    public class DemoClass
    {
        ~DemoClass()
        {
            Console.WriteLine("Финализатор вызван!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Память до создания объектов: {GC.GetTotalMemory(false)} байт");

            for (int i = 0; i < 10000; i++)
            {
                var obj = new DemoClass();
            }

            Console.WriteLine($"Память после создания объектов: {GC.GetTotalMemory(false)} байт");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Память после GC.Collect: {GC.GetTotalMemory(false)} байт");
        }
    }
} 