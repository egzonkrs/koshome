using KosHome.Domain.Entities.Users;
using KosHome.Domain.ValueObjects.Users;
using KosHome.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Configurations;

internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(entity => entity.Id)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();
        
        builder.Property(user => user.FirstName)
            .HasMaxLength(200)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value));

        builder.Property(user => user.LastName)
            .HasMaxLength(200)
            .HasConversion(firstName => firstName.Value, value => new LastName(value));

        builder.Property(user => user.Email)
            .HasMaxLength(400)
            .HasConversion(email => email.Value, value => new Email(value));

        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasIndex(user => user.IdentityId).IsUnique();
    }
}