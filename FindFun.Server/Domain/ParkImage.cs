namespace FindFun.Server.Domain;

public class ParkImage : Image
{
    public int ParkId { get; set; }
    public Park Park { get; set; } = null!;

    protected ParkImage() { }

    public ParkImage(string url) : base(url)
    {
    }

    public void SetPark(Park park)
    {
        Park = park;
        ParkId = park.Id;
    }
}
