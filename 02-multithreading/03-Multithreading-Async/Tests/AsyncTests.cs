using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Async.Tests
{
    public class AsyncTests
    {
        // Тест 1: Проверка базового async/await
        [Fact]
        public async Task BasicAsyncAwait_ShouldWorkCorrectly()
        {
            // Arrange
            async Task<int> GetValueAsync()
            {
                await Task.Delay(100);
                return 42;
            }

            // Act
            var result = await GetValueAsync();

            // Assert
            Assert.Equal(42, result);
        }

        // Тест 2: Проверка параллельного выполнения задач
        [Fact]
        public async Task ParallelExecution_ShouldWorkCorrectly()
        {
            // Arrange
            async Task<int> GetValueAsync(int value)
            {
                await Task.Delay(100);
                return value;
            }

            // Act
            var task1 = GetValueAsync(1);
            var task2 = GetValueAsync(2);
            var task3 = GetValueAsync(3);

            var results = await Task.WhenAll(task1, task2, task3);

            // Assert
            Assert.Equal(new[] { 1, 2, 3 }, results);
        }

        // Тест 3: Проверка отмены задачи
        [Fact]
        public async Task TaskCancellation_ShouldWorkCorrectly()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(100);

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await Task.Delay(1000, cts.Token);
            });
        }

        // Тест 4: Проверка обработки исключений
        [Fact]
        public async Task ExceptionHandling_ShouldWorkCorrectly()
        {
            // Arrange
            async Task<int> ThrowExceptionAsync()
            {
                await Task.Delay(100);
                throw new InvalidOperationException("Test exception");
            }

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await ThrowExceptionAsync();
            });
        }

        // Тест 5: Проверка Task.WhenAny
        [Fact]
        public async Task WhenAny_ShouldWorkCorrectly()
        {
            // Arrange
            async Task<int> GetValueAsync(int value, int delay)
            {
                await Task.Delay(delay);
                return value;
            }

            // Act
            var task1 = GetValueAsync(1, 200);
            var task2 = GetValueAsync(2, 100);
            var task3 = GetValueAsync(3, 300);

            var completedTask = await Task.WhenAny(task1, task2, task3);
            var result = await completedTask;

            // Assert
            Assert.Equal(2, result);
        }

        // Тест 6: Проверка ConfigureAwait
        [Fact]
        public async Task ConfigureAwait_ShouldWorkCorrectly()
        {
            // Arrange
            var context = SynchronizationContext.Current;

            // Act
            await Task.Delay(100).ConfigureAwait(false);
            var newContext = SynchronizationContext.Current;

            // Assert
            Assert.NotEqual(context, newContext);
        }

        // Тест 7: Проверка ValueTask
        [Fact]
        public async Task ValueTask_ShouldWorkCorrectly()
        {
            // Arrange
            async ValueTask<int> GetValueAsync()
            {
                await Task.Delay(100);
                return 42;
            }

            // Act
            var result = await GetValueAsync();

            // Assert
            Assert.Equal(42, result);
        }

        // Тест 8: Проверка TaskCompletionSource
        [Fact]
        public async Task TaskCompletionSource_ShouldWorkCorrectly()
        {
            // Arrange
            var tcs = new TaskCompletionSource<int>();

            // Act
            var task = tcs.Task;
            tcs.SetResult(42);
            var result = await task;

            // Assert
            Assert.Equal(42, result);
        }

        // Тест 9: Проверка Task.Run
        [Fact]
        public async Task TaskRun_ShouldWorkCorrectly()
        {
            // Arrange
            int GetValue()
            {
                Thread.Sleep(100);
                return 42;
            }

            // Act
            var result = await Task.Run(GetValue);

            // Assert
            Assert.Equal(42, result);
        }

        // Тест 10: Проверка асинхронного потока
        [Fact]
        public async Task AsyncStream_ShouldWorkCorrectly()
        {
            // Arrange
            async IAsyncEnumerable<int> GetValuesAsync()
            {
                for (int i = 0; i < 3; i++)
                {
                    await Task.Delay(100);
                    yield return i;
                }
            }

            // Act
            var values = new List<int>();
            await foreach (var value in GetValuesAsync())
            {
                values.Add(value);
            }

            // Assert
            Assert.Equal(new[] { 0, 1, 2 }, values);
        }

        // Тест 11: Проверка асинхронного метода с использованием lock
        [Fact]
        public async Task AsyncWithLock_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;
            var lockObj = new object();

            async Task IncrementAsync()
            {
                await Task.Delay(100);
                lock (lockObj)
                {
                    counter++;
                }
            }

            // Act
            var task1 = IncrementAsync();
            var task2 = IncrementAsync();
            await Task.WhenAll(task1, task2);

            // Assert
            Assert.Equal(2, counter);
        }

        // Тест 12: Проверка асинхронного метода с использованием SemaphoreSlim
        [Fact]
        public async Task AsyncWithSemaphoreSlim_ShouldWorkCorrectly()
        {
            // Arrange
            var counter = 0;
            using var semaphore = new SemaphoreSlim(1, 1);

            async Task IncrementAsync()
            {
                await Task.Delay(100);
                await semaphore.WaitAsync();
                try
                {
                    counter++;
                }
                finally
                {
                    semaphore.Release();
                }
            }

            // Act
            var task1 = IncrementAsync();
            var task2 = IncrementAsync();
            await Task.WhenAll(task1, task2);

            // Assert
            Assert.Equal(2, counter);
        }
    }
} 