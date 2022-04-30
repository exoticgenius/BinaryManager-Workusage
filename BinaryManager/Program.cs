using Microsoft.Extensions.Configuration;

using System.Collections.ObjectModel;
using System.Threading.Channels;

var channel = Channel.CreateUnbounded<Command>();
ObservableCollection<string> output = new();
var conf = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();
var Commands = new Dictionary<string, Command>();
bool MenuisVisible = await args.Aggregate(conf, Commands, channel.Writer);
Task Executer = channel.CommandExecuter(output, MenuisVisible);







(int width, int height) = Commands.GetCommandsSize();
Console.SetWindowSize(width, height);
while (true)
{
    Console.Clear();


}