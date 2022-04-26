public class BinaryEmulator : Command
{
    private readonly string src;
    private readonly string dest;
    Dictionary<string, Command> commands;
    public BinaryEmulator(
        string key, 
        string title, 
        string src,
        string dest, 
        Dictionary<string, Command> commands) :
        base(key, title)
    {
        this.src = src;
        this.dest = dest;
        this.commands = commands;
    }

    public override string Do()
    {
        try
        {
            int c = 1;
            foreach (var item in
            Directory.GetDirectories(src).
            Select(x => new DirectoryInfo(x)).
            Where(x =>
            File.Exists(Path.Combine(x.FullName, Statics.FILENAME
            ))))
            {
                BinaryFile bf = new(
                    c++.ToString(),
                    item.Name,
                    Path.Combine(item.FullName, Statics.FILENAME),
                    Path.Combine(dest, Statics.FILENAME));
                commands.Add(bf);
            }

            return $"{c} files loaded";
        }
        catch
        {
            return "failed to load binaries";
        }
    }

    public override string Undo()
    {
        var files = commands.Where(x => x.Value is BinaryFile);
        foreach (var item in files)
        {
            commands.Remove(item.Key);
        }

        return $"{files.Count()} files unloaded";
    }
}