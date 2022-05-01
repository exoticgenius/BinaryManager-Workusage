using Microsoft.Extensions.Configuration;

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

    public static (int, int) GetCommandsSize(this Dictionary<string, Command> d) => (
            Math.Min(
                d.Values.Where(x => x.IsVisible).Select(x => x.Titlelength).Max(),
                Console.WindowWidth - 35),
            Math.Min(
                d.Count,
                Console.WindowHeight - 2)
        );

    public static async Task<bool> Aggregate(this string[] args, IConfiguration conf, Dictionary<string, Command> Commands, ChannelWriter<Command> channel)
    {
        bool showMenu = false;

        if (args.Length == 0)
        {
            await binaryEm(conf, Commands, channel);
            await processEm(conf, Commands, channel);
            await taskEm(conf, Commands, channel);
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
            await binaryEm(conf, Commands, channel);
        }
        if (args.Contains(PROCESSEMULATORKEY))
        {
            await processEm(conf, Commands, channel);
        }
        if (args.Contains(TASKEMULATORKEY))
        {
            await taskEm(conf, Commands, channel);
        }
        return showMenu;
    }

    private static async Task taskEm(IConfiguration conf, Dictionary<string, Command> Commands, ChannelWriter<Command> channel)
    {
        var em = new TaskEmulator(conf, Commands);
        Commands.Add(em);
        await channel.WriteAsync(em);
    }

    private static async Task processEm(IConfiguration conf, Dictionary<string, Command> Commands, ChannelWriter<Command> channel)
    {
        var em = new ProcessEmulator(conf, Commands);
        Commands.Add(em);
        await channel.WriteAsync(em);
    }

    private static async Task binaryEm(IConfiguration conf, Dictionary<string, Command> Commands, ChannelWriter<Command> channel)
    {
        var em = new BinaryEmulator(conf, Commands);
        Commands.Add(em);
        await channel.WriteAsync(em);
    }
    public static async Task CommandExecuter(this Channel<Command> channel, List<string> output, Dictionary<string, Command> commands, bool menuisVisible)
    {
        await foreach (var item in channel.Reader.ReadAllAsync())
        {
            try
            {
                string result = item.Do();
                output.Add(result);
            }
            catch (Exception ex)
            {
                output.Add(ex.Message);
            }
            RenderWindow(commands, output);
        }
    }
    private static void RenderWindow(Dictionary<string, Command> commands, List<string> output)
    {
        try
        {
            string raw = "┌┐└┘┴┬│─";
            int width = Console.WindowWidth - 3;

            var o1 = output.
                TakeLast(commands.Count);
            var o2 = o1.
                Select(s => Enumerable.Range(0, (int)Math.Ceiling(s.Length / 32.0)).
                    Select(i => s.Length > 32 ? s.Substring(i * 32, 32) : s.PadRight(32)));
            var o3 = o2.
                SelectMany(x => x);

            var o4 = o3.
                TakeLast(commands.Count);

            var selectedOutput = new Queue<string>(o4);


            var s1 = new string('─', width - 32);
            var s2 = new string('─', 32);

            Console.Clear();

            Console.WriteLine($"┌{s1}┬{s2}┐");
            foreach (var item in commands.Where(x => x.Value.IsVisible).GroupBy(x => x.Value.Key).Select(x => x.First()))
            {
                string cmd = item.Value.PrintableTitle;

                if (cmd.Length > width - 32) cmd = cmd.Substring(0, width - 32);
                else cmd = cmd.PadRight(width - 32);

                Console.WriteLine($"│{cmd}|{(selectedOutput.Count > 0 ? selectedOutput.Dequeue() : new string(' ', 32))}|");
            }
            Console.WriteLine($"└{s1}┴{s2}┘");
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}