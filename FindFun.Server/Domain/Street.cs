namespace FindFun.Server.Domain;

public class Street
{
    public string Name { get; private set; }

    public int Id { get; private set; }
    public ICollection<Address> Addresses { get; private set; } = [];

    public Municipality? Municipio { get; private set; }
    public int MunicipioGid { get; private set; }

    protected Street()
    {
        
    }
    public Street(string name, int municipalityId)
    {
        Name = name;
        SetMunicipioId(municipalityId);
    }

    public void AddAddress(Address address)
    {
        if (!Addresses.Contains(address))
        {
            Addresses.Add(address);
            if (address.Street != this)
                address.SetStreet(this);
        }
    }

    public void RemoveAddress(Address address)
    {
        if (Addresses.Remove(address) && address.Street == this)
            address.SetStreet(null);
    }
    public void SetMunicipioId(int municipioGid)
    {
        MunicipioGid = municipioGid;
    }

}

