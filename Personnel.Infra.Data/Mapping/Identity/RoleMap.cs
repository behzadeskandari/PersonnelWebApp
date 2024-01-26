using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Personnel.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Mapping.Identity
{
    public class RoleMap : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles", "Identity");

            builder.HasKey(a => a.Id);

            builder.HasIndex(x => x.Name).IsUnique().HasName("IX_Name");

            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
            builder.Property(x => x.SystemName).HasMaxLength(150).IsRequired();
        }
    }
}
