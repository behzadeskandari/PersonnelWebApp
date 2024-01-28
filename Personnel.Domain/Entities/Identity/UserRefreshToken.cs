using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class UserRefreshToken : BaseEntity<Guid>
    {
        public UserRefreshToken()
        {
            CreatedAt = DateTime.Now;
        }

        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsValid { get; set; }
        public bool Deleted { get; set; }
    }
}
