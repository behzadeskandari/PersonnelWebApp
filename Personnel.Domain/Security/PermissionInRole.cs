using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Security
{

    public class PermissionInRole
    {
        public int RoleId { get; set; }

        public int PermissionRecordId { get; set; }


        public Roles Role { get; set; }

        public PermissionRecord PermissionRecord { get; set; }
    }
}
