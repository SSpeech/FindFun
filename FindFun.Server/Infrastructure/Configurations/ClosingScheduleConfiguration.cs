using FindFun.Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFun.Server.Infrastructure.Configurations;

public class ClosingScheduleConfiguration : IEntityTypeConfiguration<ClosingSchedule>
{
    public void Configure(EntityTypeBuilder<ClosingSchedule> builder)
    {
        builder.ToTable("closing_schedules");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ParkId)
            .HasColumnName("park_id");

        builder.HasIndex(x => x.ParkId);

        builder.HasOne(x => x.Park)
            .WithOne(x => x.ClosingSchedule)
            .HasForeignKey<ClosingSchedule>(x => x.ParkId)
            .IsRequired();

        builder.OwnsMany(x => x.Entries, entries =>
        {
            entries.ToTable("closing_schedule_entries");
            entries.WithOwner().HasForeignKey("closing_schedule_id");

            entries.Property<int>("id");
            entries.HasKey("id");

            entries.Property(x => x.Day)
                .HasColumnName("day");

            entries.Property(x => x.OpensAt)
                .HasColumnName("opens_at");

            entries.Property(x => x.ClosesAt)
                .HasColumnName("closes_at");

            entries.Property(x => x.IsClosed)
                .HasColumnName("is_closed");
        });
    }
}
