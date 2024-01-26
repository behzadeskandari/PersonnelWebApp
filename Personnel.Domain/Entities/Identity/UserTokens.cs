using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class UserTokens : BaseEntity
    {
        public User User { get; set; }

        public int UserId { get; set; }

        public string AccessTokenHash { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public string RefreshTokenIdHashSource { get; set; }

        public DateTime AccessTokenExpiresDateTime { get; set; }

        public DateTime RefreshTokenExpiresDateTime { get; set; }
    }
}
