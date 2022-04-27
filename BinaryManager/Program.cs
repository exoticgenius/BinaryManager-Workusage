using Microsoft.Extensions.Configuration;

var conf = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();
var Commands = new Dictionary<string, Command>();
Commands.Add(new BinaryEmulator(conf,Commands));
Commands.Add(new ProcessEmulator(conf, Commands));









(int width, int height) = Commands.GetCommandsSize();
Console.SetWindowSize(width, height);
while (true)
{
    Console.Clear();


}