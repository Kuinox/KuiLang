using KuiLang.Semantic;
using KuiLang.Syntax;
using OneOf;
using System;

namespace KuiLang.Compiler.Symbols
{
    public class SingleOrMultiStatementSymbol : OneOfBase<ISymbolWithAStatement, StatementBlockSymbol>
    {
        protected SingleOrMultiStatementSymbol( OneOf<ISymbolWithAStatement, StatementBlockSymbol> input ) : base( input )
        {
        }

        public static SingleOrMultiStatementSymbol From( ISymbolWithAStatement symbol )
            => new( OneOf<ISymbolWithAStatement, StatementBlockSymbol>.FromT0( symbol ) );
        public static SingleOrMultiStatementSymbol From( StatementBlockSymbol symbol )
            => new( OneOf<ISymbolWithAStatement, StatementBlockSymbol>.FromT1( symbol ) );

        public static SingleOrMultiStatementSymbol From<T>( ISymbol<T> symbol ) where T : Ast
        {
            if( symbol is StatementBlockSymbol block ) return From( block );
            if( symbol is ISymbolWithAStatement statement ) return From( statement );
            throw new InvalidCastException();
        }
    }
}
