using System;
using System.Collections.Generic;
using Xunit;

namespace OOPTests
{
    public class OOPTests
    {
        [Fact]
        public void TestInheritance()
        {
            // Arrange
            var dog = new Dog { Name = "Бобик", Age = 3, Breed = "Лабрадор" };
            var cat = new Cat { Name = "Мурка", Age = 2, IsIndoor = true };

            // Act & Assert
            Assert.True(dog is Animal);
            Assert.True(cat is Animal);
            Assert.Equal("Лабрадор", dog.Breed);
            Assert.True(cat.IsIndoor);
        }

        [Fact]
        public void TestPolymorphism()
        {
            // Arrange
            var zoo = new Zoo();
            var dog = new Dog { Name = "Бобик" };
            var cat = new Cat { Name = "Мурка" };
            zoo.AddAnimal(dog);
            zoo.AddAnimal(cat);

            // Act
            var output = new System.IO.StringWriter();
            Console.SetOut(output);
            zoo.MakeAllAnimalsSound();

            // Assert
            var expectedOutput = "Бобик лает: Гав-гав!\r\nМурка мяукает: Мяу-мяу!\r\n";
            Assert.Equal(expectedOutput, output.ToString());
        }

        [Fact]
        public void TestEncapsulation()
        {
            // Arrange
            var account = new BankAccount("1234567890", 1000);

            // Act & Assert
            Assert.Equal(1000, account.GetBalance());
            Assert.Throws<ArgumentException>(() => account.Deposit(-100));
            Assert.Throws<InvalidOperationException>(() => account.Withdraw(2000));
        }
    }

    public class SOLIDTests
    {
        [Fact]
        public void TestSingleResponsibility()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John", Email = "john@example.com" };
            var validator = new UserValidator();
            var repository = new UserRepository();

            // Act & Assert
            Assert.True(validator.ValidateEmail(user.Email));
            repository.Add(user);
            Assert.Equal(user, repository.GetById(1));
        }

        [Fact]
        public void TestOpenClosed()
        {
            // Arrange
            var shapes = new List<Shape>
            {
                new Rectangle { Width = 5, Height = 3 },
                new Circle { Radius = 2 }
            };

            // Act & Assert
            Assert.Equal(15, shapes[0].CalculateArea());
            Assert.Equal(Math.PI * 4, shapes[1].CalculateArea());
        }

        [Fact]
        public void TestLiskovSubstitution()
        {
            // Arrange
            var birds = new List<Bird>
            {
                new Bird(),
                new Penguin()
            };

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => birds[1].Fly());
        }

        [Fact]
        public void TestInterfaceSegregation()
        {
            // Arrange
            var simplePrinter = new SimplePrinter();
            var allInOnePrinter = new AllInOnePrinter();

            // Act & Assert
            Assert.True(simplePrinter is IPrinter);
            Assert.True(allInOnePrinter is IPrinter);
            Assert.True(allInOnePrinter is IScanner);
            Assert.True(allInOnePrinter is IFax);
        }

        [Fact]
        public void TestDependencyInversion()
        {
            // Arrange
            ILogger logger = new FileLogger();
            var userService = new UserService(logger);

            // Act
            var output = new System.IO.StringWriter();
            Console.SetOut(output);
            userService.CreateUser("Alice");

            // Assert
            var expectedOutput = "Запись в файл: Создан пользователь: Alice\r\n";
            Assert.Equal(expectedOutput, output.ToString());
        }
    }
} 