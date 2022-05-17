using System.Diagnostics;

public class ProcessKiller : Command
{
    private readonly string processName;

    public override bool IsVisible => true;

    public ProcessKiller(string[] key, string title, string processName) : base(key, title)
    {
        this.processName = processName;
    }

    public override string Do()
    {
        int kills = 0;
        try
        {
            Process[] workers = Process.GetProcessesByName(processName);
            foreach (Process worker in workers)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
                kills++;
            }
            return $"{kills} process(s) were killed";
        }
        catch
        {
            return $"kill process failed and {kills} process(s) were killed";
        }
    }

    public override string Undo() =>
        string.Empty;
}