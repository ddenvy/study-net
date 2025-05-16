using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CSharp.Compilation
{
    // Атрибуты сборки
    [assembly: AssemblyTitle("CompilationExample")]
    [assembly: AssemblyDescription("Пример компиляции и выполнения")]
    [assembly: AssemblyConfiguration("")]
    [assembly: AssemblyCompany("Example Company")]
    [assembly: AssemblyProduct("CompilationExample")]
    [assembly: AssemblyCopyright("Copyright © 2024")]
    [assembly: AssemblyTrademark("")]
    [assembly: AssemblyCulture("")]
    [assembly: ComVisible(false)]
    [assembly: Guid("12345678-1234-1234-1234-123456789012")]
    [assembly: AssemblyVersion("1.0.0.0")]
    [assembly: AssemblyFileVersion("1.0.0.0")]

    // Пример класса с атрибутами
    [Serializable]
    public class CompilationExample
    {
        // Поле с атрибутом
        [Obsolete("Этот метод устарел")]
        public string OldMethod()
        {
            return "Старый метод";
        }

        // Метод с атрибутом
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int FastMethod(int x, int y)
        {
            return x + y;
        }

        // Метод с условной компиляцией
        public void ConditionalMethod()
        {
#if DEBUG
            Console.WriteLine("Отладочная версия");
#else
            Console.WriteLine("Релизная версия");
#endif
        }

        // Метод с проверкой условий
        public void CheckEnvironment()
        {
            // Проверка платформы
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("Запущено на Windows");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine("Запущено на Linux");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.WriteLine("Запущено на macOS");
            }

            // Проверка архитектуры
            Console.WriteLine($"Архитектура: {RuntimeInformation.ProcessArchitecture}");
            Console.WriteLine($"Версия .NET: {Environment.Version}");
        }
    }

    // Пример класса с ресурсами
    public class ResourceExample
    {
        // Встроенный ресурс
        private const string EmbeddedResource = "Встроенный ресурс";

        public void DemonstrateResources()
        {
            // Работа с файловыми ресурсами
            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, "Тестовые данные");
                Console.WriteLine($"Создан временный файл: {tempFile}");

                // Чтение файла
                string content = File.ReadAllText(tempFile);
                Console.WriteLine($"Содержимое файла: {content}");
            }
            finally
            {
                // Очистка ресурсов
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                    Console.WriteLine("Временный файл удален");
                }
            }
        }
    }

    // Пример класса с оптимизациями
    public class OptimizationExample
    {
        // Константа времени компиляции
        private const int MaxValue = 100;

        // Статическое поле
        private static readonly int[] StaticArray = new int[MaxValue];

        // Метод с оптимизациями
        public void DemonstrateOptimizations()
        {
            // Использование stackalloc для выделения памяти в стеке
            Span<int> stackArray = stackalloc int[10];
            for (int i = 0; i < stackArray.Length; i++)
            {
                stackArray[i] = i;
            }

            // Использование ReadOnlySpan для работы с неизменяемыми данными
            ReadOnlySpan<char> text = "Тестовый текст";
            Console.WriteLine($"Длина текста: {text.Length}");

            // Использование ValueTuple для эффективной работы с кортежами
            var (x, y) = (10, 20);
            Console.WriteLine($"x = {x}, y = {y}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрация компиляции и выполнения\n");

            // Демонстрация атрибутов и условной компиляции
            var compilationExample = new CompilationExample();
            compilationExample.ConditionalMethod();
            compilationExample.CheckEnvironment();

            // Демонстрация работы с ресурсами
            var resourceExample = new ResourceExample();
            resourceExample.DemonstrateResources();

            // Демонстрация оптимизаций
            var optimizationExample = new OptimizationExample();
            optimizationExample.DemonstrateOptimizations();

            // Вывод информации о сборке
            var assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine($"\nИнформация о сборке:");
            Console.WriteLine($"Название: {assembly.GetName().Name}");
            Console.WriteLine($"Версия: {assembly.GetName().Version}");
            Console.WriteLine($"Расположение: {assembly.Location}");

            // Пример 6: Динамическая загрузка сборки
            try
            {
                Assembly assemblyToLoad = Assembly.LoadFrom("Example.dll");
                Type typeToLoad = assemblyToLoad.GetType("Example.Class");
                object instance = Activator.CreateInstance(typeToLoad);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки сборки: {ex.Message}");
            }

            // Пример 7: Работа с ресурсами
            using (var resourceStream = new System.IO.StreamReader("config.json"))
            {
                string content = resourceStream.ReadToEnd();
                Console.WriteLine($"Содержимое файла config.json: {content}");
            }

            // Пример 8: Обработка исключений
            try
            {
                // Код, который может вызвать исключение
                throw new Exception("Тестовое исключение");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Обработка исключения: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Блок finally выполнен");
            }

            Console.WriteLine("\nНажмите Enter для выхода");
            Console.ReadLine();
        }
    }
} 