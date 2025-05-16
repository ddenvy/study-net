using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace MessageBrokers.DeliveryGuarantees
{
    public class Message
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int RetryCount { get; set; }
    }

    public class DeliveryGuaranteesService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string ExchangeName = "guaranteed_exchange";
        private const string QueueName = "guaranteed_queue";
        private const string DeadLetterExchange = "dead_letter_exchange";
        private const string DeadLetterQueue = "dead_letter_queue";
        private const string RoutingKey = "guaranteed_key";
        private const int MaxRetries = 3;

        public DeliveryGuaranteesService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Настройка Dead Letter Exchange
            _channel.ExchangeDeclare(
                exchange: DeadLetterExchange,
                type: ExchangeType.Direct,
                durable: true);

            _channel.QueueDeclare(
                queue: DeadLetterQueue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _channel.QueueBind(
                queue: DeadLetterQueue,
                exchange: DeadLetterExchange,
                routingKey: RoutingKey);

            // Настройка основного обменника и очереди
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Direct,
                durable: true);

            var arguments = new System.Collections.Generic.Dictionary<string, object>
            {
                { "x-dead-letter-exchange", DeadLetterExchange },
                { "x-dead-letter-routing-key", RoutingKey }
            };

            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: arguments);

            _channel.QueueBind(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: RoutingKey);

            // Включение подтверждений публикации
            _channel.ConfirmSelect();
        }

        public void PublishMessageWithGuarantee(Message message)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.MessageId = message.Id;
            properties.Headers = new System.Collections.Generic.Dictionary<string, object>
            {
                { "retry-count", message.RetryCount }
            };

            // Публикация с подтверждением
            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: RoutingKey,
                basicProperties: properties,
                body: body);

            // Ожидание подтверждения
            if (!_channel.WaitForConfirms(TimeSpan.FromSeconds(5)))
            {
                throw new Exception("Не получено подтверждение публикации");
            }

            Console.WriteLine($"Отправлено сообщение: {messageJson}");
        }

        public void ConsumeMessagesWithGuarantee()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<Message>(messageJson);

                try
                {
                    // Имитация обработки сообщения
                    ProcessMessage(message);

                    // Подтверждение успешной обработки
                    _channel.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine($"Сообщение обработано: {messageJson}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обработки: {ex.Message}");

                    // Проверка количества попыток
                    var retryCount = (int)ea.BasicProperties.Headers["retry-count"];
                    if (retryCount < MaxRetries)
                    {
                        // Повторная отправка в основную очередь
                        var properties = _channel.CreateBasicProperties();
                        properties.Persistent = true;
                        properties.MessageId = message.Id;
                        properties.Headers = new System.Collections.Generic.Dictionary<string, object>
                        {
                            { "retry-count", retryCount + 1 }
                        };

                        _channel.BasicPublish(
                            exchange: ExchangeName,
                            routingKey: RoutingKey,
                            basicProperties: properties,
                            body: body);

                        _channel.BasicAck(ea.DeliveryTag, false);
                        Console.WriteLine($"Сообщение отправлено на повторную обработку. Попытка {retryCount + 1}");
                    }
                    else
                    {
                        // Отправка в Dead Letter Queue
                        _channel.BasicReject(ea.DeliveryTag, false);
                        Console.WriteLine($"Сообщение отправлено в Dead Letter Queue после {MaxRetries} попыток");
                    }
                }
            };

            _channel.BasicConsume(
                queue: QueueName,
                autoAck: false,
                consumer: consumer);
        }

        private void ProcessMessage(Message message)
        {
            // Имитация обработки с возможной ошибкой
            if (new Random().Next(3) == 0)
            {
                throw new Exception("Случайная ошибка обработки");
            }

            Thread.Sleep(1000); // Имитация работы
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
            Console.WriteLine("Демонстрация гарантий доставки\n");

            using (var service = new DeliveryGuaranteesService())
            {
                // Отправка сообщений
                for (int i = 1; i <= 5; i++)
                {
                    var message = new Message
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = $"Тестовое сообщение #{i}",
                        Timestamp = DateTime.UtcNow,
                        RetryCount = 0
                    };

                    service.PublishMessageWithGuarantee(message);
                }

                Console.WriteLine("\nОжидание сообщений...");
                service.ConsumeMessagesWithGuarantee();

                Console.WriteLine("Нажмите Enter для выхода");
                Console.ReadLine();
            }
        }
    }
} 