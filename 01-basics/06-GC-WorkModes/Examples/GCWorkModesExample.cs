using System;
using System.Runtime;

namespace GCWorkModesExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Server GC: {GCSettings.IsServerGC}");
            Console.WriteLine($"Latency Mode: {GCSettings.LatencyMode}");

            // Изменение режима задержки
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            Console.WriteLine($"Latency Mode после изменения: {GCSettings.LatencyMode}");

            // Принудительный запуск сборки мусора
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Возврат к стандартному режиму
            GCSettings.LatencyMode = GCLatencyMode.Interactive;
            Console.WriteLine($"Latency Mode после возврата: {GCSettings.LatencyMode}");
        }
    }
} 