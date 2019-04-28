using System;

namespace GameEngine
{
    public class TrackableInstance
    {
        public Guid TrackingId { get; set; } = Guid.NewGuid();
    }
}
