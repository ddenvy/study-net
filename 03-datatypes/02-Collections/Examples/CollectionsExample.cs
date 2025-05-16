using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace CollectionsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Пример 1: List<T>
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            numbers.Add(6);
            numbers.Remove(3);
            Console.WriteLine($"List: {string.Join(", ", numbers)}");

            // Пример 2: Dictionary<TKey, TValue>
            var dict = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
                ["three"] = 3
            };
            dict["four"] = 4;
            Console.WriteLine($"Dictionary: {string.Join(", ", dict.Select(kv => $"{kv.Key}={kv.Value}"))}");

            // Пример 3: Queue<T>
            var queue = new Queue<string>();
            queue.Enqueue("First");
            queue.Enqueue("Second");
            queue.Enqueue("Third");
            Console.WriteLine($"Queue: {queue.Dequeue()}");

            // Пример 4: Stack<T>
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            Console.WriteLine($"Stack: {stack.Pop()}");

            // Пример 5: HashSet<T>
            var set = new HashSet<int> { 1, 2, 3, 4, 5 };
            set.Add(6);
            set.Remove(3);
            Console.WriteLine($"HashSet: {string.Join(", ", set)}");

            // Пример 6: ConcurrentDictionary
            var concurrentDict = new ConcurrentDictionary<string, int>();
            concurrentDict.TryAdd("one", 1);
            concurrentDict.TryAdd("two", 2);
            Console.WriteLine($"ConcurrentDictionary: {string.Join(", ", concurrentDict.Select(kv => $"{kv.Key}={kv.Value}"))}");

            // Пример 7: LINQ с коллекциями
            var filtered = numbers.Where(n => n % 2 == 0)
                                .OrderByDescending(n => n)
                                .ToList();
            Console.WriteLine($"Filtered: {string.Join(", ", filtered)}");

            // Пример 8: Группировка
            var grouped = numbers.GroupBy(n => n % 2 == 0 ? "Even" : "Odd")
                               .ToDictionary(g => g.Key, g => g.ToList());
            foreach (var group in grouped)
            {
                Console.WriteLine($"{group.Key}: {string.Join(", ", group.Value)}");
            }
        }
    }
} 