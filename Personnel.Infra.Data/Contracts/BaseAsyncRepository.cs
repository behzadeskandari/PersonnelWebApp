using Microsoft.EntityFrameworkCore;
using Personnel.Domain.Core.Contracts;
using Personnel.Domain.Entities;
using Personnel.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Contracts
{
    public abstract class BaseAsyncRepository<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        protected BaseAsyncRepository(PersonnelDbContext dbContext) : base(dbContext)
        {
        }

        public virtual PagedList<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex)
        {
            var query = TableNoTracking.Where(predicate);
            return new PagedList<TEntity>(query, pageSize, pageIndex);
        }
        protected virtual async Task<List<TEntity>> ListAllAsync()
        {
            return await Entities.ToListAsync();
        }

        protected virtual async Task AddAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);

        }

        protected virtual void Update(TEntity entity)
        {

            Entities.Update(entity);
        }

        protected virtual void Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }
    }


    public abstract class BaseRepository<TEntity> : BaseEntity where TEntity : class
    {
        public readonly PersonnelDbContext DbContext;
        protected DbSet<TEntity> Entities { get; }
        protected virtual IQueryable<TEntity> Table => Entities;
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTrackingWithIdentityResolution();
        public IQueryable<TEntity> GetQueryableNoTracking() => TableNoTracking;
        protected BaseRepository(PersonnelDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = DbContext.Set<TEntity>(); // City => Cities
        }
    }
}
