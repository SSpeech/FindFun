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
}
