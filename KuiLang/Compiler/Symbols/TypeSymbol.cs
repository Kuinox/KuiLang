using KuiLang.Compiler;
using KuiLang.Compiler.Symbols;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KuiLang.Syntax.Ast;

namespace KuiLang.Semantic
{
    public class TypeSymbol : ISymbol, ISymbolWithFields
    {
        public TypeSymbol( ProgramRootSymbol parent, Ast.Statement.Definition.Type ast )
        {
            Parent = parent;
            Ast = ast;
        }

        public bool IsMethod { get; }

        public ProgramRootSymbol Parent { get; }

        ISymbol? ISymbol.Parent => Parent;

        public Ast.Statement.Definition.Type Ast { get; }

        public OrderedDictionary<string, FieldSymbol> Fields { get; } = new();

        public FunctionExpressionSymbol Constructor { get; internal set; } = null!;

        public Identifier Identifier => new( Ast.Name );

        public override string ToString()
            =>
$@"
{{
    ""Fields"": [
{string.Join( "\n,", Fields.Values.Select( s => s.ToString() ) )}
        ]
";

    }
}
