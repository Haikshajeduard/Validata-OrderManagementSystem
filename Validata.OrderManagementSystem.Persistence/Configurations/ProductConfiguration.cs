using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence.Configurations;

public class ProductConfiguration : EntityTypeConfiguration<Product>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Product> builder)
    {
        //set identity off for product
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .IsRequired();
        
        builder.Property(x => x.Price)
            .HasColumnName("Price")
            .IsRequired();
        
        builder.ToTable("Products");
    }
}