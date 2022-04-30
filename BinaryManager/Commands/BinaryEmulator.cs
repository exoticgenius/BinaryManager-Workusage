using Microsoft.Extensions.Configuration;

public class BinaryEmulator : Command
{
    private readonly IConfiguration configuration;
    Dictionary<string, Command> commands;

    public override bool IsVisible => false;

    public BinaryEmulator(
        IConfiguration configuration,
        Dictionary<string, Command> commands) :
        base(Statics.BINARYEMULATORKEY, Statics.BINARYEMULATOR)
    {
        this.configuration = configuration;
        this.commands = commands;
    }

    public override string Do()
    {
        Undo();
        try
        {
            string src = configuration[Statics.BINROOT];
            string dest = configuration[Statics.DESTROOT];
            int c = 1;

            foreach (var item in
            Directory.GetDirectories(src).
            Select(x => new DirectoryInfo(x)).
            Where(x =>
            File.Exists(Path.Combine(x.FullName, Statics.FILENAME
            ))))
            {
                BinaryFile bf = new(
                    $"c{c++}",
                    item.Name,
                    Path.Combine(item.FullName, Statics.FILENAME),
                    Path.Combine(dest, Statics.FILENAME));
                commands.Add(bf);
            }

            return $"{c} file(s) loaded";
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

        return $"{files.Count()} file(s) unloaded";
    }
}