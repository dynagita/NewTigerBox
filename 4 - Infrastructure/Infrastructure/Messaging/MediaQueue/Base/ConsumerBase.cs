using Microsoft.Extensions.Options;
using NewTigerBox.Common.Response;
using NewTigerBox.Infrastructure.Messaging.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewTigerBox.Infrastructure.Messaging.MediaQueue.Base
{
    internal class ConsumerBase<T> : IConsumerBase<T>
    {
        private readonly IOptions<RabbitConfiguration> _rabbitConfiguration;
        private readonly ILogger _logger;
        private readonly IConnection _connection;
        private readonly AsyncRetryPolicy _retry;

        public string? QueueName
        {
            get
            {
                return _rabbitConfiguration
                    .Value
                    .Queues
                    .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.EventName) &&
                                         x.EventName.Equals(typeof(T).Name))?
                    .QueueName;
            }
        }

        public ConsumerBase(
            IOptions<RabbitConfiguration> rabbitConfiguration,
            ILogger logger)
        {
            _rabbitConfiguration = rabbitConfiguration;
            
            _logger = logger;
            
            _retry = Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(3, retryAttemp)));

            _connection = new ConnectionFactory()
            {
                Uri = new Uri(_rabbitConfiguration.Value.Connection)
            }.CreateConnection();

            Validate();
        }

        public async Task<Output> ConsumeAsync(CancellationToken cancellationToken)
        {
            try
            {
                T response = (T)Activator.CreateInstance(typeof(T));
                var chanel = _connection.CreateModel();
                chanel.QueueDeclare(queue: QueueName,
                                     durable: _rabbitConfiguration.Value.Durable,
                                     exclusive: _rabbitConfiguration.Value.Exclusive,
                                     autoDelete: _rabbitConfiguration.Value.AutoDelete,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(chanel);

                consumer.Received += (model, deliveryArgs) =>
                {
                    var body = deliveryArgs.Body.ToArray();
                    var envelope = Encoding.UTF8.GetString(body);
                    response = JsonConvert.DeserializeObject<T>(envelope);
                };


                chanel.BasicConsume(queue: QueueName,
                                     autoAck: true,
                                     consumer: consumer);

                return Output.ValidResult(response);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Consumer} - {Method} - Exception Thrown", this.GetType().Name, nameof(ConsumeAsync));
                return Output.InvalidResult($"It was impossible queue {typeof(T).Name} event.");
            }
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(QueueName))
            {
                throw new ArgumentException($"It was impossible find a queue for event class {typeof(T).Name}.");
            }
        }
    }
}
