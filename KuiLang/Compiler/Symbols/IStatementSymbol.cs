using KuiLang.Semantic;
using KuiLang.Syntax;
using OneOf;
using System;

namespace KuiLang.Compiler.Symbols
{
    public abstract class StatementSymbol : ISymbol
    {
        public ISymbol Parent { get; }

        public StatementSymbol( ISymbol parent )
        {
            Parent = parent;
            if( parent is ProgramRootSymbol root )
            {
                if( root.Statement != null ) throw new InvalidOperationException();
                root.Statement = (StatementBlockSymbol)this;
                return;
            }
            if( parent is StatementBlockSymbol block )
            {
                block.Statements.Add( this );
                return;
            }
            if( parent is ISymbolWithAStatement mono )
            {
                if( mono.Statement != null ) throw new InvalidOperationException( "Statement already set." );
                mono.Statement = this;
                return;
            }
        }

        protected virtual string ValueString => "\"TODO\"";

        public override string ToString() =>
$@"{{
""{GetType().Name}"":{ValueString}
}}";
    }
}
