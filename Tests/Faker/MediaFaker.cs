using Bogus;
using NewTigerBox.Domain.Model;
using System;
using System.Collections.Generic;

namespace NewTigerBox.Faker
{
    public static class MediaFaker
    {
        public static Media CreateValid(bool newMedia = false, Album album = null)
        {
            var faker = new Faker<Media>("pt_BR")
                .StrictMode(false);

            faker
                .RuleFor(x => x.CreatedAt, DateTime.Now)
                .RuleFor(x => x.UpdatedAt, DateTime.Now)
                .RuleFor(x => x.ImagePath, f => f.Image.LoremPixelUrl())
                .RuleFor(x => x.PlayedCount, f => f.Random.Int(1, 100))
                .RuleFor(x => x.Name, f => f.Random.Words(5))
                .RuleFor(x => x.FilePath, f => f.Image.LoremPixelUrl());

            if (newMedia)
                faker.RuleFor(x => x.Id, Guid.Empty);
            else
                faker.RuleFor(x => x.Id, Guid.NewGuid());

            if (album is not null)
                faker.RuleFor(x => x.Album, album);

            return faker.Generate();
        }

        public static List<Media> CreateValid(int count, bool newMedia = false, Album album = null)
        {
            List<Media> medias = new List<Media>();
            for (int i = 0; i < count; i++)
            {
                medias.Add(CreateValid(newMedia, album));
            }

            return medias;
        }        
    }
}
