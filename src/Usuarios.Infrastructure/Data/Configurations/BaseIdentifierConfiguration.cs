using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Usuarios.Domain.Entities.Interfaces;

namespace Usuarios.Infrastructure.Data.Configurations;

public class BaseIdentifierConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IIdentifier
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
    }
}