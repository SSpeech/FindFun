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
    public Street(string name, Municipality? municipio = null)
    {
        Name = name;
        if (municipio != null)
            SetMunicipio(municipio);
    }

    public void AddAddress(Address address)
    {
        if (address == null) return;
        if (!Addresses.Contains(address))
        {
            Addresses.Add(address);
            if (address.Street != this)
                address.SetStreet(this);
        }
    }

    public void RemoveAddress(Address address)
    {
        if (address is null) return;
        if (Addresses.Remove(address))
        {
            if (address.Street == this)
            {
                address.SetStreet(null);
            }
        }
    }

    public void SetMunicipio(Municipality? municipio)
    {
        if (municipio is null)
        {
            Municipio = null;
            MunicipioGid = 0;
            return;
        }

        Municipio = municipio;
        MunicipioGid = municipio.Gid;
        if (!municipio.Streets.Contains(this))
            municipio.Streets.Add(this);
    }

    // New: set the foreign key without loading the Municipality entity
    public void SetMunicipioId(int municipioGid)
    {
        Municipio = null;
        MunicipioGid = municipioGid;
    }

}

