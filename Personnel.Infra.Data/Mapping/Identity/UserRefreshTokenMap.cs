using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Mapping.Identity
{
    public class UserRefreshTokenMap : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("UserRefreshTokens", "Identity");
            builder.HasOne(c => c.User).WithMany(c => c.UserRefreshTokens).HasForeignKey(c => c.UserId);
            builder.HasQueryFilter(u => !u.Deleted);
        }
    }
}
