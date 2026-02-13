using FindFun.Server.Domain;
using FluentAssertions;
using Xunit;

namespace FindFund.Server.UnitTest;

public class ParkAmenityTests
{
    [Fact]
    public void ParkAmenity_ShouldHoldReferences_WhenAssigned()
    {

        var street = new Street("Main Street", 1);
        var address = new Address(line1: "123 Main St", postalCode: "00000", street, longitude: 10.0, latitude: 20.0, number: "1A");
        var park = new Park(name: "Central Park", description: "Nice park", address, entranceFee: 0m, isFree: true, organizer: "Org", parkType: "Urban", ageRecomandation: "all");
        var amenity = new Amenity { Name = "Playground", Description = "For kids" };

        var pa = new ParkAmenity { Park = park, Amenity = amenity, ParkId = park.Id, AmenityId = amenity.Id };

        pa.Park.Should().Be(park);
        pa.Amenity.Should().Be(amenity);
        pa.ParkId.Should().Be(park.Id);
        pa.AmenityId.Should().Be(amenity.Id);
    }
}
