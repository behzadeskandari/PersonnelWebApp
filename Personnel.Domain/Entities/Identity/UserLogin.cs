using Microsoft.AspNetCore.Identity;
using Personnel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Entities.Identity
{
    public class UserLogin : IdentityUserLogin<int>, IEntity
    {
        public UserLogin()
        {
            LoggedOn = DateTime.Now;
        }
        public int Id { get; set; }
        public Identity.User User { get; set; }
        public DateTime LoggedOn { get; set; }
        public bool Deleted { get; set; }
    }
}
