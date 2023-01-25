using NewTigerBox.Common.Response;
using System.Threading;
using System.Threading.Tasks;

namespace NewTigerBox.Infrastructure.Messaging.MediaQueue.Base
{
    public interface IConsumerBase<T>
    {
        Task<Output> ConsumeAsync(CancellationToken cancellationToken);
    }
}
