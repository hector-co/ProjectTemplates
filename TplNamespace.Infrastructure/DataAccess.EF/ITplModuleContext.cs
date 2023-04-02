using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TplNamespace.Infrastructure.DataAccess.EF;

public interface ITplModuleContext 
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

