using Microsoft.Extensions.Configuration;

using System.Text.Json;

public class TaskEmulator : Emulator
{
    public TaskEmulator(
        IConfiguration configuration,
        Dictionary<string, Command> commands) :
        base(Statics.TASKEMULATORKEY, Statics.TASKEMULATOR, configuration, commands)
    {
    }

    public override string Do()
    {
        Undo();
        var targets = configuration.GetSection(Statics.TASKS).Get<List<Target>>();
        int c = 1;

        foreach (var item in targets)
        {
            string[] keys = new string[]
            {
                $"t{c++}",
                item.Id
            };
            var task = new TaskRunner(keys, $"run {item.Id} task", item.Data);
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