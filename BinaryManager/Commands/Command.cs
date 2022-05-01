public abstract class Command
{
    public string[] Key { get; set; }
    public string Title { get; set; }
    public abstract bool IsVisible { get; }

    public Command(string key, string title) : this(new string[] { key }, title)
    {
    }

    public Command(string[] key, string title)
    {
        Key = key;
        Title = title;
    }

    public string PrintableTitle =>
        $"{string.Join(", ", Key)} : {Title}";

    public short Titlelength =>
       (short)(Key.Select(x => x.Length).Sum() + ((Key.Length - 1) * 2) + Title.Length + 3);

    public abstract string Do();
    public abstract string Undo();

    public static implicit operator KeyValuePair<string, Command>[](Command c) =>
        c.Key.Select(x => new KeyValuePair<string, Command>(x, c)).ToArray();
}