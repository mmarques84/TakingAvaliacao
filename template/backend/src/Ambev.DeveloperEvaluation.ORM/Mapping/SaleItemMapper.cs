using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemMapper : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SalesItems");

            builder.HasKey(si => si.Id);
            builder.Property(si => si.Id).HasColumnType("uuid");

            builder.Property(si => si.Quantity).IsRequired().HasColumnType("integer");
            builder.Property(si => si.UnitPrice).IsRequired().HasColumnType("numeric");
            builder.Property(si => si.Discount).IsRequired().HasColumnType("numeric");
            builder.Property(si => si.Active).IsRequired();
            builder.Property(si => si.CreatedAt).HasColumnType("timestamp with time zone");

            // Relacionamento com Sales
            builder.HasOne(si => si.Sale)
                .WithMany(s => s.SaleItems)
                .HasForeignKey(si => si.SaleId);

            // Relacionamento com Products
            builder.HasOne(si => si.Product)
                .WithMany(p => p.SaleItems)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
