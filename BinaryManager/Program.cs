using Microsoft.Extensions.Configuration;

using System.Threading.Channels;

Console.SetWindowSize(128, 32);

var channel = Channel.CreateUnbounded<Command>();
List<string> output = new();

var conf = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();
var Commands = new Dictionary<string, Command>();
Commands.Add(new ConsoleWiper(output));

bool MenuisVisible = await args.Aggregate(conf, Commands, channel.Writer);
Task Executer = channel.CommandExecuter(output, Commands, MenuisVisible);

while (true)
{
    string cmd = Console.ReadLine()!;
    if(Commands.TryGetValue(cmd, out Command c))
    {
        await channel.Writer.WriteAsync(c);
    }
    else
    {
        await channel.Writer.WriteAsync(new NotFound(cmd));
    }
}