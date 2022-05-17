using System.Diagnostics;

public class TaskRunner : Command
{
    private readonly string cmd;

    public TaskRunner(string[] key, string title, string cmd) : base(key, title)
    {
        this.cmd = cmd;
    }

    public override bool IsVisible => true;

    public override string Do()
    {
        ProcessStartInfo info = new ProcessStartInfo();
        info.UseShellExecute = false;
        info.RedirectStandardOutput = true;
        info.CreateNoWindow = true;
        info.FileName = cmd;

        Process p = new Process();
        p.StartInfo = info;

        try
        {
            p.Start();

            return p.StandardOutput.ReadToEnd();
        }
        catch(Exception e)
        {
            return e.Message;
        }
        finally
        {
            p.WaitForExit();
        }
    }

    public override string Undo() =>
        string.Empty;
}
