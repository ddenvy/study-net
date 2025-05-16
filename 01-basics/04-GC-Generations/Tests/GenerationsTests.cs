using System;
using Xunit;

namespace GCGenerations.Tests
{
    public class GenerationsTests
    {
        // Тест 1: Проверка начального поколения объекта
        [Fact]
        public void InitialGeneration_ShouldBeZero()
        {
            // Arrange
            var obj = new TestObject();

            // Act
            var generation = GC.GetGeneration(obj);

            // Assert
            Assert.Equal(0, generation);
        }

        // Тест 2: Проверка перехода в следующее поколение
        [Fact]
        public void Object_ShouldSurviveToNextGeneration()
        {
            // Arrange
            var obj = new TestObject();

            // Act
            GC.Collect(0);
            var generation = GC.GetGeneration(obj);

            // Assert
            Assert.Equal(1, generation);
        }

        // Тест 3: Проверка максимального поколения
        [Fact]
        public void MaxGeneration_ShouldBeTwo()
        {
            // Act
            var maxGeneration = GC.MaxGeneration;

            // Assert
            Assert.Equal(2, maxGeneration);
        }

        // Тест 4: Проверка выживания объекта после сборки мусора
        [Fact]
        public void Object_ShouldSurviveGC()
        {
            // Arrange
            var obj = new TestObject();
            var weakRef = new WeakReference(obj);

            // Act
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.True(weakRef.IsAlive);
        }

        // Тест 5: Проверка сборки мусора в определенном поколении
        [Fact]
        public void Collection_ShouldWorkForSpecificGeneration()
        {
            // Arrange
            var obj = new TestObject();
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            GC.Collect(0);
            var memoryAfterCollection = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCollection <= initialMemory);
        }

        // Тест 6: Проверка принудительной сборки мусора
        [Fact]
        public void ForcedCollection_ShouldWork()
        {
            // Arrange
            var obj = new TestObject();
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            GC.Collect(0, GCCollectionMode.Forced);
            var memoryAfterCollection = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCollection <= initialMemory);
        }

        // Тест 7: Проверка оптимизированной сборки мусора
        [Fact]
        public void OptimizedCollection_ShouldWork()
        {
            // Arrange
            var obj = new TestObject();
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            GC.Collect(0, GCCollectionMode.Optimized);
            var memoryAfterCollection = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCollection <= initialMemory);
        }

        // Тест 8: Проверка блокировки сборки мусора
        [Fact]
        public void Collection_ShouldBeBlocked()
        {
            // Arrange
            var obj = new TestObject();

            // Act
            GC.TryStartNoGCRegion(1000);
            var canStart = GC.TryStartNoGCRegion(1000);
            GC.EndNoGCRegion();

            // Assert
            Assert.False(canStart);
        }

        // Тест 9: Проверка давления на GC
        [Fact]
        public void GCPressure_ShouldBeMeasurable()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            for (int i = 0; i < 1000; i++)
            {
                var obj = new TestObject();
            }
            var memoryAfterCreation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }

        // Тест 10: Проверка финализаторов в разных поколениях
        [Fact]
        public void Finalizers_ShouldWorkInDifferentGenerations()
        {
            // Arrange
            TestObjectWithFinalizer.ResetCounter();

            // Act
            {
                var obj = new TestObjectWithFinalizer();
                GC.Collect(0);
                GC.Collect(1);
                obj = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.Equal(1, TestObjectWithFinalizer.FinalizerCallCount);
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
} 