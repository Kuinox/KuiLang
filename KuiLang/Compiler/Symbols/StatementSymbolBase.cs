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
    public abstract class StatementSymbolBase<T> : IStatementSymbol, ISymbol where T : Ast.Statement
    {
        public StatementSymbolBase( SingleOrMultiStatementSymbol parent, T symbolAst )
        {
            Parent = parent;
            SymbolAst = symbolAst;
            if( parent.IsSingle )
            {
                parent.AsSingle.Statement = this;
            }
            if( parent.IsBlock )
            {
                parent.AsBlock.Statements.Add( this );
            }
        }
        ISymbol? ISymbol.Parent => Parent.Value;
        public SingleOrMultiStatementSymbol Parent { get; }

        public T SymbolAst { get; }

    }
}
