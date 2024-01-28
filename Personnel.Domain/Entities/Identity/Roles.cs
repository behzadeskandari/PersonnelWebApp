using Microsoft.AspNetCore.Identity;
using Personnel.Domain.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personnel.Domain.Core.Extensions;

namespace Personnel.Domain.Entities.Identity
{
    public class Roles : IdentityRole<int>
    {

        public bool Active { get; set; }

        public bool IsSystemRole { get; set; }

        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Users must change passwords after a specified time
        /// </summary>
        public bool EnablePasswordLifeTime { get; set; }

        public RoleGroup RoleGroup { get; set; }
        public string RoleGroupString => RoleGroup.Description();
        public ICollection<UserInRole> UserInRoles { get; set; } = new List<UserInRole>();

        public ICollection<PermissionInRole> PermissionInRoles { get; set; } = new List<PermissionInRole>();
    }

    public enum RoleGroup
    {
        [Description("بدون گروه")]
        All = 0,
        [Description("ارتباط سازمانی")]
        Relation = 1,
        [Description("از لحاظ پست سازمانی")]
        Post = 2,
        [Description("محل خدمت")]
        Location = 3,
        [Description("ساختار سازمانی")]
        Organization = 4,
    }
}
