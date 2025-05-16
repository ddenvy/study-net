using System;
using System.Collections.Generic;

namespace OOPPrinciples
{
    // Абстракция
    public abstract class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public abstract void MakeSound();
        public virtual void Sleep()
        {
            Console.WriteLine($"{Name} спит");
        }
    }

    // Наследование
    public class Dog : Animal
    {
        public string Breed { get; set; }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} лает: Гав-гав!");
        }

        public void Fetch()
        {
            Console.WriteLine($"{Name} приносит мяч");
        }
    }

    public class Cat : Animal
    {
        public bool IsIndoor { get; set; }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} мяукает: Мяу-мяу!");
        }

        public void Climb()
        {
            Console.WriteLine($"{Name} забирается на дерево");
        }
    }

    // Инкапсуляция
    public class BankAccount
    {
        private decimal _balance;
        private readonly string _accountNumber;

        public BankAccount(string accountNumber, decimal initialBalance)
        {
            _accountNumber = accountNumber;
            _balance = initialBalance;
        }

        public decimal GetBalance()
        {
            return _balance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма должна быть положительной");

            _balance += amount;
            Console.WriteLine($"Внесено {amount:C}. Новый баланс: {_balance:C}");
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма должна быть положительной");

            if (amount > _balance)
                throw new InvalidOperationException("Недостаточно средств");

            _balance -= amount;
            Console.WriteLine($"Снято {amount:C}. Новый баланс: {_balance:C}");
        }
    }

    // Полиморфизм
    public class Zoo
    {
        private List<Animal> _animals = new List<Animal>();

        public void AddAnimal(Animal animal)
        {
            _animals.Add(animal);
        }

        public void MakeAllAnimalsSound()
        {
            foreach (var animal in _animals)
            {
                animal.MakeSound();
            }
        }
    }

    // Пример использования
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрация принципов ООП\n");

            // Абстракция и Наследование
            Console.WriteLine("Абстракция и Наследование:");
            var dog = new Dog { Name = "Бобик", Age = 3, Breed = "Лабрадор" };
            var cat = new Cat { Name = "Мурка", Age = 2, IsIndoor = true };

            dog.MakeSound();
            dog.Sleep();
            dog.Fetch();

            cat.MakeSound();
            cat.Sleep();
            cat.Climb();

            // Инкапсуляция
            Console.WriteLine("\nИнкапсуляция:");
            var account = new BankAccount("1234567890", 1000);
            Console.WriteLine($"Начальный баланс: {account.GetBalance():C}");

            try
            {
                account.Deposit(500);
                account.Withdraw(200);
                account.Withdraw(2000); // Вызовет исключение
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            // Полиморфизм
            Console.WriteLine("\nПолиморфизм:");
            var zoo = new Zoo();
            zoo.AddAnimal(dog);
            zoo.AddAnimal(cat);

            Console.WriteLine("Все животные издают звуки:");
            zoo.MakeAllAnimalsSound();
        }
    }
} 