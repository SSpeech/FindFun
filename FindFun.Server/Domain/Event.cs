namespace FindFun.Server.Domain;

public class Event
{
    public int Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
    public ICollection<Review> Reviews { get; private set; } = [];

    protected Event() { }

    public Event(string title, string? description, DateTime startTime, DateTime endTime, Guid addressId)
    {
        Title = title;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        AddressId = addressId;
    }

    public void UpdateDetails(string title, string? description, DateTime startTime, DateTime endTime, Guid addressId)
    {
        Title = title;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        AddressId = addressId;
    }
}


