using Microsoft.EntityFrameworkCore;
using NewTigerBox.Domain.Model;
using NewTigerBox.Domain.Repositories;
using NewTigerBox.Infrastructure.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewTigerBox.Infrastructure.Repository
{
    public class MediaRepository : Repository<Media>, IMediaRepository
    {
        public MediaRepository(NewTigerBoxContext context) : base(context)
        {
        }

        public async Task<List<Media>> GetByAlbumAsync(Guid albumId, CancellationToken cancellationToken)
        {
            return await Retry.ExecuteAsync(async () => await DbSet.Where(x => x.Album.Id == albumId).ToListAsync(cancellationToken));
        }
    }
}
