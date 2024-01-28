using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Users
{
    public class UserDto : LoginUserDto
    {

        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;

            }
        }

        public string? PhoneNumber { get; set; }

        public string? Role { get; set; }
        public string? Token { get; set; }
        public bool IsAdmin { get; set; }
        public List<UserClaimsDto>? Claims { get; set; }
    }
}
