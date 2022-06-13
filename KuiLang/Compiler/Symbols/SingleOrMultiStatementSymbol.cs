using KuiLang.Semantic;
using KuiLang.Syntax;
using OneOf;
using System;

namespace KuiLang.Compiler.Symbols
{
    public class SingleOrMultiStatementSymbol
    {
        SingleOrMultiStatementSymbol( ISymbol value )
        {
            Value = value;
        }
        public ISymbol Value { get; }

        public static SingleOrMultiStatementSymbol From( ISymbol symbol ) => new( symbol );

        public ISymbolWithAStatement AsSingle => (ISymbolWithAStatement)Value;
        public bool IsSingle => Value is ISymbolWithAStatement;
        public StatementBlockSymbol AsBlock => (StatementBlockSymbol)Value;
        public bool IsBlock => Value is StatementBlockSymbol;
    }
}
