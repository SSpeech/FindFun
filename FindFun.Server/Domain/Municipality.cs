using NetTopologySuite.Geometries;

namespace FindFun.Server.Domain;

public class Municipality
{
    public int Gid { get; private set; }
    public string Year { get; private set; } = null!;
    public string OfficialCo { get; private set; } = null!;
    public string OfficialNa { get; private set; } = null!;
    public string OfficialCo3 { get; private set; } = null!;
    public string OfficialNa4 { get; private set; } = null!;
    public string OfficialCo5 { get; private set; } = null!;
    public string OfficialNa6 { get; private set; } = null!;    
    public string Iso31663 { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public string? LocalName { get; private set; }
    public Geometry Geometry { get; private set; } = null!;


    public ICollection<Street> Streets { get; private set; } = [];

    protected Municipality()
    {
    }
    public static Municipality Create(int gid, string year, string officialCo, string officialNa, string officialCo3, string officialNa4, string officialCo5,
        string officialNa6, string iso31663, string type, string? localName, Geometry geometry)
    {
        return new Municipality
        {
            Gid = gid,
            Year = year,
            OfficialCo = officialCo,
            OfficialNa = officialNa,
            OfficialCo3 = officialCo3,
            OfficialNa4 = officialNa4,
            OfficialCo5 = officialCo5,
            OfficialNa6 = officialNa6,
            Iso31663 = iso31663,
            Type = type,
            LocalName = localName,
            Geometry = geometry,
        };
    }
}
