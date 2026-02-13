using FindFun.Server.Domain;
using FluentAssertions;
using Xunit;
using System.Linq;

namespace FindFund.Server.UnitTest;

public class ClosingScheduleTests
{
    [Fact]
    public void ClosingSchedule_ShouldContainEntries_WhenConstructed()
    {
        var entry1 = new ClosingScheduleEntry("Mon", "09:00", "17:00", false);
        var entry2 = new ClosingScheduleEntry("Tue", "09:00", "17:00", false);

        var schedule = new ClosingSchedule([entry1, entry2]);

        schedule.Entries.Should().HaveCount(2);
        schedule.Entries.Should().Contain(entry1).And.Contain(entry2);
    }

    [Fact]
    public void AddEntry_ShouldAddUniqueEntries()
    {
        var entry = new ClosingScheduleEntry("Mon", "09:00", "17:00", false);
        var schedule = new ClosingSchedule([entry]);

        schedule.AddEntry(entry);

        schedule.Entries.Should().HaveCount(1);

        var newEntry = new ClosingScheduleEntry("Tue", "09:00", "17:00", false);
        schedule.AddEntry(newEntry);
        schedule.Entries.Should().HaveCount(2).And.Contain(newEntry);
    }

    [Fact]
    public void SetPark_ShouldSetParkAndParkId()
    {
        var street = new Street("Main Street", 1);
        var address = new Address(line1: "123 Main St", postalCode: "00000", street, longitude: 10.0, latitude: 20.0, number: "1A");
        var park = new Park(name: "Central Park", description: "Nice park", address, entranceFee: 0m, isFree: true, organizer: "Org", parkType: "Urban", ageRecomandation: "all");
        var entry = new ClosingScheduleEntry("Mon", "09:00", "17:00", false);
        var schedule = new ClosingSchedule([entry]);

        schedule.SetPark(park);

        schedule.Park.Should().Be(park);
        schedule.ParkId.Should().Be(park.Id);
    }
}
