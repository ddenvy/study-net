using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MessageBrokers.RabbitMQ
{
    public class RabbitMQService : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string ExchangeName = "my_exchange";
        private const string QueueName = "my_queue";
        private const string RoutingKey = "my_routing_key";

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

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; // Сохранение сообщения на диск

            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: RoutingKey,
                basicProperties: properties,
                body: body);

            Console.WriteLine($"Отправлено сообщение: {message}");
        }

        public void ConsumeMessages()
        {
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Получено сообщение: {message}");

                // Подтверждение получения сообщения
                _channel.BasicAck(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false);
            };

            _channel.BasicConsume(
                queue: QueueName,
                autoAck: false,
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
        static async Task Main(string[] args)
        {
            using var rabbitMQ = new RabbitMQService();

            // Запуск потребителя
            rabbitMQ.ConsumeMessages();

            // Отправка сообщений
            for (int i = 1; i <= 5; i++)
            {
                rabbitMQ.PublishMessage($"Тестовое сообщение {i}");
                await Task.Delay(1000);
            }

            Console.WriteLine("Нажмите Enter для выхода");
            Console.ReadLine();
        }
    }
} 