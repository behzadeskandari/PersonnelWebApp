using Personnel.Domain.Entities.Identity;
using Personnel.Domain.MapperProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Users
{
    public class UserInRoleNewDto : ICreateMapper<UserInRole>
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }
    }
}
