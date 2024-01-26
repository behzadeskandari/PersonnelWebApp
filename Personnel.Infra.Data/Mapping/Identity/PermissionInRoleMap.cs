using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personnel.Domain.Security;

namespace Personnel.Infra.Data.Mapping.Identity
{
    public class PermissionInRoleMap : IEntityTypeConfiguration<PermissionInRole>
    {
        public void Configure(EntityTypeBuilder<PermissionInRole> builder)
        {
            builder.ToTable("PermissionInRoles", "Security");
            builder.HasKey(x => new { x.PermissionRecordId, x.RoleId });

            builder.HasOne(x => x.Role).WithMany(x => x.PermissionInRoles).HasForeignKey(x => x.RoleId);
            builder.HasOne(x => x.PermissionRecord).WithMany(x => x.PermissionInRoles).HasForeignKey(x => x.PermissionRecordId);
        }
    }
}
