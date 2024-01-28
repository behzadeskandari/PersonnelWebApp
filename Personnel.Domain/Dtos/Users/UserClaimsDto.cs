using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Users
{

    public class UserClaimsDto
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
