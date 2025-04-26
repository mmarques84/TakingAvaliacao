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
    public class SaleMapper : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnType("uuid");

            builder.Property(s => s.SaleNumber).IsRequired().HasColumnType("bigint");
            builder.Property(s => s.SaleDate).IsRequired().HasColumnType("timestamp with time zone");
            builder.Property(s => s.TotalAmount).IsRequired().HasColumnType("numeric");
            builder.Property(s => s.IsCancelled).IsRequired();
            builder.Property(s => s.Active).IsRequired();
            builder.Property(s => s.CreatedAt).HasColumnType("timestamp with time zone");

            // Definir os relacionamentos (foreign keys)
            builder.HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.IdCustomer)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Branch)
                .WithMany(b => b.Sales)
                .HasForeignKey(s => s.IdBranch)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
