using Microsoft.Extensions.Configuration;

using System.Text.Json;

public class ProcessEmulator : Command
{
    private readonly IConfiguration configuration;
    private readonly Dictionary<string, Command> commands;

    public override bool IsVisible { get; }

    public ProcessEmulator(
        IConfiguration configuration,
        Dictionary<string, Command> comands) :
        base(Statics.PROCESSEMULATORKEY, Statics.PROCESSEMULATOR)
    {
        this.configuration = configuration;
        this.commands = comands;
    }

    public override string Do()
    {
        Undo();
        var targets = JsonSerializer.Deserialize<List<TargetProcess>>(configuration[Statics.PROCESSES]) ?? new List<TargetProcess>();
        int c = 1;

        foreach (var item in targets)
        {
            var killer = new ProcessKiller($"k{c++}", item.Id, item.Name);
            commands.Add(killer);
        }

        return $"{targets.Count} process(s) loaded";
    }

    public override string Undo()
    {
        var processes = commands.Where(x => x.Value is ProcessKiller);
        foreach (var item in processes)
        {
            commands.Remove(item.Key);
        }

        return $"{processes.Count()} process(es) unloaded";
    }
}