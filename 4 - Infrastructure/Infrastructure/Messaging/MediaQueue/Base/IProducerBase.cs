using NewTigerBox.Common.Response;
using System.Threading;
using System.Threading.Tasks;

namespace NewTigerBox.Infrastructure.Messaging.MediaQueue.Base
{
    public interface IProducerBase<T>
    {
        Task<Output> ProduceAsync(T message, CancellationToken cancellationToken);
    }
}
