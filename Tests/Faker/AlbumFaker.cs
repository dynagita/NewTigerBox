using Bogus;
using NewTigerBox.Domain.Model;
using System;

namespace NewTigerBox.Faker
{
    public static class AlbumFaker
    {        
        public static Album CreateValid(bool newAlbum = false)
        {
            var faker = new Faker<Album>("pt_BR")
                .StrictMode(false);

            faker                
                .RuleFor(x => x.CreatedAt, DateTime.Now)
                .RuleFor(x => x.UpdatedAt, DateTime.Now)
                .RuleFor(x => x.ImagePath, f => f.Image.LoremPixelUrl())
                .RuleFor(x => x.MediasPlayedCount, f => f.Random.Int(1, 100))
                .RuleFor(x => x.Name, f => f.Random.Words(5));
                
            if (newAlbum)
                faker.RuleFor(x => x.Id, Guid.Empty);
            else
                faker.RuleFor(x => x.Id, Guid.NewGuid());
            
            var album = faker.Generate();

            album.Medias = MediaFaker.CreateValid(10, false, album);

            return album;
        }
    }
}
