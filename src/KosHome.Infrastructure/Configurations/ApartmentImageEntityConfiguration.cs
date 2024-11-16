using KosHome.Domain.Entities.ApartmentImages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Configurations;

internal sealed class ImageConfiguration : IEntityTypeConfiguration<ApartmentImage>
{
    public void Configure(EntityTypeBuilder<ApartmentImage> builder)
    {
        builder.HasKey(image => image.Id);

        builder.Property(image => image.ApartmentId)
            .IsRequired();

        builder.OwnsOne(image => image.ImageUrl, imageUrlBuilder =>
        {
            imageUrlBuilder.Property(iu => iu.Value)
                .HasMaxLength(255);
        });

        builder.Property(image => image.IsPrimary)
            .IsRequired();

        builder.Property(image => image.CreatedAt)
            .IsRequired();
    }
}
