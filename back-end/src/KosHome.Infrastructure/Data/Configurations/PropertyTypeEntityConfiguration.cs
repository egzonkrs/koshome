using KosHome.Domain.Entities.PropertyTypes;
using KosHome.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Data.Configurations;

internal sealed class PropertyTypeConfiguration : IEntityTypeConfiguration<PropertyType>
{
    private const string TableName = "koshome_property_types";
    
    public void Configure(EntityTypeBuilder<PropertyType> builder)
    {
        builder.ToTable(TableName);
        
        builder.HasKey(pt => pt.Id);

        builder.Property(entity => entity.Id)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(pt => pt.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(pt => pt.Description)
            .HasMaxLength(255);

        builder.Property(pt => pt.CreatedAt)
            .IsRequired();

        builder.Property(pt => pt.UpdatedAt);
        
        // Add a unique index on Name
        builder.HasIndex(pt => pt.Name)
            .IsUnique();
            
        // Seed initial data
        SeedData(builder);
    }
    
    private static void SeedData(EntityTypeBuilder<PropertyType> builder)
    {
        var apartment = PropertyType.Create("Apartment", "A residential apartment.");
        var house = PropertyType.Create("House", "A residential house.");
        var commercial = PropertyType.Create("Commercial", "A commercial property.");
        var land = PropertyType.Create("Land", "A land plot.");
        
        builder.HasData(apartment, house, commercial, land);
    }
} 