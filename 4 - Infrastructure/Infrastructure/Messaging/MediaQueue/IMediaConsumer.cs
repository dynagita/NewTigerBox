using NewTigerBox.Domain.Model;
using NewTigerBox.Infrastructure.Messaging.MediaQueue.Base;

namespace NewTigerBox.Infrastructure.Messaging.MediaQueue
{
    internal interface IMediaConsumer : IConsumerBase<Media>
    {
    }
}
