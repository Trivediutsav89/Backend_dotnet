namespace Project.Core.Entities.General
{
    /// <summary>
    /// Base entity with audit tracking capabilities
    /// </summary>
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }

    /// <summary>
    /// Audit log entity for tracking all entity changes
    /// </summary>
    public class AuditLog
    {
        public int Id { get; set; }
        public string? EntityName { get; set; }
        public int EntityId { get; set; }
        public string? Operation { get; set; } // Create, Update, Delete
        public string? OldValues { get; set; } // JSON serialized old values
        public string? NewValues { get; set; } // JSON serialized new values
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public string? IpAddress { get; set; }
    }
}
