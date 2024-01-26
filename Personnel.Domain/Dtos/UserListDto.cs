using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos
{
    public class UserListDto : BaseEntityDto
    {
        public string AdminComment { get; set; }
        public string Email { get; set; }
        public string NationalCode { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string UserRoleNames { get; set; }
        public bool Deleted { get; set; }
        public string Phone { get; set; }
        public string ZipPostalCode { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnString { get { return CreatedOn > DateTime.MinValue ? CreatedOn.ToString("g") : ""; } }
        public string Mobile { get; set; }
        public bool? ShahkarInquiry { get; set; }
        public string ShahkarResult => ShahkarInquiry.HasValue ? (ShahkarInquiry.Value ? "موفق" : "ناموفق") : null;
        public string OperationUnitCode { get; set; }
        public UserType? Type { get; set; }
        public UserJobType? JobType { get; set; }
    }

}
