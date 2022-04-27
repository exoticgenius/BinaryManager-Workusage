public abstract class Command
{
    public string Key { get; set; }
    public string Title { get; set; }
    public abstract bool IsVisible { get; }

    public Command(string key, string title)
    {
        Key = key;
        Title = title;
    }

    public string PrintableTitle =>
        $"{Key} : {Title}";

    public short Titlelength =>
       (short)(Key.Length + Title.Length + 3);

    public abstract string Do();
    public abstract string Undo();

    public static implicit operator KeyValuePair<string, Command>(Command c) =>
        new KeyValuePair<string, Command>(c.Key, c);
}