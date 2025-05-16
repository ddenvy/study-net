using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace MessageBrokers.RabbitMQ
{
    public class Message
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class RabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string ExchangeName = "demo_exchange";
        private const string QueueName = "demo_queue";
        private const string RoutingKey = "demo_key";

        public RabbitMQService()
        {
            // Создание подключения
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Объявление обменника
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);

            // Объявление очереди
            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Привязка очереди к обменнику
            _channel.QueueBind(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: RoutingKey);
        }

        public void PublishMessage(Message message)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; // Сохранение сообщений на диск

            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: RoutingKey,
                basicProperties: properties,
                body: body);

            Console.WriteLine($"Отправлено сообщение: {messageJson}");
        }

        public void ConsumeMessages()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<Message>(messageJson);

                Console.WriteLine($"Получено сообщение: {messageJson}");

                // Подтверждение обработки сообщения
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(
                queue: QueueName,
                autoAck: false, // Ручное подтверждение
                consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрация работы с RabbitMQ\n");

            using (var rabbitMQ = new RabbitMQService())
            {
                // Отправка сообщений
                for (int i = 1; i <= 5; i++)
                {
                    var message = new Message
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = $"Тестовое сообщение #{i}",
                        Timestamp = DateTime.UtcNow
                    };

                    rabbitMQ.PublishMessage(message);
                }

                Console.WriteLine("\nОжидание сообщений...");
                rabbitMQ.ConsumeMessages();

                Console.WriteLine("Нажмите Enter для выхода");
                Console.ReadLine();
            }
        }
    }
} 