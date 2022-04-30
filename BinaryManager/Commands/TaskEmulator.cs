using Microsoft.Extensions.Configuration;

using System.Text.Json;

public class TaskEmulator : Command
{
    private readonly IConfiguration configuration;
    private readonly Dictionary<string, Command> commands;

    public TaskEmulator(
        IConfiguration configuration,
        Dictionary<string, Command> commands) :
        base(Statics.TASKEMULATORKEY, Statics.TASKEMULATOR)
    {
        this.configuration = configuration;
        this.commands = commands;
    }

    public override bool IsVisible => false;

    public override string Do()
    {
        Undo();
        var targets = JsonSerializer.Deserialize<List<TargetTask>>(configuration[Statics.PROCESSES]) ?? new List<TargetTask>();
        int c = 1;

        foreach (var item in targets)
        {
            var task = new TaskRunner($"t{c++}", item.Id, item.Cmd);
            commands.Add(task);
        }

        return $"{targets.Count} task(s) loaded";
    }

    public override string Undo()
    {
        var tasks = commands.Where(x => x.Value is TaskRunner);
        foreach (var item in tasks)
        {
            commands.Remove(item.Key);
        }

        return $"{tasks.Count()} tasks(s) unloaded";
    }
}