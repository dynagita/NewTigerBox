using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewTigerBox.Domain.Model;

namespace NewTigerBox.Infrastructure.Repository.Context.EntityMap
{
    internal class AlbumMap : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable(nameof(Album));

            builder.HasKey(x => x.Id)
                .HasName("PK_MEDIA");

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever()
                .HasComment("Define album registers key");

            builder.Property(x => x.MediasPlayedCount)
                .HasColumnName("MediasPlayedCount")
                .HasComment("Count how many album's media has been played.")
                .HasColumnType("int")
                .IsRequired(true)
                .HasDefaultValue(0);

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasComment("Define album's name.")
                .HasColumnType("varchar(500)")
                .IsRequired(true);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasComment("Define when album was inserted at database.")
                .HasColumnType("DateTime")
                .IsRequired(true);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasComment("Define when album has been updated for the last time.")
                .HasColumnType("DateTime")
                .IsRequired(true);            

            builder.Property(x => x.ImagePath)
                .HasColumnName("UpdatedAt")
                .HasComment("Define album's image path.")
                .HasColumnType("varchar(1000)")
                .IsRequired(true);

            builder.HasMany(album => album.Medias)
                .WithOne(media => media.Album)
                .HasConstraintName("FK_MEDIA_ALBUM");
        }
    }
}
