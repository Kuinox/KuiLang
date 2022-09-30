using KuiLang.Compiler.Symbols;
using KuiLang.Semantic;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Interpreter
{
    public class RuntimeReference
    {
        public RuntimeReference( RuntimeObject owner, object field )
        {
            Owner = owner;
            Field = field;
            if( !owner.Fields.ContainsKey( field ) ) throw new Wat();
        }

        public RuntimeObject Owner { get; }
        public object Field { get; }
    }

    public class RuntimeObject
    {
        public Dictionary<object, OneOf<RuntimeObject, object, FunctionExpressionSymbol>> Fields { get; } = new();
    }
}
