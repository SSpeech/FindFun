using FindFun.Server.Domain;
using FluentAssertions;


namespace FindFund.Server.UnitTest;

public class StreetTest
{
    [Theory]
    [MemberData(nameof(GetStreetTestData))]
    public void Street_ShouldBeCreate_WhenValidDataProvided(string name, Municipality municipality,Address address)
    {
        var street = new Street(name, municipality.Gid);
        street.AddAddress(address);
        street.Should().NotBeNull().And.BeOfType<Street>().And.Satisfy<Street>(s =>
        {
            s.Addresses.Contains(address);
            s.Addresses.Should().AllBeAssignableTo<Address>()
            .And.AllBeEquivalentTo(address);
            s.MunicipioGid.Should().Be(municipality.Gid);
        });
    }
    [Theory]
    [MemberData(nameof(GetStreetTestData))]
    public void Street_ShouldBeCreateWithoutAddress_WhenValidDataProvided(string name, Municipality municipality, Address address)
    {
        var street = new Street(name, municipality.Gid);
        street.Should().NotBeNull().And.BeOfType<Street>().And.Satisfy<Street>(s =>
        {
            s.Addresses.Should().BeEmpty();
            s.MunicipioGid.Should().Be(municipality.Gid);
        });
    }
    [Theory]
    [MemberData(nameof(GetStreetTestData))]
    public void Street_ShouldAddAddress_WhenValidDataProvided(string name, Municipality municipality, Address address)
    {
        var street = new Street(name, municipality.Gid);
        street.AddAddress(address);
        street.Addresses.Should().ContainSingle().Which.Should().BeEquivalentTo(address);
    }
    [Theory]
    [MemberData(nameof(GetStreetTestData))]
    public void Street_ShouldRemoveAddress_WhenValidDataProvided(string name, Municipality municipality, Address address)
    {
        var street = new Street(name, municipality.Gid);
        street.AddAddress(address);
        street.RemoveAddress(address);
        street.Addresses.Should().BeEmpty();
    }
    [Theory]
    [MemberData(nameof(GetStreetTestData))]
    public void Street_ShouldSetMunicipioId_WhenValidDataProvided(string name, Municipality municipality, Address address)
    {
        var street = new Street(name, municipality.Gid);
        var newMunicipality = Municipality.Create(
            gid: 2,
            year: "2024",
            officialCo: "SPF",
            officialNa: "Springfield",
            officialCo3: "SPF",
            officialNa4: "Springfield",
            officialCo5: "SPF",
            officialNa6: "Springfield",
            iso31663: "SPF",
            type: "City",
            localName: "Springfield",
            geometry: null!
        );
        street.SetMunicipioId(newMunicipality.Gid);
        street.MunicipioGid.Should().Be(newMunicipality.Gid);
    }
    public static TheoryData<string, Municipality, Address> GetStreetTestData()
    {
        var municipality = Municipality.Create(
            gid: 1,
            year: "2024",
            officialCo: "SPF",
            officialNa: "Springfield",
            officialCo3: "SPF",
            officialNa4: "Springfield",
            officialCo5: "SPF",
            officialNa6: "Springfield",
            iso31663: "SPF",
            type: "City",
            localName: "Springfield",
            geometry: null!
        );
        var street = new Street("Main Street", 1);
        var address = new Address(line1: "123 Main St", postalCode: "00000", street, longitude: 10.0, latitude: 20.0, number: "1A");
        return new TheoryData<string, Municipality, Address>
        {
            { "Main Street",municipality, address },
            { "Elm Street", municipality , address  },
            { "Oak Avenue", municipality, address }
        };
    }
}
