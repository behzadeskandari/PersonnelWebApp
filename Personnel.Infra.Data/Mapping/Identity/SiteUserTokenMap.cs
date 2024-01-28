using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personnel.Domain.Entities.Identity;

namespace Personnel.Infra.Data.Mapping.Identity
{
    
    internal class SiteUserTokenMap : IEntityTypeConfiguration<SiteUserToken>
    {
        public void Configure(EntityTypeBuilder<SiteUserToken> builder)
        {

            builder.HasOne(u => u.User).WithMany(u => u.SiteUserTokens).HasForeignKey(u => u.UserId);
            builder.ToTable("SiteUserTokens", "Identity");
            builder.HasQueryFilter(u => !u.Deleted);
        }
    }
}
