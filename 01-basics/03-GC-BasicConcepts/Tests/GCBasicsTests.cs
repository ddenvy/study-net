using System;
using Xunit;

namespace GCBasics.Tests
{
    public class GCBasicsTests
    {
        // Тест 1: Проверка создания и удаления объектов
        [Fact]
        public void ObjectCreationAndCollection_ShouldWorkCorrectly()
        {
            // Arrange
            WeakReference weakRef;

            // Act
            {
                var obj = new TestObject();
                weakRef = new WeakReference(obj);
                obj = null;
            }

            // Принудительная сборка мусора
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.False(weakRef.IsAlive);
        }

        // Тест 2: Проверка финализатора
        [Fact]
        public void Finalizer_ShouldBeCalled()
        {
            // Arrange
            TestObjectWithFinalizer.ResetCounter();

            // Act
            {
                var obj = new TestObjectWithFinalizer();
                obj = null;
            }

            // Принудительная сборка мусора
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.Equal(1, TestObjectWithFinalizer.FinalizerCallCount);
        }

        // Тест 3: Проверка использования памяти
        [Fact]
        public void MemoryUsage_ShouldBeMeasurable()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var objects = new TestObject[1000];
            for (int i = 0; i < 1000; i++)
            {
                objects[i] = new TestObject();
            }

            var memoryAfterCreation = GC.GetTotalMemory(false);
            objects = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var memoryAfterCollection = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
            Assert.True(memoryAfterCollection < memoryAfterCreation);
        }

        // Тест 4: Проверка поколений объектов
        [Fact]
        public void ObjectGenerations_ShouldWorkCorrectly()
        {
            // Arrange
            var obj = new TestObject();

            // Act
            var generation = GC.GetGeneration(obj);

            // Assert
            Assert.Equal(0, generation);
        }

        // Тест 5: Проверка принудительной сборки мусора
        [Fact]
        public void ForcedCollection_ShouldWorkCorrectly()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var objects = new TestObject[1000];
            for (int i = 0; i < 1000; i++)
            {
                objects[i] = new TestObject();
            }

            var memoryBeforeCollection = GC.GetTotalMemory(false);
            objects = null;
            GC.Collect(0, GCCollectionMode.Forced);
            var memoryAfterCollection = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCollection < memoryBeforeCollection);
        }

        // Тест 6: Проверка утечки памяти
        [Fact]
        public void MemoryLeak_ShouldBeDetectable()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);
            var objects = new TestObject[1000];

            // Act
            for (int i = 0; i < 1000; i++)
            {
                objects[i] = new TestObject();
            }

            var memoryAfterCreation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }

        // Тест 7: Проверка работы с большими объектами
        [Fact]
        public void LargeObject_ShouldBeHandledCorrectly()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var largeObject = new byte[85000]; // Объект > 85KB попадает в LOH
            var memoryAfterCreation = GC.GetTotalMemory(false);
            largeObject = null;
            GC.Collect();
            var memoryAfterCollection = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
            Assert.True(memoryAfterCollection < memoryAfterCreation);
        }

        // Тест 8: Проверка слабых ссылок
        [Fact]
        public void WeakReference_ShouldWorkCorrectly()
        {
            // Arrange
            var obj = new TestObject();
            var weakRef = new WeakReference(obj);

            // Act
            obj = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.False(weakRef.IsAlive);
        }

        // Тест 9: Проверка финализаторов и Dispose
        [Fact]
        public void FinalizerAndDispose_ShouldWorkCorrectly()
        {
            // Arrange
            TestObjectWithDispose.ResetCounter();

            // Act
            {
                using (var obj = new TestObjectWithDispose())
                {
                    obj.DoWork();
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.Equal(1, TestObjectWithDispose.DisposeCallCount);
            Assert.Equal(0, TestObjectWithDispose.FinalizerCallCount);
        }

        // Тест 10: Проверка давления на GC
        [Fact]
        public void GCPressure_ShouldBeMeasurable()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            for (int i = 0; i < 1000; i++)
            {
                var obj = new TestObject();
                obj = null;
            }

            GC.Collect();
            var memoryAfterCollection = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCollection >= initialMemory);
        }
    }

    // Вспомогательные классы для тестов
    public class TestObject
    {
        private readonly byte[] _data = new byte[1000];
    }

    public class TestObjectWithFinalizer
    {
        private static int _finalizerCallCount;
        public static int FinalizerCallCount => _finalizerCallCount;

        public static void ResetCounter()
        {
            _finalizerCallCount = 0;
        }

        ~TestObjectWithFinalizer()
        {
            _finalizerCallCount++;
        }
    }

    public class TestObjectWithDispose : IDisposable
    {
        private static int _disposeCallCount;
        private static int _finalizerCallCount;
        private bool _disposed;

        public static int DisposeCallCount => _disposeCallCount;
        public static int FinalizerCallCount => _finalizerCallCount;

        public static void ResetCounter()
        {
            _disposeCallCount = 0;
            _finalizerCallCount = 0;
        }

        public void DoWork()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TestObjectWithDispose));
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
                    _disposeCallCount++;
                }
                _disposed = true;
            }
        }

        ~TestObjectWithDispose()
        {
            Dispose(false);
            _finalizerCallCount++;
        }
    }
} 