using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Xunit;

namespace Collections.Tests
{
    public class CollectionsTests
    {
        // Тест 1: Проверка работы с List<T>
        [Fact]
        public void List_ShouldWork()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            list.Add(6);
            list.Remove(3);
            list.Insert(2, 10);

            // Assert
            Assert.Equal(6, list.Count);
            Assert.Equal(10, list[2]);
            Assert.DoesNotContain(3, list);
        }

        // Тест 2: Проверка работы с Dictionary<TKey, TValue>
        [Fact]
        public void Dictionary_ShouldWork()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act
            dict.Add("one", 1);
            dict["two"] = 2;
            dict.TryAdd("three", 3);

            // Assert
            Assert.Equal(3, dict.Count);
            Assert.Equal(1, dict["one"]);
            Assert.True(dict.ContainsKey("two"));
        }

        // Тест 3: Проверка работы с HashSet<T>
        [Fact]
        public void HashSet_ShouldWork()
        {
            // Arrange
            var set = new HashSet<int>();

            // Act
            set.Add(1);
            set.Add(2);
            set.Add(1); // Дубликат

            // Assert
            Assert.Equal(2, set.Count);
            Assert.True(set.Contains(1));
        }

        // Тест 4: Проверка работы с Queue<T>
        [Fact]
        public void Queue_ShouldWork()
        {
            // Arrange
            var queue = new Queue<int>();

            // Act
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            var first = queue.Dequeue();
            var peek = queue.Peek();

            // Assert
            Assert.Equal(1, first);
            Assert.Equal(2, peek);
            Assert.Equal(2, queue.Count);
        }

        // Тест 5: Проверка работы с Stack<T>
        [Fact]
        public void Stack_ShouldWork()
        {
            // Arrange
            var stack = new Stack<int>();

            // Act
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            var last = stack.Pop();
            var peek = stack.Peek();

            // Assert
            Assert.Equal(3, last);
            Assert.Equal(2, peek);
            Assert.Equal(2, stack.Count);
        }

        // Тест 6: Проверка работы с LinkedList<T>
        [Fact]
        public void LinkedList_ShouldWork()
        {
            // Arrange
            var list = new LinkedList<int>();

            // Act
            list.AddLast(1);
            list.AddLast(2);
            list.AddFirst(0);
            var node = list.Find(1);
            list.AddAfter(node, 3);

            // Assert
            Assert.Equal(4, list.Count);
            Assert.Equal(0, list.First.Value);
            Assert.Equal(2, list.Last.Value);
        }

        // Тест 7: Проверка работы с SortedList<TKey, TValue>
        [Fact]
        public void SortedList_ShouldWork()
        {
            // Arrange
            var list = new SortedList<string, int>();

            // Act
            list.Add("b", 2);
            list.Add("a", 1);
            list.Add("c", 3);

            // Assert
            Assert.Equal("a", list.Keys[0]);
            Assert.Equal("b", list.Keys[1]);
            Assert.Equal("c", list.Keys[2]);
        }

        // Тест 8: Проверка работы с ConcurrentDictionary<TKey, TValue>
        [Fact]
        public void ConcurrentDictionary_ShouldWork()
        {
            // Arrange
            var dict = new ConcurrentDictionary<string, int>();

            // Act
            dict.TryAdd("one", 1);
            dict.AddOrUpdate("one", 2, (key, oldValue) => oldValue + 1);
            var value = dict.GetOrAdd("two", 2);

            // Assert
            Assert.Equal(2, dict["one"]);
            Assert.Equal(2, value);
        }

        // Тест 9: Проверка работы с ConcurrentQueue<T>
        [Fact]
        public void ConcurrentQueue_ShouldWork()
        {
            // Arrange
            var queue = new ConcurrentQueue<int>();

            // Act
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.TryDequeue(out var result);
            queue.TryPeek(out var peek);

            // Assert
            Assert.Equal(1, result);
            Assert.Equal(2, peek);
        }

        // Тест 10: Проверка работы с ConcurrentStack<T>
        [Fact]
        public void ConcurrentStack_ShouldWork()
        {
            // Arrange
            var stack = new ConcurrentStack<int>();

            // Act
            stack.Push(1);
            stack.Push(2);
            stack.TryPop(out var result);
            stack.TryPeek(out var peek);

            // Assert
            Assert.Equal(2, result);
            Assert.Equal(1, peek);
        }

        // Тест 11: Проверка работы с ObservableCollection<T>
        [Fact]
        public void ObservableCollection_ShouldWork()
        {
            // Arrange
            var collection = new System.Collections.ObjectModel.ObservableCollection<int>();
            var changes = 0;

            // Act
            collection.CollectionChanged += (s, e) => changes++;
            collection.Add(1);
            collection.Add(2);
            collection.Remove(1);

            // Assert
            Assert.Equal(3, changes);
            Assert.Single(collection);
            Assert.Equal(2, collection[0]);
        }

        // Тест 12: Проверка работы с ReadOnlyCollection<T>
        [Fact]
        public void ReadOnlyCollection_ShouldWork()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };
            var readOnly = new System.Collections.ObjectModel.ReadOnlyCollection<int>(list);

            // Act & Assert
            Assert.Equal(3, readOnly.Count);
            Assert.Equal(1, readOnly[0]);
            Assert.Throws<NotSupportedException>(() => readOnly.Add(4));
        }
    }
} 