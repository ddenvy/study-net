using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CSharp.ThreadSafeCollections
{
    public class ThreadSafeCollectionsExample
    {
        // ConcurrentDictionary - потокобезопасный словарь
        private readonly ConcurrentDictionary<string, int> _cache = new();
        
        // ConcurrentQueue - потокобезопасная очередь
        private readonly ConcurrentQueue<string> _messageQueue = new();
        
        // BlockingCollection - блокирующая коллекция
        private readonly BlockingCollection<string> _processingQueue = new();
        
        // ConcurrentBag - потокобезопасный мешок
        private readonly ConcurrentBag<int> _numbers = new();

        public void DemonstrateConcurrentDictionary()
        {
            Console.WriteLine("Демонстрация ConcurrentDictionary\n");

            // Добавление элементов
            _cache.TryAdd("key1", 1);
            _cache.TryAdd("key2", 2);

            // Атомарное обновление
            _cache.AddOrUpdate("key1", 3, (key, oldValue) => oldValue + 1);

            // Получение или добавление
            var value = _cache.GetOrAdd("key3", 3);

            // Вывод всех элементов
            foreach (var item in _cache)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }
        }

        public void DemonstrateConcurrentQueue()
        {
            Console.WriteLine("\nДемонстрация ConcurrentQueue\n");

            // Добавление элементов
            _messageQueue.Enqueue("Сообщение 1");
            _messageQueue.Enqueue("Сообщение 2");

            // Извлечение элементов
            if (_messageQueue.TryDequeue(out string message))
            {
                Console.WriteLine($"Извлечено сообщение: {message}");
            }

            // Вывод оставшихся элементов
            foreach (var item in _messageQueue)
            {
                Console.WriteLine($"Оставшееся сообщение: {item}");
            }
        }

        public void DemonstrateBlockingCollection()
        {
            Console.WriteLine("\nДемонстрация BlockingCollection\n");

            // Запуск потребителя
            var consumer = Task.Run(() =>
            {
                foreach (var item in _processingQueue.GetConsumingEnumerable())
                {
                    Console.WriteLine($"Обработка: {item}");
                    Thread.Sleep(100); // Имитация обработки
                }
            });

            // Добавление элементов
            _processingQueue.Add("Задача 1");
            _processingQueue.Add("Задача 2");
            _processingQueue.Add("Задача 3");

            // Завершение добавления
            _processingQueue.CompleteAdding();

            // Ожидание завершения потребителя
            consumer.Wait();
        }

        public void DemonstrateConcurrentBag()
        {
            Console.WriteLine("\nДемонстрация ConcurrentBag\n");

            // Параллельное добавление элементов
            Parallel.For(0, 10, i =>
            {
                _numbers.Add(i);
            });

            // Вывод всех элементов
            Console.WriteLine("Элементы в ConcurrentBag:");
            foreach (var number in _numbers)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
        }

        public void DemonstrateProducerConsumer()
        {
            Console.WriteLine("\nДемонстрация паттерна Producer-Consumer\n");

            var queue = new BlockingCollection<int>();
            var producers = new List<Task>();
            var consumers = new List<Task>();

            // Создание производителей
            for (int i = 0; i < 3; i++)
            {
                producers.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        var item = j;
                        queue.Add(item);
                        Console.WriteLine($"Производитель {Task.CurrentId} добавил: {item}");
                        Thread.Sleep(100);
                    }
                }));
            }

            // Создание потребителей
            for (int i = 0; i < 2; i++)
            {
                consumers.Add(Task.Run(() =>
                {
                    foreach (var item in queue.GetConsumingEnumerable())
                    {
                        Console.WriteLine($"Потребитель {Task.CurrentId} обработал: {item}");
                        Thread.Sleep(200);
                    }
                }));
            }

            // Ожидание завершения производителей
            Task.WaitAll(producers.ToArray());
            queue.CompleteAdding();

            // Ожидание завершения потребителей
            Task.WaitAll(consumers.ToArray());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var example = new ThreadSafeCollectionsExample();

            // Демонстрация различных коллекций
            example.DemonstrateConcurrentDictionary();
            example.DemonstrateConcurrentQueue();
            example.DemonstrateBlockingCollection();
            example.DemonstrateConcurrentBag();
            example.DemonstrateProducerConsumer();

            Console.WriteLine("\nНажмите Enter для выхода");
            Console.ReadLine();
        }
    }
} 