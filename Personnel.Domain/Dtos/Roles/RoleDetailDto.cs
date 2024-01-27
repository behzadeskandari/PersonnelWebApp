using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Roles
{
    public class RoleDetailDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public bool IsSystemRole { get; set; }

        public string SystemName { get; set; }

        public int[] PermisonRecordIds { get; set; }
    }
}
