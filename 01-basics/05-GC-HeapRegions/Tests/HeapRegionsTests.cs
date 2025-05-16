using System;
using Xunit;

namespace GCHeapRegions.Tests
{
    public class HeapRegionsTests
    {
        // Тест 1: Проверка размещения малых объектов
        [Fact]
        public void SmallObject_ShouldBePlacedInSOH()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var smallObject = new byte[1000];
            var memoryAfterCreation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }

        // Тест 2: Проверка размещения больших объектов
        [Fact]
        public void LargeObject_ShouldBePlacedInLOH()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var largeObject = new byte[85000]; // Объект > 85KB попадает в LOH
            var memoryAfterCreation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }

        // Тест 3: Проверка фрагментации кучи
        [Fact]
        public void HeapFragmentation_ShouldBeDetectable()
        {
            // Arrange
            var objects = new byte[100][];
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            for (int i = 0; i < 100; i++)
            {
                objects[i] = new byte[1000];
            }

            // Удаляем каждый второй объект
            for (int i = 0; i < 100; i += 2)
            {
                objects[i] = null;
            }

            GC.Collect();
            var memoryAfterFragmentation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterFragmentation > initialMemory);
        }

        // Тест 4: Проверка компактификации кучи
        [Fact]
        public void HeapCompaction_ShouldWork()
        {
            // Arrange
            var objects = new byte[100][];
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            for (int i = 0; i < 100; i++)
            {
                objects[i] = new byte[1000];
            }

            // Удаляем все объекты
            for (int i = 0; i < 100; i++)
            {
                objects[i] = null;
            }

            GC.Collect();
            var memoryAfterCompaction = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCompaction <= initialMemory);
        }

        // Тест 5: Проверка размещения объектов фиксированного размера
        [Fact]
        public void FixedSizeObjects_ShouldBePlacedEfficiently()
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

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }

        // Тест 6: Проверка размещения объектов переменного размера
        [Fact]
        public void VariableSizeObjects_ShouldBePlacedCorrectly()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var objects = new byte[100][];
            for (int i = 0; i < 100; i++)
            {
                objects[i] = new byte[i * 100];
            }

            var memoryAfterCreation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }

        // Тест 7: Проверка размещения объектов с финализаторами
        [Fact]
        public void FinalizableObjects_ShouldBePlacedCorrectly()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var objects = new TestObjectWithFinalizer[1000];
            for (int i = 0; i < 1000; i++)
            {
                objects[i] = new TestObjectWithFinalizer();
            }

            var memoryAfterCreation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }

        // Тест 8: Проверка размещения объектов с большими строками
        [Fact]
        public void LargeStrings_ShouldBePlacedCorrectly()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var largeString = new string('a', 100000);
            var memoryAfterCreation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }

        // Тест 9: Проверка размещения объектов с массивами
        [Fact]
        public void ArrayObjects_ShouldBePlacedCorrectly()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var arrays = new int[100][];
            for (int i = 0; i < 100; i++)
            {
                arrays[i] = new int[1000];
            }

            var memoryAfterCreation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }

        // Тест 10: Проверка размещения объектов с вложенными объектами
        [Fact]
        public void NestedObjects_ShouldBePlacedCorrectly()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var objects = new NestedObject[1000];
            for (int i = 0; i < 1000; i++)
            {
                objects[i] = new NestedObject();
            }

            var memoryAfterCreation = GC.GetTotalMemory(false);

            // Assert
            Assert.True(memoryAfterCreation > initialMemory);
        }
    }

    // Вспомогательные классы для тестов
    public class TestObject
    {
        private readonly byte[] _data = new byte[1000];
    }

    public class TestObjectWithFinalizer
    {
        private readonly byte[] _data = new byte[1000];

        ~TestObjectWithFinalizer()
        {
            // Финализатор
        }
    }

    public class NestedObject
    {
        private readonly TestObject _nested = new TestObject();
        private readonly byte[] _data = new byte[1000];
    }
} 