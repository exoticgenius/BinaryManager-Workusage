using Microsoft.Extensions.Configuration;

using System.Collections.ObjectModel;
using System.Threading.Channels;

public static class Statics
{
    public const string BINARYEMULATORKEY = "/b";
    public const string BINARYEMULATOR = "BINARYEMULATOR";
    public const string FILENAME = "binary.dll";
    public const string BINROOT = "BinRoot";
    public const string DESTROOT = "DestRoot";

    public const string PROCESSEMULATORKEY = "/p";
    public const string PROCESSEMULATOR = "PROCESSEMULATOR";
    public const string PROCESSES = "Processes";

    public const string TASKEMULATORKEY = "/t";
    public const string TASKEMULATOR = "TASKEMULATOR";


    public static void Add<T, Y>(this Dictionary<T, Y> d, KeyValuePair<T, Y>[] kv)
    {
        foreach (var item in kv)
            d.Add(item.Key, item.Value);
    }

    public static (int, int) GetCommandsSize(this Dictionary<string, Command> d) =>
        (Math.Max(16, (int)(d.Values.Where(x => x.IsVisible).Select(x => x.Titlelength).Max())), Math.Max(3, Math.Min(30, d.Count)));

    public static async Task<bool> Aggregate(this string[] args, IConfiguration conf, Dictionary<string, Command> Commands, ChannelWriter<Command> channel)
    {
        bool showMenu = false;

        if (args.Length == 0)
        {
            showMenu = true;
            return showMenu;
        }
        else if (args.Contains("/help"))
        {
            showMenu = true;
            return showMenu;
        }
        if (args.Contains(BINARYEMULATORKEY))
        {
            var em = new BinaryEmulator(conf, Commands);
            Commands.Add(em);
            await channel.WriteAsync(em);
        }
        if (args.Contains(PROCESSEMULATORKEY))
        {
            var em = new ProcessEmulator(conf, Commands);
            Commands.Add(em);
            em.Do();
            await channel.WriteAsync(em);
        }
        if (args.Contains(TASKEMULATORKEY))
        {
            var em = new TaskEmulator(conf, Commands);
            Commands.Add(em);
            em.Do();
            await channel.WriteAsync(em);
        }
        return showMenu;
    }

    public static async Task CommandExecuter(this Channel<Command> channel, ObservableCollection<string> output, bool menuisVisible)
    {
        await foreach (var item in channel.Reader.ReadAllAsync())
        {
           string result = item.Do();
        }
    }
}