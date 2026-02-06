using FindFun.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFun.Server.Infrastructure.Configurations;

public class ParkConfiguration : IEntityTypeConfiguration<Park>
{
    public void Configure(EntityTypeBuilder<Park> builder)
    {
        builder.ToTable("parks");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name");
        builder.Property(x => x.Description)
            .HasColumnName("description");
        builder.Property(x => x.AddressId)
            .HasColumnName("address_id");

        builder.HasIndex(x => x.AddressId);

        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId)
            .IsRequired();
    }
}
