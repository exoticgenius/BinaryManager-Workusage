public class BinaryFile : Command
{
    private readonly string src;
    private readonly string dest;

    public override bool IsVisible => true;

    public BinaryFile(string[] key, string title, string src, string dest) : base(key, title)
    {
        this.src = src;
        this.dest = dest;
    }

    public override string Do()
    {
        try
        {
            File.Copy(src, dest, true);
            return $"{Title} copied";
        }
        catch
        {
            return $"{Title} failed to copy";
        }
    }

    public override string Undo() =>
        string.Empty;
}