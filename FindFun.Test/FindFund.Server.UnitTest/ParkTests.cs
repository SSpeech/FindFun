using System.Linq;
using System.Collections.Generic;
using FindFun.Server.Domain;
using FluentAssertions;
using Xunit;

namespace FindFund.Server.UnitTest;

public class ParkTests
{
    [Theory]
    [InlineData("Central Park", "Nice park", 2.50, false, "Org", "Urban", "all")]
    [InlineData("Central Park", "Nice park", 0.00, true, "National Park Service", "Urban", "6-10")]
    public void User_ShouldCreate_WhenValidDataProvide(string name, string description, double entranceFee, bool isFree, string organizer, string parkType, string ageRecommendation)
    {
        // Arrange
        var street = new Street("Main Street", 1);
        var address = new Address("123 Main St", "00000", street, 10.0, 20.0, "1A");

        // Act
        var park = new Park(name, description, address, (decimal)entranceFee, isFree, organizer, parkType, ageRecommendation);
        // Assert
        park.Should().NotBeNull().And.BeOfType<Park>();
        park.Address.Should().BeOfType<Address>().And.NotBeNull().And.Be(address);
        park.AddressId.Should().BeGreaterThanOrEqualTo(0).And.Be(address.Id);
        park.Organizer.Should().NotBeNull().And.Be(organizer);
        park.ParkType.Should().NotBeNull().And.Be(parkType);
        park.AgeRecommendation.Should().NotBeNull().And.Be(ageRecommendation);
        park.IsFree.Should().Be(isFree);
        park.EntranceFee.Should().Be((decimal)entranceFee);
        park.Name.Should().Be(name);
        park.Description.Should().Be(description);
        park.ClosingSchedule.Should().BeNull();
        park.Amenities.Should().BeEmpty();
        park.Images.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(GetValidParkData))]
    public void ClosingSchedule_ShouldScheduleAndSetsPark_WhenValidDataProvide(string name, string description, double entranceFee, bool isFree, string organizer, string parkType, string ageRecommendation, Address address)
    {
        // Arrange
        var park = new Park(name: name, description: description, address, (decimal)entranceFee, isFree, organizer, parkType, ageRecommendation);
        var entry = new ClosingScheduleEntry("Mon", "09:00", "17:00", false);
        var schedule = new ClosingSchedule([entry]);

        // Act
        park.SetClosingSchedule(schedule);

        // Assert
        schedule.Entries.Should().ContainSingle().Which.Should().BeEquivalentTo(entry);
        schedule.Park.Should().NotBeNull().And.BeOfType<Park>().And.Be(park);
        park.ClosingSchedule.Should().NotBeNull().And.BeOfType<ClosingSchedule>().And.Be(schedule);
        schedule.ParkId.Should().Be(park.Id);
    }
    [Theory]
    [MemberData(nameof(GetValidParkData))]
    public void ClosingSchedule_ShouldBeNull_WhenClear(string name, string description, double entranceFee, bool isFree, string organizer, string parkType, string ageRecommendation, Address address)
    {
        // Arrange
        var park = new Park(name: name, description: description, address, (decimal)entranceFee, isFree, organizer, parkType, ageRecommendation);
        var entry = new ClosingScheduleEntry("Mon", "09:00", "17:00", false);
        var schedule = new ClosingSchedule([entry]);

        // Act
        park.SetClosingSchedule(schedule);

        // Assert
        schedule.Entries.Should().ContainSingle().Which.Should().BeEquivalentTo(entry);
        schedule.Park.Should().NotBeNull().And.BeOfType<Park>().And.Be(park);
        park.ClosingSchedule.Should().NotBeNull().And.BeOfType<ClosingSchedule>().And.Be(schedule);
        schedule.ParkId.Should().Be(park.Id);

        park.ClearClosingSchedule();
        park.ClosingSchedule.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(GetValidParkData))]
    public void AddAmenity_AddsParkAmenity_ShouldAddAmenity_WhenValidDataProvide(string name, string description, double entranceFee, bool isFree, string organizer, string parkType, string ageRecommendation, Address address)
    {
        // Arrange
        var park = new Park(name, description, address, (decimal)entranceFee, isFree, organizer, parkType, ageRecommendation);
        var amenity = new Amenity { Name = "Playground", Description = "For kids" };

        // Act
        park.AddAmenity(amenity);

        // Assert
        park.Amenities.Should().ContainSingle().Which.Should().BeOfType<ParkAmenity>().Which.Should().Satisfy<ParkAmenity>(parkAmenity =>
        {
            parkAmenity.Amenity.Should().NotBeNull().And.BeOfType<Amenity>().And.Be(amenity);
            parkAmenity.Park.Should().NotBeNull().And.BeOfType<Park>().And.Be(park);
        });
    }
    [Theory]
    [MemberData(nameof(GetValidParkData))]
    public void ClearAmenities_ShouldClearAmenities_WhenValidDataProvides(string name, string description, double entranceFee, bool isFree, string organizer, string parkType, string ageRecommendation, Address address)
    {
        // Arrange
        var park = new Park(name, description, address, (decimal)entranceFee, isFree, organizer, parkType, ageRecommendation);
        var amenity = new Amenity { Name = "Playground", Description = "For kids" };

        // Act
        park.AddAmenity(amenity);

        // Assert
        park.Amenities.Should().ContainSingle().Which.Should().BeOfType<ParkAmenity>().Which.Should().Satisfy<ParkAmenity>(parkAmenity =>
        {
            parkAmenity.Amenity.Should().NotBeNull().And.BeOfType<Amenity>().And.Be(amenity);
            parkAmenity.Park.Should().NotBeNull().And.BeOfType<Park>().And.Be(park);
        });

        // Act - clear
        park.ClearAmenities();

        // Assert cleared
        park.Amenities.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(GetValidParkData))]
    public void ParkImages_ShouldbeAdded_WhenAddImages(string name, string description, double entranceFee, bool isFree, string organizer, string parkType, string ageRecommendation, Address address)
    {
        // Arrange

        var park = new Park(name, description, address, (decimal)entranceFee, isFree, organizer, parkType, ageRecommendation);
        var img1 = new ParkImage("https://example.com/1.jpg");
        var img2 = new ParkImage("https://example.com/2.jpg");

        park.AddImages(images: null);

        // Assert no images
        park.Images.Should().BeEmpty();

        // Act - add images
        park.AddImages([img1, img2]);

        // Assert
        Assert.Equal(2, park.Images.Count);
        park.Images.Should().HaveCount(2).And.Contain(img1).And.Contain(img2);

    }
    [Theory]
    [MemberData(nameof(GetValidParkData))]
    public void ParkImages_ShouldBeNull_WhenClearImages_Clears(string name, string description, double entranceFee, bool isFree, string organizer, string parkType, string ageRecommendation, Address address)
    {
        // Arrange

        var park = new Park(name, description, address, (decimal)entranceFee, isFree, organizer, parkType, ageRecommendation);
        var img1 = new ParkImage("https://example.com/1.jpg");
        var img2 = new ParkImage("https://example.com/2.jpg");

        park.AddImages(images: null);

        // Assert no images
        park.Images.Should().BeNullOrEmpty();

        // Act - add images
        park.AddImages([img1, img2]);

        // Assert
        Assert.Equal(2, park.Images.Count);
        park.Images.Should().HaveCount(2).And.Contain(img1).And.Contain(img2);

        // Act - clear
        park.ClearImages();

        // Assert cleared
        park.Images.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(GetValidParkData))]
    public void ParkImages_ShouldBeNull_WhenNoImagesProvided(string name, string description, double entranceFee, bool isFree, string organizer, string parkType, string ageRecommendation, Address address)
    {
        // Arrange

        var park = new Park(name, description, address, (decimal)entranceFee, isFree, organizer, parkType, ageRecommendation);
        var img1 = new ParkImage("https://example.com/1.jpg");
        var img2 = new ParkImage("https://example.com/2.jpg");

        park.AddImages(images: null);

        // Assert no images
        park.Images.Should().BeNullOrEmpty();

    }
    public static TheoryData<string, string, double, bool, string, string, string,Address> GetValidParkData()
    {
       var street =  new Street("Main Street", 1);
        var address = new Address(line1: "123 Main St", postalCode: "00000", street, longitude: 10.0, 20.0, number: "1A");
        return new TheoryData<string, string, double, bool, string, string, string, Address>
        {
            { "Central Park", "Nice park", 2.50, false, "Org", "Urban", "all", address},
            { "Central Park", "Nice park", 0.00, true, "National Park Service", "Urban", "6-10", address }
        };
    }
}
