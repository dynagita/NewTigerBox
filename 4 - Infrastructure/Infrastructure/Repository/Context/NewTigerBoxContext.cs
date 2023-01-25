using Microsoft.EntityFrameworkCore;
using NewTigerBox.Domain.Model;
using NewTigerBox.Infrastructure.Repository.Context.EntityMap;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NewTigerBox.Infrastructure.Repository.Context
{
    [ExcludeFromCodeCoverage]
    public class NewTigerBoxContext : DbContext
    {
        public List<Album> Albuns { get; set; }
        public List<Media> Medias { get; set; }

        public NewTigerBoxContext(DbContextOptions<NewTigerBoxContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AlbumMap());
            builder.ApplyConfiguration(new MediaMap());
        }
    }
}
