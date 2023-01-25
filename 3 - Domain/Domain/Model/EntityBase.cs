using System;

namespace NewTigerBox.Domain.Model
{
    public class EntityBase
    {
        public EntityBase()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Id = Guid.Empty;
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
