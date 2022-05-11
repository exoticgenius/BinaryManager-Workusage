using Microsoft.Extensions.Configuration;

using System.Threading.Channels;

var channel = Channel.CreateUnbounded<Command>();

var conf = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();

await args.Aggregate(conf, channel.Writer);
Task Executer = channel.CommandExecuter();

if (Statics.CliActive)
    Console.SetWindowSize(128, 32);
else
    channel.Writer.Complete();

await Statics.TakeInputs(channel);