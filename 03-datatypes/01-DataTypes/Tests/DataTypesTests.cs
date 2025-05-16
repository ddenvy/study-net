using System;
using System.Numerics;
using Xunit;

namespace DataTypes.Tests
{
    public class DataTypesTests
    {
        // Тест 1: Проверка целочисленных типов
        [Fact]
        public void IntegerTypes_ShouldWork()
        {
            // Arrange
            sbyte sbyteValue = -128;
            byte byteValue = 255;
            short shortValue = -32768;
            ushort ushortValue = 65535;
            int intValue = -2147483648;
            uint uintValue = 4294967295;
            long longValue = -9223372036854775808;
            ulong ulongValue = 18446744073709551615;

            // Act & Assert
            Assert.Equal(-128, sbyteValue);
            Assert.Equal(255, byteValue);
            Assert.Equal(-32768, shortValue);
            Assert.Equal(65535, ushortValue);
            Assert.Equal(-2147483648, intValue);
            Assert.Equal(4294967295, uintValue);
            Assert.Equal(-9223372036854775808, longValue);
            Assert.Equal(18446744073709551615, ulongValue);
        }

        // Тест 2: Проверка типов с плавающей точкой
        [Fact]
        public void FloatingPointTypes_ShouldWork()
        {
            // Arrange
            float floatValue = 3.14f;
            double doubleValue = 3.14159265359;
            decimal decimalValue = 3.14159265359m;

            // Act & Assert
            Assert.Equal(3.14f, floatValue, 2);
            Assert.Equal(3.14159265359, doubleValue, 11);
            Assert.Equal(3.14159265359m, decimalValue);
        }

        // Тест 3: Проверка символьных типов
        [Fact]
        public void CharacterTypes_ShouldWork()
        {
            // Arrange
            char charValue = 'A';
            string stringValue = "Hello, World!";

            // Act & Assert
            Assert.Equal('A', charValue);
            Assert.Equal("Hello, World!", stringValue);
        }

        // Тест 4: Проверка логического типа
        [Fact]
        public void BooleanType_ShouldWork()
        {
            // Arrange
            bool trueValue = true;
            bool falseValue = false;

            // Act & Assert
            Assert.True(trueValue);
            Assert.False(falseValue);
        }

        // Тест 5: Проверка типов даты и времени
        [Fact]
        public void DateTimeTypes_ShouldWork()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var timeSpan = new TimeSpan(1, 2, 3, 4);
            var dateOnly = new DateOnly(2024, 1, 1);
            var timeOnly = new TimeOnly(12, 0, 0);

            // Act & Assert
            Assert.Equal(2024, dateTime.Year);
            Assert.Equal(1, dateTime.Month);
            Assert.Equal(1, dateTime.Day);
            Assert.Equal(12, dateTime.Hour);
            Assert.Equal(1, timeSpan.Days);
            Assert.Equal(2, timeSpan.Hours);
            Assert.Equal(3, timeSpan.Minutes);
            Assert.Equal(4, timeSpan.Seconds);
            Assert.Equal(2024, dateOnly.Year);
            Assert.Equal(1, dateOnly.Month);
            Assert.Equal(1, dateOnly.Day);
            Assert.Equal(12, timeOnly.Hour);
            Assert.Equal(0, timeOnly.Minute);
        }

        // Тест 6: Проверка типов для больших чисел
        [Fact]
        public void BigIntegerType_ShouldWork()
        {
            // Arrange
            var bigInt = BigInteger.Parse("123456789012345678901234567890");

            // Act & Assert
            Assert.Equal(BigInteger.Parse("123456789012345678901234567890"), bigInt);
        }

        // Тест 7: Проверка типов для работы с GUID
        [Fact]
        public void GuidType_ShouldWork()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act & Assert
            Assert.NotEqual(Guid.Empty, guid);
        }

        // Тест 8: Проверка типов для работы с перечислениями
        [Fact]
        public void EnumType_ShouldWork()
        {
            // Arrange
            enum TestEnum { Value1, Value2, Value3 }
            var enumValue = TestEnum.Value2;

            // Act & Assert
            Assert.Equal(TestEnum.Value2, enumValue);
        }

        // Тест 9: Проверка типов для работы с nullable
        [Fact]
        public void NullableTypes_ShouldWork()
        {
            // Arrange
            int? nullableInt = null;
            int? nonNullInt = 42;

            // Act & Assert
            Assert.Null(nullableInt);
            Assert.NotNull(nonNullInt);
            Assert.Equal(42, nonNullInt.Value);
        }

        // Тест 10: Проверка типов для работы с объектами
        [Fact]
        public void ObjectType_ShouldWork()
        {
            // Arrange
            object intObj = 42;
            object stringObj = "Hello";
            object nullObj = null;

            // Act & Assert
            Assert.Equal(42, intObj);
            Assert.Equal("Hello", stringObj);
            Assert.Null(nullObj);
        }
    }
} 