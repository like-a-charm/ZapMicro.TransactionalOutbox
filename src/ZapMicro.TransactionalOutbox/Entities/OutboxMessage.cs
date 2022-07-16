using System;

namespace ZapMicro.TransactionalOutbox.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Payload { get; set; }

        protected bool Equals(OutboxMessage other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OutboxMessage)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}