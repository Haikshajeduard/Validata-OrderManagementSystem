using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence.Configurations;

public class OrderConfiguration : EntityTypeConfiguration<Order>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.CustomerId)
            .HasColumnName("CustomerId")
            .IsRequired();
        
        builder.Property(x=>x.OrderDate)
            .HasColumnName("OrderDate")
            .ValueGeneratedOnUpdate()
            .IsRequired();
        
        builder.Property(x => x.TotalPrice)
            .HasColumnName("TotalPrice")
            .IsRequired();
        
        builder.ToTable("Orders");
    }

    private void Relationships(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(x => x.OrderId);
    }
}