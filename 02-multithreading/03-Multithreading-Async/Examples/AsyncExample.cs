using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Пример 1: Базовый асинхронный метод
            Console.WriteLine("Начало работы");
            await Task.Delay(1000);
            Console.WriteLine("Прошла 1 секунда");

            // Пример 2: Параллельное выполнение
            var task1 = SimulateWorkAsync("Задача 1", 2000);
            var task2 = SimulateWorkAsync("Задача 2", 1500);
            var task3 = SimulateWorkAsync("Задача 3", 1000);

            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine("Все задачи завершены");

            // Пример 3: Отмена операции
            using var cts = new CancellationTokenSource();
            var task = SimulateLongWorkAsync(cts.Token);
            
            // Отменяем операцию через 2 секунды
            await Task.Delay(2000);
            cts.Cancel();

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Операция отменена");
            }

            // Пример 4: Обработка исключений
            try
            {
                await SimulateErrorAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Поймано исключение: {ex.Message}");
            }
        }

        static async Task SimulateWorkAsync(string name, int delay)
        {
            Console.WriteLine($"{name} начата");
            await Task.Delay(delay);
            Console.WriteLine($"{name} завершена");
        }

        static async Task SimulateLongWorkAsync(CancellationToken token)
        {
            for (int i = 0; i < 10; i++)
            {
                token.ThrowIfCancellationRequested();
                Console.WriteLine($"Шаг {i + 1}");
                await Task.Delay(500, token);
            }
        }

        static async Task SimulateErrorAsync()
        {
            await Task.Delay(1000);
            throw new Exception("Тестовое исключение");
        }
    }
} 