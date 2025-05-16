using System;
using System.Collections.Generic;

namespace ObserverPattern
{
    // Интерфейс наблюдателя
    public interface IObserver
    {
        void Update(string message);
    }

    // Интерфейс субъекта
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    // Конкретный субъект
    public class NewsAgency : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private string _news;

        public string News
        {
            get { return _news; }
            set
            {
                _news = value;
                Notify();
            }
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
            Console.WriteLine($"Добавлен новый подписчик. Всего подписчиков: {_observers.Count}");
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
            Console.WriteLine($"Удален подписчик. Осталось подписчиков: {_observers.Count}");
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_news);
            }
        }
    }

    // Конкретные наблюдатели
    public class NewsChannel : IObserver
    {
        private string _name;

        public NewsChannel(string name)
        {
            _name = name;
        }

        public void Update(string message)
        {
            Console.WriteLine($"{_name} получил новость: {message}");
        }
    }

    // Пример использования
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрация паттерна Observer\n");

            // Создание новостного агентства
            var newsAgency = new NewsAgency();

            // Создание новостных каналов
            var channel1 = new NewsChannel("Первый канал");
            var channel2 = new NewsChannel("Второй канал");
            var channel3 = new NewsChannel("Третий канал");

            // Подписка каналов на новости
            newsAgency.Attach(channel1);
            newsAgency.Attach(channel2);
            newsAgency.Attach(channel3);

            // Публикация новости
            Console.WriteLine("\nПубликация первой новости:");
            newsAgency.News = "Важная новость дня!";

            // Отписка одного канала
            Console.WriteLine("\nОтписка второго канала:");
            newsAgency.Detach(channel2);

            // Публикация еще одной новости
            Console.WriteLine("\nПубликация второй новости:");
            newsAgency.News = "Срочное сообщение!";

            // Отписка всех оставшихся каналов
            Console.WriteLine("\nОтписка всех оставшихся каналов:");
            newsAgency.Detach(channel1);
            newsAgency.Detach(channel3);

            // Публикация новости после отписки всех каналов
            Console.WriteLine("\nПубликация новости после отписки всех каналов:");
            newsAgency.News = "Эта новость не будет опубликована";
        }
    }
} 