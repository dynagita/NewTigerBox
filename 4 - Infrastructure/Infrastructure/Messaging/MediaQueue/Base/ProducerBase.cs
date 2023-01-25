using Microsoft.Extensions.Options;
using NewTigerBox.Common.Response;
using NewTigerBox.Infrastructure.Messaging.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewTigerBox.Infrastructure.Messaging.MediaQueue.Base
{
    internal class ProducerBase<T> : IProducerBase<T>
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

        public ProducerBase(
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

        public async Task<Output> ProduceAsync(T message, CancellationToken cancellationToken)
        {
            try
            {
                return await _retry.ExecuteAsync(async () =>
                {
                    var channel = _connection.CreateModel();
                    channel.QueueDeclare(
                        queue: QueueName,
                        durable: _rabbitConfiguration.Value.Durable,
                        exclusive: _rabbitConfiguration.Value.Exclusive,
                        autoDelete: _rabbitConfiguration.Value.AutoDelete,
                        arguments: null
                        );

                    var jsonMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });

                    var serializedMessage = Encoding.UTF8.GetBytes(jsonMessage);

                    channel.BasicPublish(exchange: "",
                                        routingKey: QueueName,
                                        basicProperties: null,
                                        body: serializedMessage);

                    return Output.ValidResult();
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Producer} - {Method} - Exception Thrown", this.GetType().Name, nameof(ProduceAsync));
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
