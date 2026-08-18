using Inventory.Application.Common.Interfaces;
using Inventory.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Inventory.Infrastructure.Data.Interceptors;

public sealed class AuditableEntityInterceptor(IUser user, TimeProvider dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null) return;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified)) continue;

            var utcNow = dateTime.GetUtcNow();

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = user.Id;
                entry.Entity.CreatedAtUtc = utcNow;
            }

            entry.Entity.LastModifiedBy = user.Id;
            entry.Entity.LastModifiedUtc = utcNow;
        }
    }
}
