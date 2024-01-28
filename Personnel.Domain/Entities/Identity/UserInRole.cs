using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class UserInRole : IdentityUserRole<int>
    {
        public int Id { get; set; }
        public virtual Roles Role { get; set; }


        public bool Deleted { get; set; }
        public virtual User User { get; set; }
    }
}
