using Microsoft.Extensions.Configuration;

public class FileEmulator : Emulator
{
    public FileEmulator(
        IConfiguration configuration,
        Dictionary<string, Command> commands) :
        base(Statics.FILEEMULATORKEY, Statics.FILEEMULATOR, configuration, commands)
    {
    }

    public override string Do()
    {
        var files = configuration.GetSection(Statics.FILELOCATIONES).Get<List<FileTarget>>();
        int c = 1;
        foreach (var item in files)
        {
            FileCopier copier = new($"f{c++}", item.Id, item.Source, item.Destination);
            commands.Add(copier);
        }

        return $"{files.Count} location(s) loaded";
    }

    public override string Undo()
    {
        var copiers = commands.Where(x => x.Value is FileCopier);
        foreach (var item in copiers)
        {
            commands.Remove(item.Key);
        }

        return $"{copiers.Count()} location(s) unloaded";
    }
}