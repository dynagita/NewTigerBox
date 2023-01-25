using System.Collections.Generic;

namespace NewTigerBox.Infrastructure.Messaging.Configuration
{
    internal class RabbitConfiguration
    {
        public string Connection { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public List<Queue> Queues { get; set; }
    }
}
