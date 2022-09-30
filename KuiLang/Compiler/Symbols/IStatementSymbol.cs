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
            if( parent is ProgramRootSymbol root )
            {
                if( root.MainFunction != null ) throw new InvalidOperationException();
                root.MainFunction = new FunctionExpressionSymbol( root, "Main", null )
                {
                    Statement = this
                };
                Parent = root.MainFunction;
                return;
            }
            Parent = parent;
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
