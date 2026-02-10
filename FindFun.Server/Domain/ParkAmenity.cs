namespace FindFun.Server.Domain;

public class ParkAmenity
{
    public int Id { get; private set; }
    public int ParkId { get; set; }
    public Park Park { get; set; } = null!;
    public int AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}
