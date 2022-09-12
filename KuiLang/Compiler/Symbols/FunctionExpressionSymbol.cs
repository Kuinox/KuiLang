using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class FunctionExpressionSymbol : ISymbol, IExpressionSymbol
    {
        public FunctionExpressionSymbol( ISymbol parent, string name, Ast.Statement.Definition.Typed.Method? method )
        {
            Parent = parent;
            Name = name;
            ReturnType = new TypeSymbol( parent.GetRoot(), new Ast.Statement.Definition.Type( name, new List<Ast.Statement.Definition>()
            {

            } ) );
            FuncReturnTypeIdentifier = method?.ReturnTypeIdentifier;
        }

        public ISymbol Parent { get; }
        public string Name { get; }
        public OrderedDictionary<string, ParameterSymbol> Parameters { get; } = new();

        public StatementSymbol Statement { get; set; }
        public TypeSymbol FuncReturnType { get; set; }
        public TypeSymbol ReturnType { get; internal set; }
        public Identifier? FuncReturnTypeIdentifier { get; }
    }
}
