using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryManager.Commands
{
    public class ExitApp : Command
    {
        public ExitApp() : base(new string[] {"e","exit","ret"}, "Exit")
        {
        }

        public override bool IsVisible => true;

        public override string Do()
        {
            Environment.Exit(0);
            return string.Empty;
        }

        public override string Undo() =>
            string.Empty;
    }
}
