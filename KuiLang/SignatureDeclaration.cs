using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang
{
    public class SignatureDeclaration
    {
        public SignatureDeclaration(FieldLocation returnType, string methodName, List<Arg>? arguments)
        {
            Type = returnType;
            Name = methodName;
            Arguments = arguments?? new List<Arg>();
        }

        public FieldLocation Type { get; }
        public string Name { get; }
        public IReadOnlyCollection<Arg> Arguments { get; }

        public override string ToString() => $"{Type} {Name}({string.Join(", ", Arguments.Select(s=>s.ToString()))})";
    }
}
