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
    public class ProgramRootSymbol : ISymbol, ISymbolWithAStatement, ISymbolWithMethods
    {
        readonly Dictionary<string, TypeSymbol> _typesSymbols = new();

        public ProgramRootSymbol( Ast symbolAst )
        {
            Ast = symbolAst;
            Statement = new( this, null );
        }

        public Ast Ast { get; }


        public IReadOnlyDictionary<string, TypeSymbol> TypesSymbols => _typesSymbols;
        public Dictionary<string, MethodSymbol> Methods { get; } = new();
        public StatementBlockSymbol Statement { get; }
        StatementSymbol ISymbolWithAStatement.Statement
        {
            get => Statement;
            set => throw new NotSupportedException();
        }
        public ISymbol? Parent => null;

        public void Add( TypeSymbol symbol ) => _typesSymbols.Add( symbol.Name, symbol );
    }
}
