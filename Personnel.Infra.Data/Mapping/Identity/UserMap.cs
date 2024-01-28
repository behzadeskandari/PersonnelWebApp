using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Personnel.Infra.Data.Mapping.Identity
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "Identity");

            builder.HasKey(a => a.Id);
            builder
            .HasMany<SiteUserToken>(sut => sut.SiteUserTokens)
            .WithOne(u => u.User)
            .HasForeignKey(sut => sut.UserId);

            builder.Ignore(x => x.FullName);

            builder.Property(c => c.Mobile).HasMaxLength(15).IsRequired();
            builder.Property(c => c.OperationUnitCode).HasMaxLength(15).IsRequired();
            builder.Property(c => c.Phone).HasMaxLength(100).IsRequired();
            builder.Property(c => c.UserName).HasMaxLength(50).IsRequired();
            builder.Property(c => c.NationalCode).HasMaxLength(15).IsRequired();

            builder.Property(c => c.Password).HasMaxLength(450).IsRequired();
            builder.Property(c => c.PasswordSalt).HasMaxLength(50).IsRequired();

            builder.Property(c => c.Email).HasMaxLength(150);
            builder.Property(c => c.UrlReferrer).HasMaxLength(150);
            builder.Property(c => c.LastIpAddress).HasMaxLength(150);

            builder.Property(c => c.FirstName).HasMaxLength(100);
            builder.Property(c => c.LastName).HasMaxLength(100);

            builder.Property(e => e.SerialNumber).HasMaxLength(450);

            builder.Property(e => e.SmsToken).HasMaxLength(20);

            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}
