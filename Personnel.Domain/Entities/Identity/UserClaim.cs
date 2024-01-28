using Microsoft.AspNetCore.Identity;
using Personnel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<int>, IEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public bool Deleted { get; set; }
    }
}
