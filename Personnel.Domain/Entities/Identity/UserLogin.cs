﻿using Microsoft.AspNetCore.Identity;
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

        public Identity.User User { get; set; }
        public DateTime LoggedOn { get; set; }
    }
}