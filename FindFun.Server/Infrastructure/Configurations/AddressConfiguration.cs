using FindFun.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFun.Server.Infrastructure.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Line).HasColumnName("line1");
        builder.Property(x => x.PostalCode).HasColumnName("postal_code");
        builder.Property(x => x.Coordinates)
            .HasColumnName("coordinates")
            .HasColumnType("geometry");

        builder.Property(x => x.StreetId).HasColumnName("street_id");
        builder.Property(x => x.Number).HasColumnName("number");

        builder.HasIndex(x => x.StreetId);

        builder.HasOne(x => x.Street)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.StreetId);
    }
}
