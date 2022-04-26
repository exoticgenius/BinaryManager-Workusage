public static class Statics
{
    public const string FILENAME = "binary.dll";
    public const string BINROOT = "BinRoot";
    public const string DESTROOT = "DestRoot";




    public static void Add<T, Y>(this Dictionary<T, Y> d, KeyValuePair<T, Y> kv) =>
        d.Add(kv.Key, kv.Value);
    public static (int, int) GetCommandsSize(this Dictionary<string, Command> d) =>
        (Math.Max(16, (int)d.Values.Select(x => x.Titlelength).Max()), Math.Max(3, Math.Min(30, d.Count)));
}