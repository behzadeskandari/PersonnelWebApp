using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Personnel.Domain.Entities.Identity;
using Personnel.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Stores
{
    public class BaseUserStore : UserStore<User, Roles, PersonnelDbContext, int, UserClaim, UserInRole, UserLogin, SiteUserToken, RoleClaim>
    {
        public BaseUserStore(PersonnelDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
