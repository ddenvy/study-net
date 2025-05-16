using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Multithreading.Tests
{
    public class MultithreadingTests
    {
        // Тест 1: Проверка создания и запуска потока
        [Fact]
        public void ThreadCreation_ShouldWorkCorrectly()
        {
            // Arrange
            var threadCompleted = false;

            // Act
            var thread = new Thread(() =>
            {
                Thread.Sleep(100);
                threadCompleted = true;
            });
            thread.Start();
            thread.Join();

            // Assert
            Assert.True(threadCompleted);
        }

        // Тест 2: Проверка передачи параметров в поток
        [Fact]
        public void ThreadParameterPassing_ShouldWorkCorrectly()
        {
            // Arrange
            var result = 0;

            // Act
            var thread = new Thread((obj) =>
            {
                result = (int)obj;
            });
            thread.Start(42);
            thread.Join();

            // Assert
            Assert.Equal(42, result);
        }

        // Тест 3: Проверка приоритетов потока
        [Fact]
        public void ThreadPriority_ShouldWorkCorrectly()
        {
            // Arrange
            var thread = new Thread(() => { });

            // Act
            thread.Priority = ThreadPriority.Highest;

            // Assert
            Assert.Equal(ThreadPriority.Highest, thread.Priority);
        }

        // Тест 4: Проверка состояния потока
        [Fact]
        public void ThreadState_ShouldWorkCorrectly()
        {
            // Arrange
            var thread = new Thread(() => { });

            // Act & Assert
            Assert.Equal(ThreadState.Unstarted, thread.ThreadState);
            thread.Start();
            thread.Join();
            Assert.Equal(ThreadState.Stopped, thread.ThreadState);
        }

        // Тест 5: Проверка пула потоков
        [Fact]
        public void ThreadPool_ShouldWorkCorrectly()
        {
            // Arrange
            var completed = false;

            // Act
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(100);
                completed = true;
            });
            Thread.Sleep(200);

            // Assert
            Assert.True(completed);
        }

        // Тест 6: Проверка Task
        [Fact]
        public async Task Task_ShouldWorkCorrectly()
        {
            // Arrange
            var task = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 42;
            });

            // Act
            var result = await task;

            // Assert
            Assert.Equal(42, result);
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        // Тест 7: Проверка параллельного выполнения
        [Fact]
        public void ParallelExecution_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;
            var tasks = new Task[10];

            // Act
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    Interlocked.Increment(ref counter);
                });
            }
            Task.WaitAll(tasks);

            // Assert
            Assert.Equal(10, counter);
        }

        // Тест 8: Проверка отмены задачи
        [Fact]
        public async Task TaskCancellation_ShouldWorkCorrectly()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                await Task.Delay(1000, cts.Token);
            }, cts.Token);

            // Act
            cts.Cancel();

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => task);
        }

        // Тест 9: Проверка продолжений задачи
        [Fact]
        public async Task TaskContinuation_ShouldWorkCorrectly()
        {
            // Arrange
            var result = 0;

            // Act
            await Task.Run(() => 42)
                .ContinueWith(t => result = t.Result * 2);

            // Assert
            Assert.Equal(84, result);
        }

        // Тест 10: Проверка обработки исключений в задачах
        [Fact]
        public async Task TaskExceptionHandling_ShouldWorkCorrectly()
        {
            // Arrange
            var task = Task.Run(() =>
            {
                throw new Exception("Test exception");
            });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => task);
            Assert.Equal(TaskStatus.Faulted, task.Status);
        }

        // Тест 11: Проверка синхронизации потоков
        [Fact]
        public void ThreadSynchronization_ShouldWorkCorrectly()
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

        // Тест 12: Проверка ожидания завершения нескольких задач
        [Fact]
        public async Task TaskWhenAll_ShouldWorkCorrectly()
        {
            // Arrange
            var tasks = new Task<int>[3];

            // Act
            for (int i = 0; i < 3; i++)
            {
                tasks[i] = Task.Run(() => 42);
            }

            var results = await Task.WhenAll(tasks);

            // Assert
            Assert.All(results, result => Assert.Equal(42, result));
        }
    }
} 