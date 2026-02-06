namespace FindFun.Server.Domain;

public class Park
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public Address Address { get; private set; } = null!;
    public int AddressId { get; private set; }

    protected Park()
    {
    }

    public Park( string name, string description, Address address)
    {
        Name = name;
        Description = description;
        Address = address;
        AddressId = address.Id;
    }
}
