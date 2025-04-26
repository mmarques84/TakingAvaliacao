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
    public class CustomerMapper : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnType("uuid");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(100);
            builder.Property(c => c.City).IsRequired().HasMaxLength(100);
            builder.Property(c => c.BirthDate).HasColumnType("timestamp with time zone").IsRequired();
            builder.Property(c => c.Active).IsRequired();
            builder.Property(c => c.CreatedAt).HasColumnType("timestamp with time zone");
        }
    }
}
