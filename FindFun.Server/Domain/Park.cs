namespace FindFun.Server.Domain;

using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;
using System.Linq;

public class Park
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public Address Address { get; private set; } = null!;
    public int AddressId { get; private set; }

    public decimal EntranceFee { get; private set; }

    public string? Organizer { get; private set; }
    public string? AgeRecommendation { get; private set; }
    public string? ParkType { get; private set; }
    public bool IsFree { get; private set; }

    public ClosingSchedule? ClosingSchedule { get; private set; }

    private readonly List<ParkAmenity> _amenities = [];
    public IReadOnlyCollection<ParkAmenity> Amenities => _amenities;
    private readonly List<ParkImage> _images = [];
    public IReadOnlyCollection<ParkImage> Images => _images;

    protected Park()
    {
    }

    public Park(string name, string description, Address address, decimal entranceFee, bool isFree, string?
        organizer, string parkType, string? ageRecomandation)
    {
        Name = name;
        Description = description;
        Address = address;
        AddressId = address.Id;
        EntranceFee = entranceFee;
        IsFree = isFree;
        Organizer = organizer;
        ParkType = parkType;
        AgeRecommendation = ageRecomandation;
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

    public void AddImages(IEnumerable<ParkImage> images)
    {
        if (images is null) return;
        _images.AddRange(images);
    }

    public void ClearImages()
    {
        _images.Clear();
    }
}
