namespace Shared.DDD;


    public abstract class Entity<TId> : IEntity<TId>
    {
        public TId Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
    }
