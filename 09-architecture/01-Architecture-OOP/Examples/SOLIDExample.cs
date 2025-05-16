using System;
using System.Collections.Generic;

namespace SOLIDPrinciples
{
    // Single Responsibility Principle (SRP)
    // Каждый класс должен иметь только одну причину для изменения
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    // Разделение ответственности: хранение и валидация
    public class UserRepository
    {
        private List<User> _users = new List<User>();

        public void Add(User user)
        {
            _users.Add(user);
        }

        public User GetById(int id)
        {
            return _users.Find(u => u.Id == id);
        }
    }

    public class UserValidator
    {
        public bool ValidateEmail(string email)
        {
            return email.Contains("@");
        }
    }

    // Open/Closed Principle (OCP)
    // Классы должны быть открыты для расширения, но закрыты для модификации
    public abstract class Shape
    {
        public abstract double CalculateArea();
    }

    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public override double CalculateArea()
        {
            return Width * Height;
        }
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }

        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
    }

    // Liskov Substitution Principle (LSP)
    // Подтипы должны быть заменяемы своими базовыми типами
    public class Bird
    {
        public virtual void Fly()
        {
            Console.WriteLine("Птица летит");
        }
    }

    public class Penguin : Bird
    {
        public override void Fly()
        {
            throw new NotImplementedException("Пингвины не летают!");
        }
    }

    // Interface Segregation Principle (ISP)
    // Клиенты не должны зависеть от интерфейсов, которые они не используют
    public interface IPrinter
    {
        void Print();
    }

    public interface IScanner
    {
        void Scan();
    }

    public interface IFax
    {
        void Fax();
    }

    public class SimplePrinter : IPrinter
    {
        public void Print()
        {
            Console.WriteLine("Печать документа");
        }
    }

    public class AllInOnePrinter : IPrinter, IScanner, IFax
    {
        public void Print()
        {
            Console.WriteLine("Печать документа");
        }

        public void Scan()
        {
            Console.WriteLine("Сканирование документа");
        }

        public void Fax()
        {
            Console.WriteLine("Отправка факса");
        }
    }

    // Dependency Inversion Principle (DIP)
    // Зависимости должны строиться относительно абстракций, а не деталей
    public interface ILogger
    {
        void Log(string message);
    }

    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"Запись в файл: {message}");
        }
    }

    public class DatabaseLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"Запись в базу данных: {message}");
        }
    }

    public class UserService
    {
        private readonly ILogger _logger;

        public UserService(ILogger logger)
        {
            _logger = logger;
        }

        public void CreateUser(string name)
        {
            _logger.Log($"Создан пользователь: {name}");
        }
    }

    // Пример использования
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрация принципов SOLID\n");

            // SRP
            Console.WriteLine("Single Responsibility Principle:");
            var user = new User { Id = 1, Name = "John", Email = "john@example.com" };
            var validator = new UserValidator();
            var repository = new UserRepository();

            if (validator.ValidateEmail(user.Email))
            {
                repository.Add(user);
                Console.WriteLine("Пользователь добавлен");
            }

            // OCP
            Console.WriteLine("\nOpen/Closed Principle:");
            var shapes = new List<Shape>
            {
                new Rectangle { Width = 5, Height = 3 },
                new Circle { Radius = 2 }
            };

            foreach (var shape in shapes)
            {
                Console.WriteLine($"Площадь фигуры: {shape.CalculateArea()}");
            }

            // LSP
            Console.WriteLine("\nLiskov Substitution Principle:");
            var birds = new List<Bird>
            {
                new Bird(),
                new Penguin()
            };

            foreach (var bird in birds)
            {
                try
                {
                    bird.Fly();
                }
                catch (NotImplementedException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            // ISP
            Console.WriteLine("\nInterface Segregation Principle:");
            var simplePrinter = new SimplePrinter();
            var allInOnePrinter = new AllInOnePrinter();

            simplePrinter.Print();
            allInOnePrinter.Print();
            allInOnePrinter.Scan();
            allInOnePrinter.Fax();

            // DIP
            Console.WriteLine("\nDependency Inversion Principle:");
            ILogger logger = new FileLogger();
            var userService = new UserService(logger);
            userService.CreateUser("Alice");

            logger = new DatabaseLogger();
            userService = new UserService(logger);
            userService.CreateUser("Bob");
        }
    }
} 