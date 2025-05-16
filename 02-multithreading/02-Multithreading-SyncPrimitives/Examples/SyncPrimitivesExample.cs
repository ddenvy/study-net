using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncPrimitivesExample
{
    class Program
    {
        private static readonly object _lock = new object();
        private static readonly Semaphore _semaphore = new Semaphore(2, 2);
        private static readonly AutoResetEvent _event = new AutoResetEvent(false);
        private static readonly ReaderWriterLock _rwLock = new ReaderWriterLock();
        private static int _counter = 0;

        static void Main(string[] args)
        {
            // Пример 1: Использование lock
            Parallel.For(0, 10, i =>
            {
                lock (_lock)
                {
                    _counter++;
                    Console.WriteLine($"Counter: {_counter}");
                }
            });

            // Пример 2: Использование Semaphore
            for (int i = 0; i < 5; i++)
            {
                Task.Run(() =>
                {
                    _semaphore.WaitOne();
                    Console.WriteLine($"Поток {Task.CurrentId} получил доступ");
                    Thread.Sleep(1000);
                    Console.WriteLine($"Поток {Task.CurrentId} освобождает доступ");
                    _semaphore.Release();
                });
            }

            // Пример 3: Использование AutoResetEvent
            Task.Run(() =>
            {
                Console.WriteLine("Ожидание сигнала...");
                _event.WaitOne();
                Console.WriteLine("Получен сигнал!");
            });

            Thread.Sleep(1000);
            _event.Set();

            // Пример 4: Использование ReaderWriterLock
            Parallel.Invoke(
                () => ReadData(),
                () => ReadData(),
                () => WriteData()
            );

            Thread.Sleep(2000);
        }

        static void ReadData()
        {
            _rwLock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                Console.WriteLine($"Чтение данных потоком {Task.CurrentId}");
                Thread.Sleep(500);
            }
            finally
            {
                _rwLock.ReleaseReaderLock();
            }
        }

        static void WriteData()
        {
            _rwLock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                Console.WriteLine($"Запись данных потоком {Task.CurrentId}");
                Thread.Sleep(1000);
            }
            finally
            {
                _rwLock.ReleaseWriterLock();
            }
        }
    }
} 