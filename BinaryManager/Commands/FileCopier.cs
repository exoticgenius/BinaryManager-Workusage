public class FileCopier : Command
{
    private readonly string source;
    private readonly string dest;

    public FileCopier(string key, string title,string source, string dest) : base(new string[] {key, title}, title)
    {
        this.source = source;
        this.dest = dest;
    }

    public override bool IsVisible => true;

    public override string Do()
    {
        var sdi = new DirectoryInfo(source);
        var ddi = new DirectoryInfo(dest);
        var result = RecursiveCopy(sdi, ddi);

        return $"{result.all} file(s) processed, {result.success} file(s) copied, {result.failed} file(s) failed";
    }

    private (int all, int success, int failed) RecursiveCopy(DirectoryInfo sdi, DirectoryInfo ddi)
    {
        int all = 0;
        int success = 0;
        int failed = 0;

        try
        {
            foreach (var item in sdi.GetFiles())
            {
                all++;
                try
                {
                    var dp = Path.Combine(ddi.FullName, item.Name);
                    item.CopyTo(Path.Combine(ddi.FullName, item.Name), true);
                    success++;
                }
                catch
                {
                    failed++;
                }

                Statics.Log(item.Name);
            }
        }
        catch { }

        try
        {
            foreach (var item in sdi.GetDirectories())
            {
                DirectoryInfo childDir = new DirectoryInfo(Path.Combine(ddi.FullName, item.Name));
                if (!childDir.Exists) childDir.Create();

                var res = RecursiveCopy(item, childDir);
                all += res.all;
                success += res.success;
                failed += res.failed;
            }
        }
        catch { }

        return (all, success, failed);
    }

    public override string Undo() =>
        string.Empty;
}