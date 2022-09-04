using KuiLang.Compiler.Symbols;
using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler
{
    public class ProgramRootSymbol : ISymbol, ISymbolWithAStatement, ISymbolWithMethods, IMethodSymbol
    {
        readonly Dictionary<string, TypeSymbol> _typesSymbols = new();

        public ProgramRootSymbol( Ast symbolAst )
        {
            Ast = symbolAst;
        }

        public Ast Ast { get; }


        public IReadOnlyDictionary<string, TypeSymbol> TypesSymbols => _typesSymbols;
        public Dictionary<string, MethodSymbol> Methods { get; } = new();
        public StatementBlockSymbol Statement { get; set; } = null!;
        StatementSymbol ISymbolWithAStatement.Statement
        {
            get => Statement;
            set => throw new NotSupportedException();
        }
        public ISymbol? Parent => throw new InvalidOperationException( "Cannot access root parent." );

        public OrderedDictionary<string, MethodParameterSymbol> ParameterSymbols { get; } = new();

        public void Add( TypeSymbol symbol ) => _typesSymbols.Add( symbol.Ast.Name, symbol );

        public override string ToString()
            =>
$@"
{{
    ""Root"": {{
        ""Statement"": {Statement},
        ""Types"": [
{string.Join( "\n,", TypesSymbols.Values.Select( s => s.ToString()  ) )}
        ],
        ""Methods"": [  
{string.Join( "\n,", Methods.Values.Select( s => s.ToString() ) )}
        ],
        ""HardcodedTypes"": [
            {HardcodedSymbols.NumberType}
        ]
    }}
}}
";

    }
}
