using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Users
{
    public class UserSearchDto
    {
        public UserSearchDto()
        {
            Types = new List<UserType>();
            JobTypes = new List<UserJobType>();
        }
        public DateTime? CreatedFromUtc { get; set; }
        public DateTime? CreatedToUtc { get; set; }
        public string[] UserRoleIds { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string ZipPostalCode { get; set; }
        public string IpAddress { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public string OperationUnitCode { get; set; }
        public int? Role { get; set; }
        public List<UserType> Types { get; set; }
        public List<UserJobType> JobTypes { get; set; }


    }
}
