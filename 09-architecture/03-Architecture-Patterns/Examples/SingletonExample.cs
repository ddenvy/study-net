using System;
using System.Threading;

namespace SingletonPattern
{
    // Потокобезопасная реализация Singleton
    public sealed class Singleton
    {
        private static Singleton _instance;
        private static readonly object _lock = new object();
        private static int _instanceCount = 0;

        private Singleton()
        {
            _instanceCount++;
            Console.WriteLine($"Создан экземпляр Singleton #{_instanceCount}");
        }

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Singleton();
                        }
                    }
                }
                return _instance;
            }
        }

        public void DoSomething()
        {
            Console.WriteLine("Выполняется метод DoSomething()");
        }
    }

    // Пример использования
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрация паттерна Singleton\n");

            // Создание первого экземпляра
            Console.WriteLine("Создание первого экземпляра:");
            Singleton instance1 = Singleton.Instance;
            instance1.DoSomething();

            // Попытка создать второй экземпляр
            Console.WriteLine("\nПопытка создать второй экземпляр:");
            Singleton instance2 = Singleton.Instance;
            instance2.DoSomething();

            // Проверка, что это один и тот же экземпляр
            Console.WriteLine("\nПроверка идентичности экземпляров:");
            Console.WriteLine($"instance1 == instance2: {ReferenceEquals(instance1, instance2)}");

            // Демонстрация потокобезопасности
            Console.WriteLine("\nДемонстрация потокобезопасности:");
            Thread[] threads = new Thread[5];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    Singleton instance = Singleton.Instance;
                    instance.DoSomething();
                });
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
} 