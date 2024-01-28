using Microsoft.AspNetCore.Identity;
using Personnel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class SiteUserToken : IdentityUserToken<int>, IEntity
    {
        public SiteUserToken()
        {
            GeneratedTime = DateTime.Now;
        }
        public int Id { get; set; }
        public User User { get; set; }
        public bool Deleted { get; set; }
        public DateTime GeneratedTime { get; set; }
    }
}
