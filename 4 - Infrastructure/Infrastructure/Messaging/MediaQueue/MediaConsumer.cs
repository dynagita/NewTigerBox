using Microsoft.Extensions.Options;
using NewTigerBox.Domain.Model;
using NewTigerBox.Infrastructure.Messaging.Configuration;
using NewTigerBox.Infrastructure.Messaging.MediaQueue.Base;
using RabbitMQ.Client;
using Serilog;

namespace NewTigerBox.Infrastructure.Messaging.MediaQueue
{
    internal class MediaConsumer : ConsumerBase<Media>, IMediaConsumer
    {
        public MediaConsumer(
            IOptions<RabbitConfiguration> rabbitConfiguration, 
            ILogger logger) : base(rabbitConfiguration, logger)
        {
        }
    }
}
