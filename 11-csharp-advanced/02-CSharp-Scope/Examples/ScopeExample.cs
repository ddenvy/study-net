using System;
using System.Threading;

namespace CSharp.Scope
{
    // Пример класса с различными модификаторами доступа
    public class AccessModifiersExample
    {
        // Публичное поле
        public string PublicField = "Публичное поле";

        // Приватное поле
        private string _privateField = "Приватное поле";

        // Защищенное поле
        protected string ProtectedField = "Защищенное поле";

        // Внутреннее поле
        internal string InternalField = "Внутреннее поле";

        // Защищенное внутреннее поле
        protected internal string ProtectedInternalField = "Защищенное внутреннее поле";

        // Приватное защищенное поле
        private protected string PrivateProtectedField = "Приватное защищенное поле";

        // Публичный метод
        public void PublicMethod()
        {
            Console.WriteLine("Публичный метод");
            PrivateMethod(); // Доступ к приватному методу внутри класса
        }

        // Приватный метод
        private void PrivateMethod()
        {
            Console.WriteLine("Приватный метод");
        }

        // Защищенный метод
        protected void ProtectedMethod()
        {
            Console.WriteLine("Защищенный метод");
        }
    }

    // Наследник класса с модификаторами доступа
    public class DerivedClass : AccessModifiersExample
    {
        public void DemonstrateInheritance()
        {
            // Доступ к защищенным членам базового класса
            Console.WriteLine(ProtectedField);
            ProtectedMethod();

            // Доступ к защищенным внутренним членам
            Console.WriteLine(ProtectedInternalField);

            // Доступ к приватным защищенным членам
            Console.WriteLine(PrivateProtectedField);
        }
    }

    // Пример класса с управлением ресурсами
    public class ResourceManager : IDisposable
    {
        private bool _disposed;
        private readonly string _resourceName;

        public ResourceManager(string resourceName)
        {
            _resourceName = resourceName;
            Console.WriteLine($"Создан ресурс: {_resourceName}");
        }

        public void UseResource()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ResourceManager));

            Console.WriteLine($"Использование ресурса: {_resourceName}");
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
                    // Освобождение управляемых ресурсов
                    Console.WriteLine($"Освобождение управляемых ресурсов: {_resourceName}");
                }

                // Освобождение неуправляемых ресурсов
                Console.WriteLine($"Освобождение неуправляемых ресурсов: {_resourceName}");
                _disposed = true;
            }
        }

        ~ResourceManager()
        {
            Dispose(false);
        }
    }

    // Пример класса с областями видимости
    public class ScopeExample
    {
        // Статическое поле - глобальная область видимости
        private static int _globalCounter;

        // Поле класса
        private readonly string _instanceField;

        public ScopeExample(string instanceField)
        {
            _instanceField = instanceField;
        }

        public void DemonstrateScopes()
        {
            // Локальная переменная метода
            int localVar = 10;

            // Блок с собственной областью видимости
            {
                int blockVar = 20;
                Console.WriteLine($"Переменная блока: {blockVar}");
                Console.WriteLine($"Локальная переменная: {localVar}");
                Console.WriteLine($"Поле экземпляра: {_instanceField}");
                Console.WriteLine($"Глобальный счетчик: {_globalCounter}");
            }

            // Попытка доступа к переменной блока вызовет ошибку компиляции
            // Console.WriteLine(blockVar);
        }

        public void DemonstrateLifetime()
        {
            // Создание объекта с использованием using
            using (var resource = new ResourceManager("Временный ресурс"))
            {
                resource.UseResource();
            } // Ресурс автоматически освобождается

            // Создание объекта без using
            var resource2 = new ResourceManager("Постоянный ресурс");
            try
            {
                resource2.UseResource();
            }
            finally
            {
                resource2.Dispose();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрация областей видимости и времени жизни\n");

            // Демонстрация модификаторов доступа
            var accessExample = new AccessModifiersExample();
            accessExample.PublicMethod();
            Console.WriteLine(accessExample.PublicField);
            // Следующие строки вызовут ошибки компиляции:
            // accessExample.PrivateMethod();
            // Console.WriteLine(accessExample._privateField);

            // Демонстрация наследования
            var derived = new DerivedClass();
            derived.DemonstrateInheritance();

            // Демонстрация областей видимости
            var scopeExample = new ScopeExample("Тестовое поле");
            scopeExample.DemonstrateScopes();

            // Демонстрация времени жизни
            scopeExample.DemonstrateLifetime();

            Console.WriteLine("\nНажмите Enter для выхода");
            Console.ReadLine();
        }
    }
} 