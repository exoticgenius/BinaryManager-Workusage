using Microsoft.Extensions.Configuration;

using System.Threading.Channels;



var conf = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();

await args.Aggregate(conf);

if (!Statics.CliActive)
    Statics.channel.Writer.Complete();

await Statics.TakeInputs();