using BinaryManager.Commands;

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
    public const string TASKS = "Tasks";

    public const string FILEEMULATORKEY = "/f";
    public const string FILEEMULATOR = "FILEEMULATOR";
    public const string FILELOCATIONES = "Locations";


    public static void Add<T, Y>(this Dictionary<T, Y> d, KeyValuePair<T, Y>[] kv)
    {
        foreach (var item in kv)
            d.Add(item.Key, item.Value);
    }

    public static async Task<bool> Aggregate(this string[] args, IConfiguration conf, Dictionary<string, Command> Commands, List<string> output, ChannelWriter<Command> channel)
    {
        bool cliActive = false;
        binaryEm(conf, Commands);
        processEm(conf, Commands);
        taskEm(conf, Commands);

        if (args.Length == 0)
        {
            cliActive = true;
            Commands.Add(new ConsoleWiper(output));
            Commands.Add(new ExitApp());

            foreach (var item in Commands.Where(x => x.Value is Emulator))
            {
                await channel.WriteAsync(item.Value);
            }
        }
        else
        {
            foreach (var item in args)
            {
                if (Commands.TryGetValue(item, out var cmd))
                {
                    if (cmd is Emulator) AutoCmd(cmd);
                }
            }
            foreach (var item in args)
            {
                if (Commands.TryGetValue(item, out var cmd))
                {
                    if (cmd is not Emulator) AutoCmd(cmd);
                }
            }
        }

        return cliActive;
    }

    private static void taskEm(IConfiguration conf, Dictionary<string, Command> Commands)
    {
        var em = new TaskEmulator(conf, Commands);
        Commands.Add(em);
    }

    private static void processEm(IConfiguration conf, Dictionary<string, Command> Commands)
    {
        var em = new ProcessEmulator(conf, Commands);
        Commands.Add(em);
    }

    private static void binaryEm(IConfiguration conf, Dictionary<string, Command> Commands)
    {
        var em = new BinaryEmulator(conf, Commands);
        Commands.Add(em);
    }

    public static async Task TakeInputs(Channel<Command> channel, Dictionary<string, Command> Commands)
    {
        while (true)
        {
            string cmd = Console.ReadLine()!;
            if (Commands.TryGetValue(cmd, out Command c))
            {
                await channel.Writer.WriteAsync(c);
            }
            else
            {
                await channel.Writer.WriteAsync(new NotFound(cmd));
            }
        }
    }

    public static async Task CommandExecuter(this Channel<Command> channel, List<string> output, Dictionary<string, Command> commands, bool cliActive)
    {
        await foreach (var item in channel.Reader.ReadAllAsync())
        {
            if (cliActive)
            {
                RunCmd(output, item);
                RenderWindow(commands, output);
            }
            else
            {
                AutoCmd(item);
            }
        }
    }

    private static void RunCmd(List<string> output, Command item)
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
    }

    private static void AutoCmd(Command item)
    {
        try
        {
            string result = item.Do();
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
    {
        for (int i = 0; i < str.Length; i += maxChunkSize)
            yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i)).PadRight(32);
    }

    private static void RenderWindow(Dictionary<string, Command> commands, List<string> output)
    {
        try
        {
            var visibleCmds = commands.Where(x => x.Value.IsVisible).GroupBy(x => x.Value.Key).Select(x => x.First()).ToList();

            string raw = "┌┐└┘┴┬│─";
            int width = Console.WindowWidth - 3;

            var o1 = output.
                TakeLast(visibleCmds.Count).ToList();
            var o2 = o1.
                Select(s => ChunksUpto(s, 32))
                    .ToList();
            var o3 = o2.
                SelectMany(x => x).ToList();

            var o4 = o3.
                TakeLast(visibleCmds.Count).ToList();

            var selectedOutput = new Queue<string>(o4);


            var s1 = new string('─', width - 32);
            var s2 = new string('─', 32);

            Console.Clear();

            Console.WriteLine($"┌{s1}┬{s2}┐");
            foreach (var item in visibleCmds)
            {
                string cmd = item.Value.PrintableTitle;

                if (cmd.Length > width - 32) cmd = cmd.Substring(0, width - 32);
                else cmd = cmd.PadRight(width - 32);

                Console.WriteLine($"│{cmd}|{(selectedOutput.Count > 0 ? selectedOutput.Dequeue() : new string(' ', 32))}|");
            }
            Console.WriteLine($"└{s1}┴{s2}┘");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}