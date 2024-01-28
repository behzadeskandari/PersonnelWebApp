using Personnel.Domain.Dtos.Employee;

namespace Personnel.Api.Models
{
    public class ProfileModel
    {
        public ProfileModel()
        {
            EmployeeDto = new EmployeeDto();
        }
        public EmployeeDto EmployeeDto { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Roles { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int UserId { get; set; }
        public bool IsAgentOrBroker { get; set; }
        public string ActionName { get; set; }
        public string Semat { get; set; }
    }

}
