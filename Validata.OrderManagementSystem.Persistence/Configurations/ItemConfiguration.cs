using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence.Configurations;

public class ItemConfiguration : EntityTypeConfiguration<Item>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Item> builder)
    {
        builder.Property(x => x.ProductId)
            .HasColumnName("ProductId")
            .IsRequired();
        
        builder.Property(x => x.Quantity)
            .HasColumnName("Quantity")
            .IsRequired();
        
        builder.ToTable("Items");

        Relationships(builder);
    }

    private void Relationships(EntityTypeBuilder<Item> builder)
    {
        builder.HasOne(x=>x.Product)
            .WithMany(x=>x.Items)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(x=>x.ProductId);
    }
}