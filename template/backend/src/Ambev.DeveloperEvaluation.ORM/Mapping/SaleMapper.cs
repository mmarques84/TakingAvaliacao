using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SaleMapper : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        // Definir a chave primária
        builder.HasKey(s => s.Id);

        // Definir o tipo de dado da chave primária UUID
        builder.Property(s => s.Id).HasColumnType("uuid");

        // Configurar as demais propriedades
        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasColumnType("bigint");


        builder.Property(s => s.TotalAmount)
            .IsRequired()
            .HasColumnType("numeric");


        // Relacionamento com o Cliente (Customer)
        builder.HasOne(s => s.Customer)
            .WithMany(c => c.Sales)  // Cliente pode ter várias vendas
            .HasForeignKey(s => s.IdCustomer)
            .OnDelete(DeleteBehavior.Cascade);  // Se o cliente for excluído, as vendas também serão

        // Relacionamento com a Filial (Branch)
        builder.HasOne(s => s.Branch)
            .WithMany(b => b.Sales)  // Filial pode ter várias vendas
            .HasForeignKey(s => s.IdBranch)
            .OnDelete(DeleteBehavior.Cascade);  // Se a filial for excluída, as vendas também serão

        // Configurar a lista de itens da venda (SaleItems)
        builder.HasMany(s => s.SaleItems) // Uma venda tem muitos itens
            .WithOne(si => si.Sale) // Cada item de venda pertence a uma venda
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);  // Se a venda for excluída, os itens de venda também serão
    }
}
