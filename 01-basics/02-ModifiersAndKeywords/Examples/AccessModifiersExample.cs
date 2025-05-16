using System;

namespace AccessModifiersExample
{
    // Базовый класс с различными модификаторами доступа
    public class BaseClass
    {
        public string PublicField = "Public";
        private string PrivateField = "Private";
        protected string ProtectedField = "Protected";
        internal string InternalField = "Internal";
        protected internal string ProtectedInternalField = "Protected Internal";
        private protected string PrivateProtectedField = "Private Protected";

        public void PublicMethod()
        {
            Console.WriteLine("Public method");
            // Доступ ко всем полям внутри класса
            Console.WriteLine(PrivateField);
            Console.WriteLine(ProtectedField);
            Console.WriteLine(InternalField);
            Console.WriteLine(ProtectedInternalField);
            Console.WriteLine(PrivateProtectedField);
        }

        private void PrivateMethod()
        {
            Console.WriteLine("Private method");
        }

        protected void ProtectedMethod()
        {
            Console.WriteLine("Protected method");
        }

        internal void InternalMethod()
        {
            Console.WriteLine("Internal method");
        }
    }

    // Наследник базового класса
    public class DerivedClass : BaseClass
    {
        public void DemonstrateAccess()
        {
            // Доступ к public и protected членам
            Console.WriteLine(PublicField);
            Console.WriteLine(ProtectedField);
            Console.WriteLine(InternalField);
            Console.WriteLine(ProtectedInternalField);
            Console.WriteLine(PrivateProtectedField);

            // Вызов методов
            PublicMethod();
            ProtectedMethod();
            InternalMethod();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var derived = new DerivedClass();
            derived.DemonstrateAccess();

            // Доступ только к public и internal членам
            Console.WriteLine(derived.PublicField);
            Console.WriteLine(derived.InternalField);
            Console.WriteLine(derived.ProtectedInternalField);
        }
    }
} 