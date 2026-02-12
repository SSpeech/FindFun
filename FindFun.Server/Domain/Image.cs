namespace FindFun.Server.Domain;

public abstract class Image
{
    public int Id { get; private set; }

    public string Url { get; private set; } = null!;

    protected Image() { }

    protected Image(string url)
    {
        Url = url;
    }
}
