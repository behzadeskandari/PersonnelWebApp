using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Personnel.Domain.Security;
using Personnel.Infra.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Mapping.Identity
{
    public class PermissionRecordMap : BaseEntityTypeConfiguration<PermissionRecord>
    {
        public override void Configure(EntityTypeBuilder<PermissionRecord> builder)
        {
            builder.ToTable("PermissionRecord", "Security");

            builder.HasKey(a => a.Id);
        }
    }
}
