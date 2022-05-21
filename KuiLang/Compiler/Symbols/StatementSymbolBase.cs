using KuiLang.Semantic;
using KuiLang.Syntax;
using OneOf;
using System;

namespace KuiLang.Compiler.Symbols
{
    public interface IStatementSymbol
    {
        SingleOrMultiStatementSymbol Parent { get; }
    }
    public abstract class StatementSymbolBase<T> : IStatementSymbol, ISymbol<T> where T : Ast.Statement
    {
        public StatementSymbolBase( SingleOrMultiStatementSymbol parent, T symbolAst )
        {
            Parent = parent;
            SymbolAst = symbolAst;
            if( parent.IsT0 )
            {
                parent.AsT0.Statement = this;
            }
            if( parent.IsT1 )
            {
                parent.AsT1.Statements.Add( this );
            }
        }

        public SingleOrMultiStatementSymbol Parent { get; }

        public T SymbolAst { get; }
    }
}
