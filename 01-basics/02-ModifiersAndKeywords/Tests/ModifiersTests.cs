using System;
using Xunit;

namespace ModifiersAndKeywords.Tests
{
    public class ModifiersTests
    {
        // Тест 1: Проверка модификаторов доступа
        [Fact]
        public void AccessModifiers_ShouldWorkCorrectly()
        {
            // Arrange
            var testClass = new TestClass();

            // Act & Assert
            Assert.Equal("Public", testClass.PublicField);
            Assert.Equal("Internal", testClass.InternalField);
            Assert.Equal("Protected", testClass.GetProtectedField());
            Assert.Equal("ProtectedInternal", testClass.GetProtectedInternalField());
        }

        // Тест 2: Проверка наследования
        [Fact]
        public void Inheritance_ShouldWorkCorrectly()
        {
            // Arrange
            var derived = new DerivedClass();

            // Act & Assert
            Assert.Equal("Base", derived.GetBaseField());
            Assert.Equal("Derived", derived.DerivedField);
        }

        // Тест 3: Проверка статических членов
        [Fact]
        public void StaticMembers_ShouldWorkCorrectly()
        {
            // Arrange
            StaticClass.Counter = 0;

            // Act
            StaticClass.IncrementCounter();
            StaticClass.IncrementCounter();

            // Assert
            Assert.Equal(2, StaticClass.Counter);
        }

        // Тест 4: Проверка readonly и const
        [Fact]
        public void ReadonlyAndConst_ShouldWorkCorrectly()
        {
            // Arrange
            var testClass = new TestClass();

            // Act & Assert
            Assert.Equal(100, TestClass.ConstantValue);
            Assert.Equal("Readonly", testClass.ReadonlyField);
        }

        // Тест 5: Проверка виртуальных методов
        [Fact]
        public void VirtualMethods_ShouldWorkCorrectly()
        {
            // Arrange
            BaseClass baseClass = new DerivedClass();

            // Act & Assert
            Assert.Equal("Derived", baseClass.VirtualMethod());
        }

        // Тест 6: Проверка ключевого слова this
        [Fact]
        public void ThisKeyword_ShouldWorkCorrectly()
        {
            // Arrange
            var testClass = new TestClass();

            // Act
            testClass.SetField("NewValue");

            // Assert
            Assert.Equal("NewValue", testClass.PublicField);
        }

        // Тест 7: Проверка ключевого слова base
        [Fact]
        public void BaseKeyword_ShouldWorkCorrectly()
        {
            // Arrange
            var derived = new DerivedClass();

            // Act & Assert
            Assert.Equal("Base", derived.GetBaseMethod());
        }

        // Тест 8: Проверка sealed класса
        [Fact]
        public void SealedClass_ShouldWorkCorrectly()
        {
            // Arrange
            var sealedClass = new SealedClass();

            // Act & Assert
            Assert.Equal("Sealed", sealedClass.Method());
        }

        // Тест 9: Проверка partial класса
        [Fact]
        public void PartialClass_ShouldWorkCorrectly()
        {
            // Arrange
            var partialClass = new PartialClass();

            // Act & Assert
            Assert.Equal("Part1", partialClass.Method1());
            Assert.Equal("Part2", partialClass.Method2());
        }

        // Тест 10: Проверка async/await
        [Fact]
        public async Task AsyncAwait_ShouldWorkCorrectly()
        {
            // Arrange
            var asyncClass = new AsyncClass();

            // Act
            var result = await asyncClass.GetValueAsync();

            // Assert
            Assert.Equal("Async", result);
        }
    }

    // Вспомогательные классы для тестов
    public class TestClass
    {
        public string PublicField = "Public";
        internal string InternalField = "Internal";
        protected string ProtectedField = "Protected";
        protected internal string ProtectedInternalField = "ProtectedInternal";
        public readonly string ReadonlyField = "Readonly";
        public const int ConstantValue = 100;

        public string GetProtectedField() => ProtectedField;
        public string GetProtectedInternalField() => ProtectedInternalField;

        public void SetField(string value)
        {
            this.PublicField = value;
        }
    }

    public class BaseClass
    {
        protected string BaseField = "Base";
        public virtual string VirtualMethod() => "Base";
        public string BaseMethod() => "Base";
    }

    public class DerivedClass : BaseClass
    {
        public string DerivedField = "Derived";
        public override string VirtualMethod() => "Derived";
        public string GetBaseField() => BaseField;
        public string GetBaseMethod() => base.BaseMethod();
    }

    public static class StaticClass
    {
        public static int Counter { get; set; }
        public static void IncrementCounter() => Counter++;
    }

    public sealed class SealedClass
    {
        public string Method() => "Sealed";
    }

    public partial class PartialClass
    {
        public string Method1() => "Part1";
    }

    public partial class PartialClass
    {
        public string Method2() => "Part2";
    }

    public class AsyncClass
    {
        public async Task<string> GetValueAsync()
        {
            await Task.Delay(100);
            return "Async";
        }
    }
} 