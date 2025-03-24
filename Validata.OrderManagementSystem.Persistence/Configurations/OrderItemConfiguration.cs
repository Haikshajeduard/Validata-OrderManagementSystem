using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence.Configurations;

public class OrderItemConfiguration : EntityTypeConfiguration<OrderItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.ItemId)
            .IsRequired();

        builder.ToTable("OrderItems");

        Relationships(builder);
    }
    private void Relationships(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(x => x.OrderId);

        builder.HasOne(x => x.Item)
            .WithMany(x => x.OrderItems)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(x => x.ItemId);
    }
}