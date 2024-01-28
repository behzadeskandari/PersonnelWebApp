using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Application.Mangers
{
    public class BaseRoleManager : RoleManager<Roles>
    {
        public BaseRoleManager(IRoleStore<Roles> store, IEnumerable<IRoleValidator<Roles>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<Roles>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
