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
    public class ProductMapper : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("uuid");

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.UnitPrice).IsRequired().HasColumnType("numeric");
            builder.Property(p => p.Active).IsRequired();
            builder.Property(p => p.CreatedAt).HasColumnType("timestamp with time zone");
        }
    }
}
