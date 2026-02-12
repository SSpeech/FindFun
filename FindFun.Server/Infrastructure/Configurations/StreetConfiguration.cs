using FindFun.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFun.Server.Infrastructure.Configurations;

public class StreetConfiguration : IEntityTypeConfiguration<Street>
{
    public void Configure(EntityTypeBuilder<Street> builder)
    {
        builder.ToTable("streets");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.MunicipioGid).HasColumnName("municipio_gid");

        builder.HasIndex(x => x.MunicipioGid);
        builder.HasIndex(x => new { x.Name, x.MunicipioGid }).IsUnique();

        builder.HasOne(x => x.Municipio)
            .WithMany(x => x.Streets)
            .HasForeignKey(x => x.MunicipioGid);
    }
}
