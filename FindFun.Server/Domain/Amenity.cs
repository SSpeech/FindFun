

namespace FindFun.Server.Domain;

public class Amenity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<ParkAmenity> ParkAmenities { get; set; } = [];
}
