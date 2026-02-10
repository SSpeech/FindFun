namespace FindFun.Server.Domain;
using System.Collections.Generic;

public class Park
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public Address Address { get; private set; } = null!;
    public int AddressId { get; private set; }

    public ClosingSchedule? ClosingSchedule { get; private set; }

    private readonly List<ParkAmenity> _amenities = [];
    public IReadOnlyCollection<ParkAmenity> Amenities => _amenities;

    protected Park()
    {
    }

    public Park(string name, string description, Address address)
    {
        Name = name;
        Description = description;
        Address = address;
        AddressId = address.Id;
    }

    public void SetClosingSchedule(ClosingSchedule schedule)
    {
        schedule.SetPark(this);
        ClosingSchedule = schedule;
    }

    public void ClearClosingSchedule()
    {
        ClosingSchedule = null;
    }

    public void AddAmenity(Amenity amenity)
    {
        var pa = new ParkAmenity { Park = this, Amenity = amenity };
        _amenities.Add(pa);
    }

    public void ClearAmenities()
    {
        _amenities.Clear();
    }
}
