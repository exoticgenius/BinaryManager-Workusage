using Microsoft.Extensions.Configuration;

using System.Text.Json;

public class ProcessEmulator : Emulator
{
    public ProcessEmulator(
        IConfiguration configuration,
        Dictionary<string, Command> commands) :
        base(Statics.PROCESSEMULATORKEY, Statics.PROCESSEMULATOR,configuration,commands)
    {
    }

    public override string Do()
    {
        Undo();
        var targets = configuration.GetSection(Statics.PROCESSES).Get<List<Target>>();
        int c = 1;

        foreach (var item in targets)
        {
            var killer = new ProcessKiller($"k{c++}", item.Id, item.Data);
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