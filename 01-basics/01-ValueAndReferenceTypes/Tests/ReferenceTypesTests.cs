using System;
using Xunit;
using ReferenceTypesExample;

namespace Tests
{
    public class ReferenceTypesTests
    {
        [Fact]
        public void Rectangle_Copy_ShouldShareReference()
        {
            // Arrange
            var rect1 = new Rectangle(10, 20);
            
            // Act
            var rect2 = rect1;
            rect2.Width = 30;
            
            // Assert
            Assert.Equal(30, rect1.Width);
            Assert.Equal(30, rect2.Width);
        }

        [Fact]
        public void Array_Copy_ShouldShareReference()
        {
            // Arrange
            int[] arr1 = new int[] { 1, 2, 3 };
            
            // Act
            int[] arr2 = arr1;
            arr2[0] = 10;
            
            // Assert
            Assert.Equal(10, arr1[0]);
            Assert.Equal(10, arr2[0]);
        }

        [Fact]
        public void String_Copy_ShouldCreateNewInstance()
        {
            // Arrange
            string str1 = "Hello";
            
            // Act
            string str2 = str1;
            str2 = "World";
            
            // Assert
            Assert.Equal("Hello", str1);
            Assert.Equal("World", str2);
        }
    }
} 