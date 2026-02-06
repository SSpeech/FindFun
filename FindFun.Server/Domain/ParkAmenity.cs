

namespace FindFun.Server.Domain;

public class ParkAmenity
{
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}
