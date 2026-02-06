

namespace FindFun.Server.Domain;

public class Amenity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<ParkAmenity> ParkAmenities { get; set; } = [];
}
