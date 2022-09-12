using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static KuiLang.Syntax.Ast.Expression;
using static KuiLang.Syntax.Ast.Expression.FuncCall;

namespace KuiLang.Syntax
{
    public abstract record Ast
    {
        public abstract record Statement : Ast
        {
            public sealed record Block( IReadOnlyList<Statement> Statements ) : Statement;
            public abstract record Definition( string Name ) : Statement
            {
                public sealed record Type(
                string Name,
                IReadOnlyList<Definition> Fields ) : Definition( Name );

                public abstract record Typed( Identifier TypeIdentifier, string Name ) : Definition( Name )
                {
                    public sealed record Parameter( Identifier TypeIdentifier, string Name, Expression? InitValue ) : Typed( TypeIdentifier, Name );
                    public sealed record Field( Identifier TypeIdentifier, string Name, Expression? InitValue ) : Typed( TypeIdentifier, Name );
                    public sealed record Method(
                        Identifier ReturnTypeIdentifier, string Name, IReadOnlyList<Parameter> Arguments,
                        Statement TheStatement
                    ) : Typed( ReturnTypeIdentifier, Name );
                }


            }
            public sealed record ExpressionStatement( Expression TheExpression ) : Statement;

            public sealed record Return(
                Expression? ReturnedValue
            ) : Statement;

            public sealed record If(
                Expression Condition,
                Statement TheStatement
            ) : Statement;


            public sealed record FieldAssignation( Expression FieldSelector, Expression NewFieldValue ) : Statement;

        }


        public abstract record Expression : Ast
        {
            public record FuncCall( Expression CallTarget, IReadOnlyList<Expression> Arguments ) : Expression
            {
                public record Operator( string Name, Expression Left, Expression Right ) : FuncCall( Left, new MyList<Expression> { Right } )
                {
                    public sealed record Multiply( Expression Left, Expression Right ) : Operator( "*", Left, Right );
                    public sealed record Divide( Expression Left, Expression Right ) : Operator( "/", Left, Right );
                    public sealed record Add( Expression Left, Expression Right ) : Operator( "+", Left, Right );
                    public sealed record Subtract( Expression Left, Expression Right ) : Operator( "-", Left, Right );
                }
            }
            public sealed record IdentifierValue( Identifier Identifier ) : Expression;
            public record Literal : Expression
            {
                public sealed record Number( decimal Value ) : Literal();
            }


        }
    }

}
