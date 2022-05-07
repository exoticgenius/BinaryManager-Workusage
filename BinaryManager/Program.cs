using Microsoft.Extensions.Configuration;

using System.Threading.Channels;

Console.SetWindowSize(128, 32);

var channel = Channel.CreateUnbounded<Command>();
List<string> output = new();

var conf = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();
var Commands = new Dictionary<string, Command>();

bool CLIActive = await args.Aggregate(conf, Commands, output, channel.Writer);
Task Executer = channel.CommandExecuter(output, Commands, CLIActive);

if (!CLIActive) return;

await Statics.TakeInputs(channel, Commands);