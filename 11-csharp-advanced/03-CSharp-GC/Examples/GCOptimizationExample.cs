using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.GCOptimization
{
    public class GCOptimizationExample
    {
        private readonly List<byte[]> _largeObjects = new();
        private readonly Stopwatch _stopwatch = new();

        public void DemonstrateMemoryPressure()
        {
            Console.WriteLine("Демонстрация давления на GC\n");

            _stopwatch.Start();

            // Создание большого количества объектов
            for (int i = 0; i < 1000; i++)
            {
                var largeObject = new byte[85000]; // Объект > 85KB попадает в LOH
                _largeObjects.Add(largeObject);
            }

            _stopwatch.Stop();
            Console.WriteLine($"Создание объектов заняло: {_stopwatch.ElapsedMilliseconds} мс");
            Console.WriteLine($"Текущее поколение GC: {GC.GetGeneration(_largeObjects)}");
        }

        public void DemonstrateObjectPooling()
        {
            Console.WriteLine("\nДемонстрация пула объектов\n");

            var pool = new ObjectPool<ExpensiveObject>(() => new ExpensiveObject());

            _stopwatch.Restart();

            // Использование пула объектов
            for (int i = 0; i < 1000; i++)
            {
                using (var obj = pool.Get())
                {
                    obj.DoWork();
                }
            }

            _stopwatch.Stop();
            Console.WriteLine($"Работа с пулом объектов заняла: {_stopwatch.ElapsedMilliseconds} мс");
        }

        public void DemonstrateResourceManagement()
        {
            Console.WriteLine("\nДемонстрация управления ресурсами\n");

            // Использование using для автоматического освобождения ресурсов
            using (var resource = new ManagedResource())
            {
                resource.DoWork();
            }

            // Принудительный вызов сборщика мусора (не рекомендуется в production)
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void DemonstrateWeakReferences()
        {
            Console.WriteLine("\nДемонстрация слабых ссылок\n");

            var weakRef = new WeakReference(new byte[1000]);

            // Проверка, жив ли объект
            if (weakRef.IsAlive)
            {
                Console.WriteLine("Объект все еще жив");
            }

            // Принудительная сборка мусора
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Проверка после сборки мусора
            if (!weakRef.IsAlive)
            {
                Console.WriteLine("Объект был собран");
            }
        }

        public void DemonstrateGCCollection()
        {
            Console.WriteLine("\nДемонстрация сборки мусора\n");

            // Получение информации о памяти
            Console.WriteLine($"Общий объем памяти: {GC.GetTotalMemory(false) / 1024} KB");
            Console.WriteLine($"Максимальное поколение: {GC.MaxGeneration}");

            // Принудительная сборка мусора
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Общий объем памяти после сборки: {GC.GetTotalMemory(false) / 1024} KB");
        }
    }

    // Пример класса для пула объектов
    public class ExpensiveObject : IDisposable
    {
        private readonly byte[] _data;

        public ExpensiveObject()
        {
            _data = new byte[1000];
        }

        public void DoWork()
        {
            // Имитация работы
            Thread.Sleep(1);
        }

        public void Dispose()
        {
            // Очистка ресурсов
        }
    }

    // Пример класса с управляемыми ресурсами
    public class ManagedResource : IDisposable
    {
        private bool _disposed;

        public void DoWork()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ManagedResource));

            // Имитация работы
            Thread.Sleep(100);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Освобождение управляемых ресурсов
                }

                // Освобождение неуправляемых ресурсов
                _disposed = true;
            }
        }

        ~ManagedResource()
        {
            Dispose(false);
        }
    }

    // Простой пул объектов
    public class ObjectPool<T> where T : class
    {
        private readonly Stack<T> _pool;
        private readonly Func<T> _factory;

        public ObjectPool(Func<T> factory)
        {
            _pool = new Stack<T>();
            _factory = factory;
        }

        public T Get()
        {
            lock (_pool)
            {
                return _pool.Count > 0 ? _pool.Pop() : _factory();
            }
        }

        public void Return(T item)
        {
            lock (_pool)
            {
                _pool.Push(item);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var example = new GCOptimizationExample();

            // Демонстрация различных аспектов оптимизации GC
            example.DemonstrateMemoryPressure();
            example.DemonstrateObjectPooling();
            example.DemonstrateResourceManagement();
            example.DemonstrateWeakReferences();
            example.DemonstrateGCCollection();

            Console.WriteLine("\nНажмите Enter для выхода");
            Console.ReadLine();
        }
    }
} 