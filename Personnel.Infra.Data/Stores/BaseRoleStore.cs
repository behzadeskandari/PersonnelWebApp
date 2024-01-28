using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personnel.Infra.Data.Context;

namespace Personnel.Infra.Data.Stores
{

    public class BaseRoleStore : RoleStore<Roles, PersonnelDbContext, int, UserInRole, RoleClaim>
    {
        public BaseRoleStore(PersonnelDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
