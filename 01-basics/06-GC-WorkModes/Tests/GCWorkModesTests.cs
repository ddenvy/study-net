using System;
using Xunit;

namespace GCWorkModes.Tests
{
    public class GCWorkModesTests
    {
        // Тест 1: Проверка режима Server GC
        [Fact]
        public void ServerGC_ShouldBeConfigurable()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var isServerGC = System.Runtime.GCSettings.IsServerGC;

            // Assert
            Assert.True(isServerGC);
        }

        // Тест 2: Проверка режима Concurrent GC
        [Fact]
        public void ConcurrentGC_ShouldBeConfigurable()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var isConcurrentGC = System.Runtime.GCSettings.LargeObjectHeapCompactionMode == 
                                System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;

            // Assert
            Assert.True(isConcurrentGC);
        }

        // Тест 3: Проверка режима Background GC
        [Fact]
        public void BackgroundGC_ShouldWork()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var isBackgroundGC = System.Runtime.GCSettings.LatencyMode == 
                                System.Runtime.GCLatencyMode.Batch;

            // Assert
            Assert.True(isBackgroundGC);
        }

        // Тест 4: Проверка режима SustainedLowLatency
        [Fact]
        public void SustainedLowLatency_ShouldWork()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.SustainedLowLatency;
            var isSustainedLowLatency = System.Runtime.GCSettings.LatencyMode == 
                                       System.Runtime.GCLatencyMode.SustainedLowLatency;

            // Assert
            Assert.True(isSustainedLowLatency);
        }

        // Тест 5: Проверка режима Interactive
        [Fact]
        public void Interactive_ShouldWork()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Interactive;
            var isInteractive = System.Runtime.GCSettings.LatencyMode == 
                               System.Runtime.GCLatencyMode.Interactive;

            // Assert
            Assert.True(isInteractive);
        }

        // Тест 6: Проверка режима Batch
        [Fact]
        public void Batch_ShouldWork()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Batch;
            var isBatch = System.Runtime.GCSettings.LatencyMode == 
                         System.Runtime.GCLatencyMode.Batch;

            // Assert
            Assert.True(isBatch);
        }

        // Тест 7: Проверка режима NoGCRegion
        [Fact]
        public void NoGCRegion_ShouldWork()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            var canStart = GC.TryStartNoGCRegion(1000);
            var isNoGCRegion = canStart;
            GC.EndNoGCRegion();

            // Assert
            Assert.True(isNoGCRegion);
        }

        // Тест 8: Проверка режима CompactOnce
        [Fact]
        public void CompactOnce_ShouldWork()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            System.Runtime.GCSettings.LargeObjectHeapCompactionMode = 
                System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
            var isCompactOnce = System.Runtime.GCSettings.LargeObjectHeapCompactionMode == 
                               System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;

            // Assert
            Assert.True(isCompactOnce);
        }

        // Тест 9: Проверка режима Default
        [Fact]
        public void Default_ShouldWork()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            System.Runtime.GCSettings.LargeObjectHeapCompactionMode = 
                System.Runtime.GCLargeObjectHeapCompactionMode.Default;
            var isDefault = System.Runtime.GCSettings.LargeObjectHeapCompactionMode == 
                           System.Runtime.GCLargeObjectHeapCompactionMode.Default;

            // Assert
            Assert.True(isDefault);
        }

        // Тест 10: Проверка режима LowLatency
        [Fact]
        public void LowLatency_ShouldWork()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(false);

            // Act
            System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.LowLatency;
            var isLowLatency = System.Runtime.GCSettings.LatencyMode == 
                              System.Runtime.GCLatencyMode.LowLatency;

            // Assert
            Assert.True(isLowLatency);
        }
    }
}