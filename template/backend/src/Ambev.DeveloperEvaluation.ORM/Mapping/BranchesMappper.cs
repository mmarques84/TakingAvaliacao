using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class BranchesMappper : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasColumnType("uuid");

            builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Description).IsRequired().HasMaxLength(500);
            builder.Property(b => b.Document).IsRequired().HasMaxLength(200);
            builder.Property(b => b.Active).IsRequired();
            builder.Property(b => b.CreatedAt).HasColumnType("timestamp with time zone");
        }

    }
}
