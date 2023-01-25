using System.Collections.Generic;

namespace NewTigerBox.Domain.Model
{
    public class Album : EntityBase
    {
        public Album() : base() 
        { 
        }

        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int MediasPlayedCount { get; set; }

        public List<Media> Medias { get; set; }
    }
}
