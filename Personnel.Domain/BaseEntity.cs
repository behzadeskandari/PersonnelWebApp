using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain
{
    public abstract class BaseEntity<TKey> : IEntity
    {
        public TKey Id { get; set; }

    }

    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
