using Project.Core.Entities.General;
using Project.Core.Interfaces.IServices;
using Project.Infrastructure.Data;
using System.Text.Json;

namespace Project.Core.Services
{
    /// <summary>
    /// Service for logging entity changes (audit trail)
    /// </summary>
    public class AuditLoggingService : IAuditLoggingService
    {
        private readonly ApplicationDbContext _context;

        public AuditLoggingService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<AuditLog> LogCreateAsync(string entityName, int entityId, string newValues, string? changedBy, string? ipAddress, CancellationToken cancellationToken)
        {
            var auditLog = new AuditLog
            {
                EntityName = entityName,
                EntityId = entityId,
                Operation = "Create",
                NewValues = newValues,
                ChangedBy = changedBy,
                IpAddress = ipAddress,
                ChangedAt = DateTime.UtcNow
            };

            _context.Set<AuditLog>().Add(auditLog);
            await _context.SaveChangesAsync(cancellationToken);
            return auditLog;
        }

        public async Task<AuditLog> LogUpdateAsync(string entityName, int entityId, string oldValues, string newValues, string? changedBy, string? ipAddress, CancellationToken cancellationToken)
        {
            var auditLog = new AuditLog
            {
                EntityName = entityName,
                EntityId = entityId,
                Operation = "Update",
                OldValues = oldValues,
                NewValues = newValues,
                ChangedBy = changedBy,
                IpAddress = ipAddress,
                ChangedAt = DateTime.UtcNow
            };

            _context.Set<AuditLog>().Add(auditLog);
            await _context.SaveChangesAsync(cancellationToken);
            return auditLog;
        }

        public async Task<AuditLog> LogDeleteAsync(string entityName, int entityId, string oldValues, string? changedBy, string? ipAddress, CancellationToken cancellationToken)
        {
            var auditLog = new AuditLog
            {
                EntityName = entityName,
                EntityId = entityId,
                Operation = "Delete",
                OldValues = oldValues,
                ChangedBy = changedBy,
                IpAddress = ipAddress,
                ChangedAt = DateTime.UtcNow
            };

            _context.Set<AuditLog>().Add(auditLog);
            await _context.SaveChangesAsync(cancellationToken);
            return auditLog;
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string entityName, int? entityId, CancellationToken cancellationToken)
        {
            var query = _context.Set<AuditLog>().AsQueryable();

            if (!string.IsNullOrEmpty(entityName))
            {
                query = query.Where(x => x.EntityName == entityName);
            }

            if (entityId.HasValue)
            {
                query = query.Where(x => x.EntityId == entityId);
            }

            return await Task.FromResult(query.OrderByDescending(x => x.ChangedAt).AsEnumerable());
        }
    }
}
