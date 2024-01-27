using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Personnel.Domain.Entities.Identity;
using Personnel.Infra.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Mapping.Identity
{
    public class UserTokenMap : BaseEntityTypeConfiguration<UserTokens>
    {
        public override void Configure(EntityTypeBuilder<UserTokens> builder)
        {

            builder.ToTable("UserTokens", "Identity");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.RefreshTokenIdHash).HasMaxLength(450).IsRequired();
            builder.Property(x => x.RefreshTokenIdHashSource).HasMaxLength(450).IsRequired();
            builder.HasOne(x => x.User).WithMany(x => x.UserTokens).HasForeignKey(ut => ut.UserId);
        }
    }
}
