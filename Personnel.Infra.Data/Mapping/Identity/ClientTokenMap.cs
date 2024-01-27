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
    public class ClientTokenMap : BaseEntityTypeConfiguration<ClientToken>
    {
        public override void Configure(EntityTypeBuilder<ClientToken> builder)
        {
            builder.ToTable("ClientToken", "Identity");
            builder.HasKey(a => a.Id);
            builder.Property(c => c.Mobile).HasMaxLength(15);
            builder.Property(c => c.NationalCode).HasMaxLength(15);
        }
    }
}
