using System;
using Xunit;
using ValueTypesExample;

namespace Tests
{
    public class ValueTypesTests
    {
        [Fact]
        public void Point_Copy_ShouldCreateNewInstance()
        {
            // Arrange
            var p1 = new Point(10, 20);
            
            // Act
            var p2 = p1;
            p2.X = 30;
            
            // Assert
            Assert.Equal(10, p1.X);
            Assert.Equal(30, p2.X);
        }

        [Fact]
        public void Int_Copy_ShouldCreateNewInstance()
        {
            // Arrange
            int number1 = 10;
            
            // Act
            int number2 = number1;
            number2 = 20;
            
            // Assert
            Assert.Equal(10, number1);
            Assert.Equal(20, number2);
        }

        [Fact]
        public void Tuple_Copy_ShouldCreateNewInstance()
        {
            // Arrange
            var tuple1 = (x: 10, y: 20);
            
            // Act
            var tuple2 = tuple1;
            tuple2.x = 30;
            
            // Assert
            Assert.Equal(10, tuple1.x);
            Assert.Equal(30, tuple2.x);
        }
    }
} 