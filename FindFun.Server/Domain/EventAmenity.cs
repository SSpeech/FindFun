

namespace FindFun.Server.Domain;

public class EventAmenity
{
    public Guid EventId { get; set; }
    public Event Event { get; set; } = null!;
    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}
