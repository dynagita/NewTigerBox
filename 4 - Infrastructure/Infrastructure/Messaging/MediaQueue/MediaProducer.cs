using Microsoft.Extensions.Options;
using NewTigerBox.Domain.Model;
using NewTigerBox.Infrastructure.Messaging.Configuration;
using NewTigerBox.Infrastructure.Messaging.MediaQueue.Base;
using Serilog;

namespace NewTigerBox.Infrastructure.Messaging.MediaQueue
{
    internal class MediaProducer : ProducerBase<Media>, IMediaProducer
    {
        public MediaProducer(
            IOptions<RabbitConfiguration> rabbitConfiguration, 
            ILogger logger) : base(rabbitConfiguration, logger)
        {
        }
    }
}
