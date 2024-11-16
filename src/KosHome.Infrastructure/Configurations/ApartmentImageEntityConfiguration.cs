using KosHome.Domain.Entities.ApartmentImages;
using KosHome.Domain.ValueObjects.ApartmentImages;
using KosHome.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Configurations;

internal sealed class ImageConfiguration : IEntityTypeConfiguration<ApartmentImage>
{
    private const string TableName = "apartment_images";

    public void Configure(EntityTypeBuilder<ApartmentImage> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(image => image.Id);

        builder.Property(entity => entity.Id)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();
        
        builder.Property(image => image.ApartmentId)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(image => image.ImageUrl)
            .HasConversion(image => image.Value, value => new ImageUrl(value))
            .HasColumnName(nameof(ApartmentImage.ImageUrl))
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(image => image.IsPrimary)
            .IsRequired();

        builder.Property(image => image.CreatedAt)
            .IsRequired();
    }
}
