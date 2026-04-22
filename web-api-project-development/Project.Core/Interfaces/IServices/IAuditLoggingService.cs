using Project.Core.Entities.General;

namespace Project.Core.Interfaces.IServices
{
    /// <summary>
    /// Interface for audit logging service to track entity changes
    /// </summary>
    public interface IAuditLoggingService
    {
        /// <summary>
        /// Logs a create operation
        /// </summary>
        Task<AuditLog> LogCreateAsync(string entityName, int entityId, string newValues, string? changedBy, string? ipAddress, CancellationToken cancellationToken);

        /// <summary>
        /// Logs an update operation
        /// </summary>
        Task<AuditLog> LogUpdateAsync(string entityName, int entityId, string oldValues, string newValues, string? changedBy, string? ipAddress, CancellationToken cancellationToken);

        /// <summary>
        /// Logs a delete operation
        /// </summary>
        Task<AuditLog> LogDeleteAsync(string entityName, int entityId, string oldValues, string? changedBy, string? ipAddress, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves audit logs for a specific entity
        /// </summary>
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string entityName, int? entityId, CancellationToken cancellationToken);
    }
}
