using Microsoft.AspNetCore.Mvc.Rendering;
using Personnel.Domain.Dtos.Users;
using Personnel.Domain.Entities.Identity;
using Personnel.Domain.MapperProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Personnel.Api.Models
{
    public class UserCreateUpdateModel : BaseAdminModel, ICreateMapper<UserNewDto>
    {
        public UserCreateUpdateModel()
        {

            SelectedUserRoleIds = new List<int>();
            AvailableUserRoles = new List<SelectListItem>();
            AvailableLocation = new List<SelectListItem>();
            AvailableJobTypes = new List<SelectListItem>();
            AvailableUserTypes = new List<SelectListItem>();
        }


        [DisplayName("نام کاربری")]
        public string Username { get; set; }
        [DisplayName("شماره موبایل")]
        public string Mobile { get; set; }
        [DisplayName("تلفن")]
        public string Phone { get; set; }
        [DisplayName("ایمیل")]
        public string Email { get; set; }
        [DisplayName("کلمه عبور")]
        public string Password { get; set; }
        [DisplayName("کد ملی")]
        public string NationalCode { get; set; }
        [DisplayName("نام")]
        public string FirstName { get; set; }
        [DisplayName("نام خانوادگی")]
        public string LastName { get; set; }
        [DisplayName("نام و نام خانوادگی")]
        public string FullName { get; set; }
        [DisplayName("کد نمایندگی/ پرسنلی")]
        public string OperationUnitCode { get; set; }
        [DisplayName("نقش ها")]
        public string AdminComment { get; set; }
        [DisplayName("تاریخ تولد")]
        public DateTime? Birthday { get; set; }
        [DisplayName("فعال")]
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        //registration date
        [DisplayName("تاریخ ایجاد")]
        public DateTime CreatedOn { get; set; }

        //User roles
        [DisplayName("نقش های کاربر")]

        public IList<SelectListItem> AvailableUserRoles { get; set; }
        [DisplayName("نقش های کاربر")]
        //[UIHint("MultiSelect")]
        public IList<int> SelectedUserRoleIds { get; set; }

        [Display(Name = "نقش")]
        public int? Role { get; set; }

        [DisplayName("محل کار کاربر")]
        public int? UserLocationId { get; set; }
        public IList<SelectListItem> AvailableLocation { get; set; }
        [Display(Name = "نوع شخص")]
        public UserType? Type { get; set; }
        [Display(Name = "نوع شغل")]

        public UserJobType? JobType { get; set; }
        public IList<SelectListItem> AvailableUserTypes { get; set; }
        public IList<SelectListItem> AvailableJobTypes { get; set; }

    }
}
