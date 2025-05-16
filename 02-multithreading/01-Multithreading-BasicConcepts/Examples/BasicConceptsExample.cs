using System;
using System.Threading;
using System.Threading.Tasks;

namespace BasicConceptsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Пример 1: Простой поток
            Thread thread = new Thread(() =>
            {
                Console.WriteLine("Поток запущен");
                Thread.Sleep(1000);
                Console.WriteLine("Поток завершен");
            });
            thread.Start();
            thread.Join();

            // Пример 2: ThreadPool
            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine("Задача в ThreadPool запущена");
                Thread.Sleep(1000);
                Console.WriteLine("Задача в ThreadPool завершена");
            });

            // Пример 3: Task
            Task<int> task = Task.Run(() =>
            {
                Console.WriteLine("Task запущен");
                Thread.Sleep(1000);
                return 42;
            });

            Console.WriteLine($"Результат Task: {task.Result}");

            // Пример 4: Проблема гонки данных
            int counter = 0;
            var tasks = new Task[10];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        counter++; // Потенциальная гонка данных
                    }
                });
            }

            Task.WaitAll(tasks);
            Console.WriteLine($"Ожидаемое значение: 10000, Фактическое: {counter}");
        }
    }
} 