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
    public class UserClaimMap : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserClaims)
                .HasForeignKey(uc => uc.UserId)
                .HasPrincipalKey(u => u.Id) // Assuming Id is the primary key of User
                .IsRequired(); // Make the relationship required

            // Apply the same global query filter
            builder.HasQueryFilter(u => !u.Deleted); // Apply the same global query filter
            // Other configurations...
        }
    }
}
