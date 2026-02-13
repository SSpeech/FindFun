using Azure.Storage.Blobs.Models;
using FindFun.Server.Domain;
using FluentAssertions;
using NetTopologySuite.Geometries;

namespace FindFund.Server.UnitTest;

public class AddressTests
{
    [Theory]
    [MemberData(nameof(GetAddressTestData))]
    public void Address_ShouldBeCreate_WhenValidDataProvided(Street street, double latitude, double longitude)
    {
        var address = new Address(line1: "123 Main St", postalCode: "10001", street,longitude,latitude,number: "123");

        address.Should().NotBeNull().And.BeOfType<Address>();

        address.Coordinates.Should().NotBeNull();
        address.Coordinates.X.Should().Be(longitude);
        address.Coordinates.Y.Should().Be(latitude);

        address.Line.Should().Be("123 Main St");
        address.PostalCode.Should().Be("10001");
        address.Number.Should().Be("123");

    }
    [Theory]
    [MemberData(nameof(GetAddressTestData))]
    public void AddressSetStreet_ShouldBeSetWhenValidDataProvided( Street street, double latitude, double longitude)
    {
        var address =  new Address(line1: "Main street", postalCode: "10001", null!, longitude, latitude, number: "123");
        address.Street.Should().BeNull();
        address.SetStreet(street);
        address.Street.Should().NotBeNull().And.BeOfType<Street>().Which.Should().Be(street);
        address.Coordinates.Should().NotBeNull().And.BeOfType<Point>().And.Satisfy<Point>(p =>
        { p.X.Should().Be(longitude);
          p.Y.Should().Be(latitude);
        });
    }
    [Theory]
    [MemberData(nameof(GetAddressTestData))]
    public void AddressStreet_ShouldBeSetNullWhenNullDataProvided( Street street, double latitude, double longitude)
    {
        var address =  new Address(line1: "Main street", postalCode: "10001", street!, longitude, latitude, number: "123");
        address.Street.Should().NotBeNull();
        address.SetStreet(null);
        address.Street.Should().BeNull();
    }
    [Theory]
    [MemberData(nameof(GetAddressTestData))]
    public void AddressStreetId_ShouldBeSetWhenValidDataProvided(Street street, double latitude, double longitude)
    {
        var address = new Address(line1: "Main street", postalCode: "10001", street!, longitude, latitude, number: "123");
        address.Street.Should().NotBeNull();
        address.SetStreet(null);
        address.SetStreetId(street.Id);
    }
    [Theory]
    [MemberData(nameof(GetAddressTestData))]
    public void AddressCoordinates_ShouldBeSetWhenValidDataProvided( Street street, double latitude, double longitude)
    {
        var address =  new Address(line1: "Main street", postalCode: "10001", street!, longitude:0, latitude:0, number: "123");
        address.Street.Should().NotBeNull();
        address.SetStreet(null);
        address.SetStreetId(street.Id);
        address.StreetId.Should().Be(street.Id);
        address.Coordinates.Should().NotBeNull().And.BeOfType<Point>().And.Satisfy<Point>(p =>
        {
            p.X.Should().Be(0);
            p.Y.Should().Be(0);
        });
        address.SetCoordinates(longitude, latitude);
        address.Coordinates.Should().NotBeNull().And.BeOfType<Point>().And.Satisfy<Point>(p =>
        {
            p.X.Should().Be(longitude);
            p.Y.Should().Be(latitude);
        });
    }
    public static TheoryData<Street,double,double> GetAddressTestData()
    {
        var latitude = 40.7128;
        var longitude = -74.0060;
        var geometry = new Point(40.7128, -74.0060);
        var municipality = Municipality.Create(
                gid: 1,
                year: "2024",
                officialCo: "NYC",
                officialNa: "New York City",
                officialCo3: "NYC3",
                officialNa4: "New York City 4",
                officialCo5: "NYC5",
                officialNa6: "New York City 6",
                iso31663: "US-NY",
                type: "City",
                localName: "New York",
                geometry: geometry
            );
            var street = new Street(name: "Main Street", municipalityId: municipality.Gid);
            return  new TheoryData< Street, double, double> { { street, latitude, longitude } };
    }
}
