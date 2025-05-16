using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SyncPrimitives.Tests
{
    public class SyncPrimitivesTests
    {
        // Тест 1: Проверка lock
        [Fact]
        public void Lock_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;
            var lockObj = new object();

            // Act
            var thread1 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    lock (lockObj)
                    {
                        counter++;
                    }
                }
            });

            var thread2 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    lock (lockObj)
                    {
                        counter++;
                    }
                }
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Assert
            Assert.Equal(2000, counter);
        }

        // Тест 2: Проверка Monitor
        [Fact]
        public void Monitor_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;
            var lockObj = new object();

            // Act
            var thread1 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Monitor.Enter(lockObj);
                    try
                    {
                        counter++;
                    }
                    finally
                    {
                        Monitor.Exit(lockObj);
                    }
                }
            });

            var thread2 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Monitor.Enter(lockObj);
                    try
                    {
                        counter++;
                    }
                    finally
                    {
                        Monitor.Exit(lockObj);
                    }
                }
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Assert
            Assert.Equal(2000, counter);
        }

        // Тест 3: Проверка Mutex
        [Fact]
        public void Mutex_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;
            using var mutex = new Mutex();

            // Act
            var thread1 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    mutex.WaitOne();
                    try
                    {
                        counter++;
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            });

            var thread2 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    mutex.WaitOne();
                    try
                    {
                        counter++;
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Assert
            Assert.Equal(2000, counter);
        }

        // Тест 4: Проверка Semaphore
        [Fact]
        public void Semaphore_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;
            using var semaphore = new Semaphore(2, 2);

            // Act
            var thread1 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    semaphore.WaitOne();
                    try
                    {
                        counter++;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }
            });

            var thread2 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    semaphore.WaitOne();
                    try
                    {
                        counter++;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Assert
            Assert.Equal(2000, counter);
        }

        // Тест 5: Проверка AutoResetEvent
        [Fact]
        public void AutoResetEvent_ShouldWorkCorrectly()
        {
            // Arrange
            var completed = false;
            using var autoResetEvent = new AutoResetEvent(false);

            // Act
            var thread = new Thread(() =>
            {
                autoResetEvent.WaitOne();
                completed = true;
            });
            thread.Start();
            autoResetEvent.Set();
            thread.Join();

            // Assert
            Assert.True(completed);
        }

        // Тест 6: Проверка ManualResetEvent
        [Fact]
        public void ManualResetEvent_ShouldWorkCorrectly()
        {
            // Arrange
            var completed = false;
            using var manualResetEvent = new ManualResetEvent(false);

            // Act
            var thread = new Thread(() =>
            {
                manualResetEvent.WaitOne();
                completed = true;
            });
            thread.Start();
            manualResetEvent.Set();
            thread.Join();

            // Assert
            Assert.True(completed);
        }

        // Тест 7: Проверка ReaderWriterLock
        [Fact]
        public void ReaderWriterLock_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;
            var rwLock = new ReaderWriterLock();

            // Act
            var reader1 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    rwLock.AcquireReaderLock(Timeout.Infinite);
                    try
                    {
                        var value = counter;
                    }
                    finally
                    {
                        rwLock.ReleaseReaderLock();
                    }
                }
            });

            var writer = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    rwLock.AcquireWriterLock(Timeout.Infinite);
                    try
                    {
                        counter++;
                    }
                    finally
                    {
                        rwLock.ReleaseWriterLock();
                    }
                }
            });

            reader1.Start();
            writer.Start();
            reader1.Join();
            writer.Join();

            // Assert
            Assert.Equal(1000, counter);
        }

        // Тест 8: Проверка Interlocked
        [Fact]
        public void Interlocked_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;

            // Act
            var thread1 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Interlocked.Increment(ref counter);
                }
            });

            var thread2 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Interlocked.Increment(ref counter);
                }
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Assert
            Assert.Equal(2000, counter);
        }

        // Тест 9: Проверка Barrier
        [Fact]
        public void Barrier_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;
            using var barrier = new Barrier(2);

            // Act
            var thread1 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Interlocked.Increment(ref counter);
                    barrier.SignalAndWait();
                }
            });

            var thread2 = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    barrier.SignalAndWait();
                    Assert.Equal(i + 1, counter);
                }
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Assert
            Assert.Equal(1000, counter);
        }

        // Тест 10: Проверка CountdownEvent
        [Fact]
        public void CountdownEvent_ShouldWorkCorrectly()
        {
            // Arrange
            var completed = false;
            using var countdownEvent = new CountdownEvent(2);

            // Act
            var thread1 = new Thread(() =>
            {
                Thread.Sleep(100);
                countdownEvent.Signal();
            });

            var thread2 = new Thread(() =>
            {
                Thread.Sleep(100);
                countdownEvent.Signal();
            });

            thread1.Start();
            thread2.Start();
            countdownEvent.Wait();
            completed = true;

            // Assert
            Assert.True(completed);
        }
    }
} 