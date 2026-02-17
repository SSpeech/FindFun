using NetTopologySuite.Geometries;

namespace FindFun.Server.Domain;

public class Address
{
    public int Id { get; private set; }
    public string Line { get; private set; } = null!;
    public string PostalCode { get; private set; } = null!;

    public Point? Coordinates { get; private set; }
    public string Number { get; private set; }

    public Street? Street { get; private set; }
    public int StreetId { get; private set; }

    protected Address()
    {

    }

    public Address(
        string line1,
        string postalCode,
        Street street,
        double longitude,
        double latitude, string number)
    {

        Line = line1;
        PostalCode = postalCode;
        SetCoordinates(longitude, latitude);
        Number = number;
        SetStreet(street);
    }

    public void SetCoordinates(double longitude, double latitude, int srid = 4326)
    {
        Coordinates = new Point(longitude, latitude) { SRID = srid };
    }

    public void SetStreet(Street? street)
    {
        if (street is null)
        {
            Street = null;
            return;
        }

        Street = street;
        StreetId = street.Id;
        if (!street.Addresses.Contains(this))
            street.AddAddress(this);
    }

    public void SetStreetId(int streetId)
    {
        StreetId = streetId;
    }
}
