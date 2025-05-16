using System;
using System.Runtime.InteropServices;
using Xunit;

namespace MemoryManagement.Tests
{
    public class MemoryManagementTests
    {
        // Тест 1: Проверка выделения неуправляемой памяти
        [Fact]
        public void AllocateUnmanagedMemory_ShouldSucceed()
        {
            // Arrange
            IntPtr ptr = IntPtr.Zero;

            // Act
            ptr = Marshal.AllocHGlobal(1024);

            // Assert
            Assert.NotEqual(IntPtr.Zero, ptr);
            Marshal.FreeHGlobal(ptr);
        }

        // Тест 2: Проверка освобождения неуправляемой памяти
        [Fact]
        public void FreeUnmanagedMemory_ShouldSucceed()
        {
            // Arrange
            var ptr = Marshal.AllocHGlobal(1024);

            // Act
            Marshal.FreeHGlobal(ptr);

            // Assert
            // Если освобождение прошло успешно, тест не выбросит исключение
        }

        // Тест 3: Проверка работы с GCHandle
        [Fact]
        public void GCHandle_ShouldPinObject()
        {
            // Arrange
            var obj = new byte[1000];

            // Act
            var handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            var ptr = handle.AddrOfPinnedObject();

            // Assert
            Assert.NotEqual(IntPtr.Zero, ptr);
            handle.Free();
        }

        // Тест 4: Проверка работы с Memory<T>
        [Fact]
        public void Memory_ShouldWorkWithSpan()
        {
            // Arrange
            var array = new byte[1000];
            var memory = new Memory<byte>(array);

            // Act
            var span = memory.Span;
            span[0] = 42;

            // Assert
            Assert.Equal(42, array[0]);
        }

        // Тест 5: Проверка работы с NativeMemory
        [Fact]
        public void NativeMemory_ShouldAllocate()
        {
            // Arrange
            void* ptr = null;

            // Act
            ptr = (void*)NativeMemory.Alloc(1024);

            // Assert
            Assert.True(ptr != null);
            NativeMemory.Free(ptr);
        }

        // Тест 6: Проверка работы с MemoryPool
        [Fact]
        public void MemoryPool_ShouldRentAndReturn()
        {
            // Arrange
            using var pool = MemoryPool<byte>.Shared;

            // Act
            using var memory = pool.Rent(1024);

            // Assert
            Assert.True(memory.Memory.Length >= 1024);
        }

        // Тест 7: Проверка работы с ArrayPool
        [Fact]
        public void ArrayPool_ShouldRentAndReturn()
        {
            // Arrange
            var pool = ArrayPool<byte>.Shared;

            // Act
            var array = pool.Rent(1024);

            // Assert
            Assert.True(array.Length >= 1024);
            pool.Return(array);
        }

        // Тест 8: Проверка работы с MemoryMarshal
        [Fact]
        public void MemoryMarshal_ShouldCastMemory()
        {
            // Arrange
            var array = new int[1000];
            var memory = new Memory<int>(array);

            // Act
            var byteMemory = MemoryMarshal.AsBytes(memory);

            // Assert
            Assert.Equal(array.Length * sizeof(int), byteMemory.Length);
        }

        // Тест 9: Проверка работы с MemoryHandle
        [Fact]
        public void MemoryHandle_ShouldPinMemory()
        {
            // Arrange
            var array = new byte[1000];
            var memory = new Memory<byte>(array);

            // Act
            using var handle = memory.Pin();

            // Assert
            Assert.NotEqual(IntPtr.Zero, handle.Pointer);
        }

        // Тест 10: Проверка работы с MemoryManager
        [Fact]
        public void MemoryManager_ShouldManageMemory()
        {
            // Arrange
            var manager = new TestMemoryManager<byte>(1000);

            // Act
            var memory = manager.Memory;

            // Assert
            Assert.Equal(1000, memory.Length);
        }

        // Тест 11: Проверка работы с GC
        [Fact]
        public void GC_ShouldCollectMemory()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);
            var array = new byte[1000000];

            // Act
            array = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var finalMemory = GC.GetTotalMemory(false);

            // Assert
            Assert.True(finalMemory < initialMemory + 1000000);
        }

        // Тест 12: Проверка работы с WeakReference
        [Fact]
        public void WeakReference_ShouldTrackObject()
        {
            // Arrange
            var obj = new byte[1000];
            var weakRef = new WeakReference(obj);

            // Act
            obj = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.False(weakRef.IsAlive);
        }
    }

    // Вспомогательные классы для тестов
    public class TestMemoryManager<T> : MemoryManager<T>
    {
        private readonly T[] _array;

        public TestMemoryManager(int length)
        {
            _array = new T[length];
        }

        public override Span<T> GetSpan()
        {
            return _array;
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            return new MemoryHandle(_array, elementIndex);
        }

        public override void Unpin()
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
} 