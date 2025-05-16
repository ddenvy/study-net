using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IOStreams.Tests
{
    public class IOStreamsTests
    {
        // Тест 1: Проверка записи и чтения из FileStream
        [Fact]
        public void FileStream_ShouldWriteAndRead()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var testData = "Test data";

            // Act
            using (var stream = new FileStream(tempFile, FileMode.Create))
            {
                var bytes = Encoding.UTF8.GetBytes(testData);
                stream.Write(bytes, 0, bytes.Length);
            }

            string result;
            using (var stream = new FileStream(tempFile, FileMode.Open))
            {
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                result = Encoding.UTF8.GetString(bytes);
            }

            // Assert
            Assert.Equal(testData, result);
            File.Delete(tempFile);
        }

        // Тест 2: Проверка работы с MemoryStream
        [Fact]
        public void MemoryStream_ShouldWork()
        {
            // Arrange
            var testData = "Test data";
            var memoryStream = new MemoryStream();

            // Act
            var writer = new StreamWriter(memoryStream);
            writer.Write(testData);
            writer.Flush();

            memoryStream.Position = 0;
            var reader = new StreamReader(memoryStream);
            var result = reader.ReadToEnd();

            // Assert
            Assert.Equal(testData, result);
        }

        // Тест 3: Проверка работы с BufferedStream
        [Fact]
        public void BufferedStream_ShouldWork()
        {
            // Arrange
            var memoryStream = new MemoryStream();
            var bufferedStream = new BufferedStream(memoryStream);
            var testData = "Test data";

            // Act
            var writer = new StreamWriter(bufferedStream);
            writer.Write(testData);
            writer.Flush();

            bufferedStream.Position = 0;
            var reader = new StreamReader(bufferedStream);
            var result = reader.ReadToEnd();

            // Assert
            Assert.Equal(testData, result);
        }

        // Тест 4: Проверка работы с BinaryReader и BinaryWriter
        [Fact]
        public void BinaryStream_ShouldWork()
        {
            // Arrange
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            var testInt = 42;
            var testString = "Test";

            // Act
            writer.Write(testInt);
            writer.Write(testString);
            writer.Flush();

            memoryStream.Position = 0;
            var reader = new BinaryReader(memoryStream);
            var resultInt = reader.ReadInt32();
            var resultString = reader.ReadString();

            // Assert
            Assert.Equal(testInt, resultInt);
            Assert.Equal(testString, resultString);
        }

        // Тест 5: Проверка асинхронной работы с потоками
        [Fact]
        public async Task AsyncStream_ShouldWork()
        {
            // Arrange
            var memoryStream = new MemoryStream();
            var testData = "Test data";
            var bytes = Encoding.UTF8.GetBytes(testData);

            // Act
            await memoryStream.WriteAsync(bytes, 0, bytes.Length);
            memoryStream.Position = 0;
            var buffer = new byte[bytes.Length];
            await memoryStream.ReadAsync(buffer, 0, buffer.Length);
            var result = Encoding.UTF8.GetString(buffer);

            // Assert
            Assert.Equal(testData, result);
        }

        // Тест 6: Проверка работы с StreamReader и StreamWriter
        [Fact]
        public void TextStream_ShouldWork()
        {
            // Arrange
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            var testData = "Test data";

            // Act
            writer.WriteLine(testData);
            writer.Flush();

            memoryStream.Position = 0;
            var reader = new StreamReader(memoryStream);
            var result = reader.ReadLine();

            // Assert
            Assert.Equal(testData, result);
        }

        // Тест 7: Проверка работы с FileStream и блокировкой
        [Fact]
        public void FileStream_ShouldHandleLocking()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var testData = "Test data";

            // Act
            using (var stream1 = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var bytes = Encoding.UTF8.GetBytes(testData);
                stream1.Write(bytes, 0, bytes.Length);
            }

            string result;
            using (var stream2 = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var bytes = new byte[stream2.Length];
                stream2.Read(bytes, 0, bytes.Length);
                result = Encoding.UTF8.GetString(bytes);
            }

            // Assert
            Assert.Equal(testData, result);
            File.Delete(tempFile);
        }

        // Тест 8: Проверка работы с CryptoStream
        [Fact]
        public void CryptoStream_ShouldWork()
        {
            // Arrange
            var memoryStream = new MemoryStream();
            var testData = "Test data";

            // Act
            using (var cryptoStream = new CryptoStream(memoryStream, null, CryptoStreamMode.Write))
            {
                var writer = new StreamWriter(cryptoStream);
                writer.Write(testData);
                writer.Flush();
            }

            // Assert
            Assert.True(memoryStream.Length > 0);
        }

        // Тест 9: Проверка работы с NetworkStream
        [Fact]
        public void NetworkStream_ShouldWork()
        {
            // Arrange
            var memoryStream = new MemoryStream();
            var testData = "Test data";

            // Act
            using (var networkStream = new NetworkStream(memoryStream))
            {
                var writer = new StreamWriter(networkStream);
                writer.Write(testData);
                writer.Flush();
            }

            // Assert
            Assert.True(memoryStream.Length > 0);
        }

        // Тест 10: Проверка работы с DeflateStream
        [Fact]
        public void DeflateStream_ShouldCompressAndDecompress()
        {
            // Arrange
            var testData = "Test data";
            var compressedStream = new MemoryStream();

            // Act
            using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Compress))
            {
                var writer = new StreamWriter(deflateStream);
                writer.Write(testData);
                writer.Flush();
            }

            compressedStream.Position = 0;
            string result;
            using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                var reader = new StreamReader(deflateStream);
                result = reader.ReadToEnd();
            }

            // Assert
            Assert.Equal(testData, result);
        }
    }
} 