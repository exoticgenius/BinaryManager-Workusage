public static class Statics
{
    public const string BINARYEMULATORKEY = "/b";
    public const string BINARYEMULATOR = "BINARYEMULATOR";
    public const string FILENAME = "binary.dll";
    public const string BINROOT = "BinRoot";
    public const string DESTROOT = "DestRoot";

    public const string PROCESSEMULATORKEY = "/p";
    public const string PROCESSEMULATOR = "PROCESSEMULATOR";
    public const string PROCESSES = "Processes";


    public static void Add<T, Y>(this Dictionary<T, Y> d, KeyValuePair<T, Y> kv) =>
        d.Add(kv.Key, kv.Value);
    public static (int, int) GetCommandsSize(this Dictionary<string, Command> d) =>
        (Math.Max(16, (int)(d.Values.Where(x=>x.IsVisible).Select(x => x.Titlelength).Max())), Math.Max(3, Math.Min(30, d.Count)));
}