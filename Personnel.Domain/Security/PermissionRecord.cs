using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Security
{

    public class PermissionRecord : BaseEntity
    {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public string Category { get; set; }

        public ICollection<PermissionInRole> PermissionInRoles { get; set; } = new List<PermissionInRole>();
    }
}
