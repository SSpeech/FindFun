using FindFun.Server.Domain;
using FluentAssertions;
using Xunit;

namespace FindFund.Server.UnitTest.Domain;

public class AmenityTests
{
    [Fact]
    public void Amenity_ShouldCreate_WhenPropertiesSet()
    {
        var amenity = new Amenity { Id = 1, Name = "Restroom", Description = "Public toilet" };

        amenity.Should().NotBeNull().And.BeOfType<Amenity>();
        amenity.Id.Should().Be(1);
        amenity.Name.Should().Be("Restroom");
        amenity.Description.Should().Be("Public toilet");
        amenity.ParkAmenities.Should().NotBeNull().And.BeAssignableTo<ICollection<ParkAmenity>>();
        amenity.ParkAmenities.Should().BeEmpty();
    }

    [Fact]
    public void ParkAmenities_ShouldAllowAddAndContain()
    {
        // Arrange
        var amenity = new Amenity { Id = 2, Name = "Bench", Description = "Seating" };
        var parkAmenity = new ParkAmenity { ParkId = 10, AmenityId = amenity.Id, Amenity = amenity, Park = null! };

        // Act
        amenity.ParkAmenities.Add(parkAmenity);
        amenity.ParkAmenities.Should().ContainSingle().Which.Should().Be(parkAmenity);
    }
}
