using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<int>, IEntity
    {
        public User User { get; set; }
    }
}
