using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang
{
    public class SignatureDeclaration
    {
        public SignatureDeclaration(FullName returnType, string methodName, List<Arg>? arguments)
        {
            Type = returnType;
            Name = methodName;
            Arguments = arguments?? new List<Arg>();
        }

        public FullName Type { get; }
        public string Name { get; }
        public IReadOnlyCollection<Arg> Arguments { get; }

        public override string ToString() => $"{Type} {Name}({string.Join(", ", Arguments.Select(s=>s.ToString()))})";
    }
}
