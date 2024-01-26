using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Contracts
{
    public interface IDbContext
    {
        EntityEntry Entry(object entity);
        public ChangeTracker ChangeTracker { get; }
        public DatabaseFacade PersonnelDataBase { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();

        bool HasModifiedProperties(object entity) => Entry(entity).Properties.Any(x => x.IsModified);
    }
}
