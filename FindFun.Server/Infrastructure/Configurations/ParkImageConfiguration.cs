using FindFun.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFun.Server.Infrastructure.Configurations;

public class ParkImageConfiguration : IEntityTypeConfiguration<ParkImage>
{
    public void Configure(EntityTypeBuilder<ParkImage> builder)
    {
        builder.ToTable("park_images");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(x => x.ParkId).HasColumnName("park_id");

        builder.Property(x => x.Url).HasColumnName("url").IsRequired();

        builder.HasOne(x => x.Park)
            .WithMany(p => p.Images)
            .HasForeignKey(x => x.ParkId)
            .IsRequired();
    }
}
