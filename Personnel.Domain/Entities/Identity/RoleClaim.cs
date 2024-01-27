using Microsoft.AspNetCore.Identity;
using Personnel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<int>, IEntity
    {
        public RoleClaim()
        {
            CreatedClaim = DateTime.Now;
        }

        public DateTime CreatedClaim { get; set; }
        public Roles Role { get; set; }

    }
}
