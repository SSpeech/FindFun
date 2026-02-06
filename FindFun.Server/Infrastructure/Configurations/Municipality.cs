using FindFun.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFun.Server.Infrastructure.Configurations;

public class GeorefSpainMunicipioMillesimeConfiguration : IEntityTypeConfiguration<Municipality>
{
    public void Configure(EntityTypeBuilder<Municipality> builder)
    {
        builder.ToTable("georef-spain-municipio-millesime", "public");

        builder.HasKey(x => x.Gid);
        builder.Property(x => x.Gid).HasColumnName("gid");

        builder.Property(x => x.Year)
            .HasColumnName("year")
            .HasMaxLength(255);

        builder.Property(x => x.OfficialCo)
            .HasColumnName("official_co")
            .HasMaxLength(255);

        builder.Property(x => x.OfficialNa)
            .HasColumnName("official_na")
            .HasMaxLength(255);

        builder.Property(x => x.OfficialCo3)
            .HasColumnName("official_co__3")
            .HasMaxLength(255);

        builder.Property(x => x.OfficialNa4)
            .HasColumnName("official_na_4")
            .HasMaxLength(255);

        builder.Property(x => x.OfficialCo5)
            .HasColumnName("official_co__5")
            .HasMaxLength(255);

        builder.Property(x => x.OfficialNa6)
            .HasColumnName("official_na__6")
            .HasMaxLength(255);

        builder.Property(x => x.Iso31663)
            .HasColumnName("iso_3166_3")
            .HasMaxLength(255);

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasMaxLength(255);

        builder.Property(x => x.LocalName)
            .HasColumnName("local_name_")
            .HasMaxLength(255);

        builder.Property(x => x.Geometry)
            .HasColumnName("geom")
            .HasColumnType("geometry(MultiPolygon,4326)");

        builder.HasMany(x => x.Streets)
            .WithOne(x => x.Municipio)
            .HasForeignKey(x => x.MunicipioGid);
    }
}
