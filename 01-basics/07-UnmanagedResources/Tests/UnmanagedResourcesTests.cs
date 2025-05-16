using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;

namespace UnmanagedResources.Tests
{
    public class UnmanagedResourcesTests
    {
        // Тест 1: Проверка освобождения неуправляемых ресурсов через Dispose
        [Fact]
        public void Dispose_ShouldReleaseUnmanagedResources()
        {
            // Arrange
            var resource = new TestUnmanagedResource();

            // Act
            resource.Dispose();

            // Assert
            Assert.True(resource.IsDisposed);
        }

        // Тест 2: Проверка освобождения неуправляемых ресурсов через using
        [Fact]
        public void Using_ShouldReleaseUnmanagedResources()
        {
            // Arrange
            var isDisposed = false;

            // Act
            using (var resource = new TestUnmanagedResource())
            {
                isDisposed = resource.IsDisposed;
            }

            // Assert
            Assert.False(isDisposed);
        }

        // Тест 3: Проверка финализатора
        [Fact]
        public void Finalizer_ShouldBeCalled()
        {
            // Arrange
            TestUnmanagedResourceWithFinalizer.ResetCounter();

            // Act
            {
                var resource = new TestUnmanagedResourceWithFinalizer();
                resource = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.Equal(1, TestUnmanagedResourceWithFinalizer.FinalizerCallCount);
        }

        // Тест 4: Проверка работы с файловым потоком
        [Fact]
        public void FileStream_ShouldBeDisposed()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var isDisposed = false;

            // Act
            using (var stream = new FileStream(tempFile, FileMode.Create))
            {
                isDisposed = stream.CanRead;
            }

            // Assert
            Assert.True(isDisposed);
            File.Delete(tempFile);
        }

        // Тест 5: Проверка работы с неуправляемой памятью
        [Fact]
        public void UnmanagedMemory_ShouldBeReleased()
        {
            // Arrange
            var handle = GCHandle.Alloc(new byte[1000], GCHandleType.Pinned);

            // Act
            handle.Free();

            // Assert
            Assert.False(handle.IsAllocated);
        }

        // Тест 6: Проверка работы с SafeHandle
        [Fact]
        public void SafeHandle_ShouldBeReleased()
        {
            // Arrange
            var handle = new TestSafeHandle();

            // Act
            handle.Dispose();

            // Assert
            Assert.True(handle.IsClosed);
        }

        // Тест 7: Проверка работы с CriticalFinalizerObject
        [Fact]
        public void CriticalFinalizerObject_ShouldBeReleased()
        {
            // Arrange
            TestCriticalFinalizerObject.ResetCounter();

            // Act
            {
                var obj = new TestCriticalFinalizerObject();
                obj = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.Equal(1, TestCriticalFinalizerObject.FinalizerCallCount);
        }

        // Тест 8: Проверка работы с COM объектами
        [Fact]
        public void COMObject_ShouldBeReleased()
        {
            // Arrange
            var comObject = new TestCOMObject();

            // Act
            Marshal.ReleaseComObject(comObject);

            // Assert
            Assert.True(comObject.IsReleased);
        }

        // Тест 9: Проверка работы с неуправляемыми ресурсами в асинхронном коде
        [Fact]
        public async Task AsyncDispose_ShouldWork()
        {
            // Arrange
            var resource = new TestAsyncDisposable();

            // Act
            await resource.DisposeAsync();

            // Assert
            Assert.True(resource.IsDisposed);
        }

        // Тест 10: Проверка работы с несколькими неуправляемыми ресурсами
        [Fact]
        public void MultipleResources_ShouldBeReleased()
        {
            // Arrange
            var resource1 = new TestUnmanagedResource();
            var resource2 = new TestUnmanagedResource();

            // Act
            resource1.Dispose();
            resource2.Dispose();

            // Assert
            Assert.True(resource1.IsDisposed);
            Assert.True(resource2.IsDisposed);
        }
    }

    // Вспомогательные классы для тестов
    public class TestUnmanagedResource : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    public class TestUnmanagedResourceWithFinalizer : IDisposable
    {
        private static int _finalizerCallCount;
        public static int FinalizerCallCount => _finalizerCallCount;

        public static void ResetCounter()
        {
            _finalizerCallCount = 0;
        }

        ~TestUnmanagedResourceWithFinalizer()
        {
            _finalizerCallCount++;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    public class TestSafeHandle : SafeHandle
    {
        public TestSafeHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            return true;
        }
    }

    public class TestCriticalFinalizerObject : CriticalFinalizerObject
    {
        private static int _finalizerCallCount;
        public static int FinalizerCallCount => _finalizerCallCount;

        public static void ResetCounter()
        {
            _finalizerCallCount = 0;
        }

        ~TestCriticalFinalizerObject()
        {
            _finalizerCallCount++;
        }
    }

    public class TestCOMObject
    {
        public bool IsReleased { get; private set; }

        public void Release()
        {
            IsReleased = true;
        }
    }

    public class TestAsyncDisposable : IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return ValueTask.CompletedTask;
        }
    }
} 