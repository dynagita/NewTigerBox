using NewTigerBox.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NewTigerBox.Domain.Repositories
{
    public interface IMediaRepository : IRepository<Media>
    {
        Task<List<Media>> GetByAlbumAsync(Guid albumId, CancellationToken cancellationToken);
    }
}
