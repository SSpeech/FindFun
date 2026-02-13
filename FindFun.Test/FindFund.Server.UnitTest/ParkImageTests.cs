using FindFun.Server.Domain;
using FluentAssertions;
using Xunit;

namespace FindFund.Server.UnitTest;

public class ParkImageTests
{
    [Fact]
    public void ParkImage_ShouldSetUrl_WhenConstructed()
    {
        var img = new ParkImage("https://example.com/1.jpg");
        img.Should().NotBeNull().And.BeOfType<ParkImage>();
        img.Url.Should().Be("https://example.com/1.jpg");
    }

    [Fact]
    public void ParkImage_SetPark_ShouldSetParkAndParkId()
    {
        var street = new Street("Main Street", 1);
        var address = new Address(line1: "123 Main St", postalCode: "00000", street, longitude: 10.0, latitude: 20.0, number: "1A");
        var park = new Park(name: "Central Park", description: "Nice park", address, entranceFee: 0m, isFree: true, organizer: "Org", parkType: "Urban", ageRecomandation: "all");
        var img = new ParkImage("https://example.com/2.jpg");

        img.SetPark(park);

        img.Park.Should().Be(park);
        img.ParkId.Should().Be(park.Id);
    }
}
