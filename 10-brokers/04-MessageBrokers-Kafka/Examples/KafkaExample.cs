using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Kafka
{
    public class KafkaService
    {
        private readonly ProducerConfig _producerConfig;
        private readonly ConsumerConfig _consumerConfig;
        private const string TopicName = "my_topic";
        private const string GroupId = "my_group";

        public KafkaService()
        {
            // Настройка производителя
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                EnableIdempotence = true,
                EnableDeliveryReports = true,
                MessageSendMaxRetries = 3,
                RetryBackoffMs = 1000
            };

            // Настройка потребителя
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
        }

        public async Task ProduceMessage(string message)
        {
            using var producer = new ProducerBuilder<string, string>(_producerConfig).Build();

            try
            {
                // Отправка сообщения
                var result = await producer.ProduceAsync(
                    TopicName,
                    new Message<string, string>
                    {
                        Key = Guid.NewGuid().ToString(),
                        Value = message
                    });

                Console.WriteLine($"Отправлено сообщение: {message}");
                Console.WriteLine($"Партиция: {result.Partition}, Офсет: {result.Offset}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Ошибка отправки: {ex.Error.Reason}");
            }
        }

        public void ConsumeMessages(CancellationToken cancellationToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();

            consumer.Subscribe(TopicName);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // Получение сообщения
                        var result = consumer.Consume(cancellationToken);

                        if (result != null)
                        {
                            Console.WriteLine($"Получено сообщение: {result.Message.Value}");
                            Console.WriteLine($"Партиция: {result.Partition}, Офсет: {result.Offset}");

                            // Подтверждение получения
                            consumer.Commit(result);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Ошибка получения: {ex.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var kafka = new KafkaService();
            var cts = new CancellationTokenSource();

            // Запуск потребителя в отдельном потоке
            var consumerTask = Task.Run(() => kafka.ConsumeMessages(cts.Token));

            // Отправка сообщений
            for (int i = 1; i <= 5; i++)
            {
                await kafka.ProduceMessage($"Тестовое сообщение {i}");
                await Task.Delay(1000);
            }

            Console.WriteLine("Нажмите Enter для выхода");
            Console.ReadLine();

            // Остановка потребителя
            cts.Cancel();
            await consumerTask;
        }
    }
} 