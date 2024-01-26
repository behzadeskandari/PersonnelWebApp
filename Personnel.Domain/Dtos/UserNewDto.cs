using Personnel.Domain.Entities.Identity;
using Personnel.Domain.MapperProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos
{
    public class UserNewDto : BaseEntityDto, ICreateMapper<User>
    {


        public Guid UserGuid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// NationalCode is Username of user 
        /// </summary>
        public string NationalCode { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }

        //public string Password { get; set; }

        //public string PasswordSalt { get; set; }
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

        public UserLocationNewDto UserLocation { get; set; }
        public UserType? Type { get; set; }
        public UserJobType? JobType { get; set; }
        //public DateTime? SmsTokenExpirationDateTime { get; set; }

        //public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();

        public IList<UserInRoleNewDto> UserInRoles { get; set; }
    }

}
