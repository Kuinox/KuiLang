using System.Collections.Generic;

namespace KuiLang
{
    public class Arg
    {
        public Arg(FieldLocation type, string argumentName)
        {
            Type = type;
            ArgumentName = argumentName;
        }

        public FieldLocation Type { get; } 
        public string ArgumentName { get; }

        public override string ToString() => $"{Type} {ArgumentName}";
    }
}