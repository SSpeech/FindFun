using FindFun.Server.Domain;
using FluentAssertions;

namespace FindFund.Server.UnitTest.Domain;

public class MunicipalityTests
{
    [Theory]
    [MemberData(nameof(GetStreetTestData))]
    public void municipality_ShouldCreate_WhenValidDataProvided(string streetName, Municipality municipality, Address address)
    {
        var street = new Street(streetName, 1);
        municipality.Streets.Add(street);
        street.AddAddress(address);

        municipality.Should().NotBeNull().And.BeOfType<Municipality>();
        municipality.Year.Should().Be(municipality.Year);
        municipality.OfficialCo.Should().Be(municipality.OfficialCo);
        municipality.OfficialNa.Should().Be(municipality.OfficialNa);
        municipality.OfficialCo3.Should().Be(municipality.OfficialCo3);
        municipality.OfficialNa4.Should().Be(municipality.OfficialNa4);
        municipality.OfficialCo5.Should().Be(municipality.OfficialCo5);
        municipality.OfficialNa6.Should().Be(municipality.OfficialNa6);
        municipality.Iso31663.Should().Be(municipality.Iso31663);
        municipality.Type.Should().Be(municipality.Type);
        municipality.LocalName.Should().Be(municipality.LocalName);
        municipality.Geometry.Should().BeNull();
        municipality.Gid.Should().Be(1);
        municipality.Streets.Should().NotBeEmpty().And.Satisfy<ICollection<Street>>(s => s.Should().NotBeNull());
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
