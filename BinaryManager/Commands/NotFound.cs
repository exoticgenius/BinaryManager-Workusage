public class NotFound : Command
{
    private readonly string title;

    public NotFound(string title) : base(string.Empty,string.Empty)
    {
        this.title = title;
    }

    public override bool IsVisible => false;

    public override string Do() =>
        $"command {title} not found";

    public override string Undo() =>
        String.Empty;
}