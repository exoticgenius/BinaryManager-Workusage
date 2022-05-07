using Microsoft.Extensions.Configuration;

public abstract class Emulator : Command
{
    protected readonly IConfiguration configuration;
    protected readonly Dictionary<string, Command> commands;

    public Emulator(string key, string title, IConfiguration configuration,
    Dictionary<string, Command> commands) : base(key, title)
    {
        this.configuration = configuration;
        this.commands = commands;
    }

    public override sealed bool IsVisible => false;
}