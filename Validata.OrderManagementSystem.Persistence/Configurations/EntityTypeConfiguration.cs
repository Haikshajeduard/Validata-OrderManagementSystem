using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence.Configurations;

public abstract class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        BaseConfiguration(builder);
        ConfigureEntity(builder);
    }
    public void BaseConfiguration(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired()
            .HasDefaultValueSql("getdate()");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .IsRequired()
            .HasDefaultValueSql("getdate()");
    }
    protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
}