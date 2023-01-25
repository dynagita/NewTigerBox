namespace NewTigerBox.Domain.Model
{
    public class Media : EntityBase
    {
        public Media() : base()
        {
            
        }

        public string Name { get; set; }

        public string FilePath { get; set; }

        public string ImagePath { get; set; }

        public int PlayedCount { get; set; }

        public Album Album { get; set; }
    }
}
