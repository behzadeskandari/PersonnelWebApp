using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            UserGuid = Guid.NewGuid();
            CreatedOn = DateTime.Now;
        }
        public Guid UserGuid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// NationalCode is Username of user 
        /// </summary>
        public string NationalCode { get; set; }

        //public string Email { get; set; }

        public string Mobile { get; set; }
        public string Phone { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }


        /// <summary>
        /// every time the user changes his Password,
        /// or an admin changes his Roles or stat/IsActive,
        /// create a new `SerialNumber` GUID and store it in the DB.
        /// </summary>
        public string SerialNumber { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public int NumberOfFailedLoginAttempts { get; set; }

        public string LastIpAddress { get; set; }

        public string UrlReferrer { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public DateTime? LastUpdateCartDate { get; set; }

        public DateTime? PasswordChangeDate { get; set; }

        public DateTime? CannotLoginUntilDate { get; set; }

        public string OperationUnitCode { get; set; }

        public string SmsToken { get; set; }

        public DateTime? Birthday { get; set; }

        public int? UserLocationId { get; set; }
        public bool? ShahkarInquiry { get; set; }
        public bool? IsSiteEmployee { get; set; }

        public UserLocation UserLocation { get; set; }
        public UserType? Type { get; set; }
        public UserJobType? JobType { get; set; }
        public DateTime? SmsTokenExpirationDateTime { get; set; }

        public virtual ICollection<UserTokens> UserTokens { get; set; } = new List<UserTokens>();
        public virtual ICollection<SiteUserToken> SiteUserTokens { get; set; } = new List<SiteUserToken>();

        public ICollection<UserInRole> UserInRoles { get; set; } = new List<UserInRole>();
        public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }

    }



    public enum UserJobType
    {
        [Description("کارمند سطح یک")]
        EmployeeOne = 1,
        [Description("کارمند سطح 2")]
        EmployeeTwo = 2,
        [Description("نماینده عمومی")]
        UserAgent = 3,
        [Description("ادمین")]
        Admin = 4,
        [Description("کارگزار")]
        Broker = 5,
        [Description("مشاور")]
        Consultant = 6,
    }

    public enum UserType : byte
    {
        [Description("حقیقی")]
        Real = 1,
        [Description("حقوقی")]
        Legal = 2
    }

}
