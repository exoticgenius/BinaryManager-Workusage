using System;
using System.Collections.Generic;
public class ConsoleWiper : Command
{
    private readonly List<string> output;

    public ConsoleWiper(
    List<string> output) : base(new string[] { "cls", "c" }, "Refresh")
    {
        this.output = output;
    }

    public override bool IsVisible => true;

    public override string Do()
    {
        output.Clear();
        Console.Clear();
        return string.Empty;
    }

    public override string Undo() =>
        string.Empty;
}