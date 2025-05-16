using System;
using static System.Math;

namespace OtherModifiersExample
{
    // Статический класс
    public static class MathUtils
    {
        public const double PI = 3.14159;
        public static readonly double E = 2.71828;

        public static double CalculateCircleArea(double radius)
        {
            return PI * Pow(radius, 2);
        }
    }

    // Абстрактный класс
    public abstract class Shape
    {
        protected readonly string Name;

        protected Shape(string name)
        {
            Name = name;
        }

        public abstract double CalculateArea();
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Shape: {Name}");
        }
    }

    // Наследник абстрактного класса
    public class Circle : Shape
    {
        private readonly double _radius;

        public Circle(double radius) : base("Circle")
        {
            _radius = radius;
        }

        public override double CalculateArea()
        {
            return MathUtils.CalculateCircleArea(_radius);
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Radius: {_radius}");
            Console.WriteLine($"Area: {CalculateArea()}");
        }
    }

    // Запечатанный класс
    public sealed class Square : Shape
    {
        private readonly double _side;

        public Square(double side) : base("Square")
        {
            _side = side;
        }

        public override double CalculateArea()
        {
            return Pow(_side, 2);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Использование статического класса
            Console.WriteLine($"PI: {MathUtils.PI}");
            Console.WriteLine($"Circle area: {MathUtils.CalculateCircleArea(5)}");

            // Использование абстрактного класса и его наследников
            Shape circle = new Circle(5);
            circle.DisplayInfo();

            Shape square = new Square(4);
            square.DisplayInfo();

            // Использование nameof
            Console.WriteLine($"Variable name: {nameof(circle)}");
            Console.WriteLine($"Property name: {nameof(Circle.CalculateArea)}");
        }
    }
}