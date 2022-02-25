using System.Collections.Generic;

namespace KuiLang
{
    public class Arg
    {
        public Arg(FullName type, string argumentName)
        {
            Type = type;
            ArgumentName = argumentName;
        }

        public FullName Type { get; } 
        public string ArgumentName { get; }

        public override string ToString() => $"{Type} {ArgumentName}";
    }
}