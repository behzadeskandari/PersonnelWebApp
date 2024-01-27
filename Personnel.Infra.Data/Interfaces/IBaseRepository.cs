using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Interfaces
{
    public interface IBaseRepository<out TEntity>
    {
        IQueryable<TEntity> GetQueryableNoTracking();
        public IQueryable<TEntity> TableNoTracking { get; }
    }
}
