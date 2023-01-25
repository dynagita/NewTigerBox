using NewTigerBox.Domain.Model;
using NewTigerBox.Domain.Repositories;
using NewTigerBox.Infrastructure.Repository.Context;

namespace NewTigerBox.Infrastructure.Repository
{
    public class AlbumRepository : Repository<Album>, IAlbumRepository
    {
        public AlbumRepository(NewTigerBoxContext context) : base(context)
        {
        }
    }
}
