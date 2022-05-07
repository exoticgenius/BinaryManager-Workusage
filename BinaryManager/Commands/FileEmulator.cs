using Microsoft.Extensions.Configuration;

public class FileEmulator : Emulator
{
    public FileEmulator(
        IConfiguration configuration,
        Dictionary<string, Command> commands) :
        base(Statics.FILEEMULATORKEY, Statics.FILEEMULATOR, configuration, commands)
    {
    }

    public override string Do()
    {
        throw new NotImplementedException();
    }

    public override string Undo()
    {
        throw new NotImplementedException();
    }
}