using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewTigerBox.Domain.Model;

namespace NewTigerBox.Infrastructure.Repository.Context.EntityMap
{
    internal class MediaMap : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable(nameof(Media));

            builder.HasKey(x => x.Id)
                .HasName("PK_MEDIA");

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever()
                .HasComment("Define media registers key");

            builder.Property(x => x.PlayedCount)
                .HasColumnName("PlayedCount")
                .HasComment("Count how many times this media has been played.")
                .HasColumnType("int")
                .IsRequired(true)
                .HasDefaultValue(0);

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasComment("Define media's name.")
                .HasColumnType("varchar(500)")
                .IsRequired(true);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasComment("Define when media was inserted at database.")
                .HasColumnType("DateTime")
                .IsRequired(true);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasComment("Define when media has been updated for the last time.")
                .HasColumnType("DateTime")
                .IsRequired(true);

            builder.Property(x => x.FilePath)
                .HasColumnName("UpdatedAt")
                .HasComment("Define media's file path.")
                .HasColumnType("varchar(1000)")
                .IsRequired(true);

            builder.Property(x => x.ImagePath)
                .HasColumnName("UpdatedAt")
                .HasComment("Define media's image path.")
                .HasColumnType("varchar(1000)")
                .IsRequired(true);

            builder.HasOne(media => media.Album)
                .WithMany(album => album.Medias)
                .HasConstraintName("FK_MEDIA_ALBUM");
        }
    }
}
