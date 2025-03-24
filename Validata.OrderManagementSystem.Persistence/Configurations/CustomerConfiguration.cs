using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence.Configurations;

public class CustomerConfiguration : EntityTypeConfiguration<Customer>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .IsRequired();
        
        builder.Property(x => x.Address)
            .HasColumnName("Address")
            .IsRequired();
        
        builder.Property(x => x.PostalCode)
            .HasColumnName("PostalCode")
            .IsRequired();
    }
}