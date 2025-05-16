using System;
using System.IO;

namespace UnmanagedResourceExample
{
    // Класс с неуправляемым ресурсом (например, файловый поток)
    public class FileWriter : IDisposable
    {
        private FileStream _stream;
        private bool _disposed = false;

        public FileWriter(string path)
        {
            _stream = new FileStream(path, FileMode.Create);
        }

        public void Write(byte[] data)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(FileWriter));
            _stream.Write(data, 0, data.Length);
        }

        // Реализация Dispose
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
                    // Освобождаем управляемые ресурсы
                    _stream?.Dispose();
                }
                // Здесь можно освободить неуправляемые ресурсы
                _disposed = true;
            }
        }

        // Финализатор
        ~FileWriter()
        {
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Использование блока using
            using (var writer = new FileWriter("test.txt"))
            {
                writer.Write(new byte[] { 1, 2, 3, 4, 5 });
            }
            Console.WriteLine("Файл записан и ресурс освобожден");
        }
    }
} 