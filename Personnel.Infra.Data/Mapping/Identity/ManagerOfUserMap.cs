using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Personnel.Domain.Identity;
using Personnel.Infra.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Mapping.Identity
{
    public class ManagerOfUserMap : BaseEntityTypeConfiguration<ManagerOfUser>
    {
        public override void Configure(EntityTypeBuilder<ManagerOfUser> builder)
        {
            builder.ToTable("ManagerOfUser", "Identity");
        }
    }
}
