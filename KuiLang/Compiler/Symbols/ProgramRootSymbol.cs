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
    public class ProgramRootSymbol : ISymbol, ISymbolWithFields
    {
        readonly Dictionary<string, TypeSymbol> _typesSymbols = new();
        public ProgramRootSymbol( Ast symbolAst )
        {
            Ast = symbolAst;
            HardcodedSymbols = new( this );
        }

        public Ast Ast { get; }

        public HardcodedSymbols HardcodedSymbols { get; } 

        public IReadOnlyDictionary<string, TypeSymbol> TypesSymbols => _typesSymbols;
        public OrderedDictionary<string, FieldSymbol> Fields { get; } = new();
        public FunctionExpressionSymbol MainFunction { get; set; }
        public ISymbol? Parent => throw new InvalidOperationException( "Cannot access root parent." );

        public OrderedDictionary<string, ParameterSymbol> ParameterSymbols { get; } = new();

        public void Add( TypeSymbol symbol ) => _typesSymbols.Add( symbol.Ast.Name, symbol );

        public override string ToString()
            =>
$@"
{{
    ""Root"": {{
        ""Types"": [
{string.Join( "\n,", TypesSymbols.Values.Select( s => s.ToString() ) )}
        ],
        ""Fields"": [  
{string.Join( "\n,", Fields.Values.Select( s => s.ToString() ) )}
        ]
    }}
}}
";

    }
}
