using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Personnel
{
    public class PersonelListDto : BaseEntityDto
    {

        public string AdminComment { get; set; }
        public string Email { get; set; }
        public string NationalCode { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string UserRoleNames { get; set; }
        public bool Deleted { get; set; }
        public string Phone { get; set; }
        public string ZipPostalCode { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnString { get { return CreatedOn > DateTime.MinValue ? CreatedOn.ToString("g") : ""; } }
        public string Mobile { get; set; }

        public string OperationUnitCode { get; set; }
    }
}
