using System;
using System.Runtime;

namespace HeapRegionsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем маленький объект (SOH)
            byte[] smallArray = new byte[1000];
            Console.WriteLine($"Маленький массив создан в SOH");

            // Создаем большой объект (LOH)
            byte[] largeArray = new byte[100_000];
            Console.WriteLine($"Большой массив создан в LOH");

            // Получаем информацию о памяти
            var memoryInfo = GC.GetGCMemoryInfo();
            Console.WriteLine($"Общая память: {memoryInfo.TotalAvailableMemoryBytes / 1024 / 1024} MB");
            Console.WriteLine($"Память в куче: {memoryInfo.HeapSizeBytes / 1024 / 1024} MB");

            // Настройка дефрагментации LOH
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            Console.WriteLine("LOH будет дефрагментирован при следующей сборке");

            // Создаем закрепленный объект (POH)
            byte[] pinnedArray = new byte[1000];
            GCHandle handle = GCHandle.Alloc(pinnedArray, GCHandleType.Pinned);
            Console.WriteLine("Объект закреплен в памяти");

            // Освобождаем закрепленный объект
            handle.Free();
            Console.WriteLine("Объект освобожден");

            // Принудительная сборка мусора
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Получаем обновленную информацию о памяти
            memoryInfo = GC.GetGCMemoryInfo();
            Console.WriteLine($"Память после сборки: {memoryInfo.HeapSizeBytes / 1024 / 1024} MB");
        }
    }
} 