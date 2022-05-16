using Microsoft.Extensions.Configuration;

public class BinaryEmulator : Emulator
{
    public BinaryEmulator(
        IConfiguration configuration,
        Dictionary<string, Command> commands) :
        base(Statics.BINARYEMULATORKEY, Statics.BINARYEMULATOR, configuration, commands)
    {
    }

    public override string Do()
    {
        Undo();
        try
        {
            string src = configuration[Statics.BINROOT];
            List<Target> dests = configuration.GetSection(Statics.DESTROOTS).Get<List<Target>>();
            int c = 1;

            var dirs = Directory.GetDirectories(src).
            Select(x => new DirectoryInfo(x)).
            Where(x =>
            File.Exists(Path.Combine(x.FullName, Statics.FILENAME
            )));

            foreach (var item in dirs)
            {
                foreach (var dest in dests)
                {
                    BinaryFile bf = new(
                        $"b{c++}",
                        $"{item.Name}_to_{dest.Id}",
                        Path.Combine(item.FullName, Statics.FILENAME),
                        Path.Combine(dest.Data, Statics.FILENAME));
                    commands.Add(bf);
                }
            }

            return $"{c - 1} binary file(s) loaded";
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

        return $"{files.Count()} binary file(s) unloaded";
    }
}