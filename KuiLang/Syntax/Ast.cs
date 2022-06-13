using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static KuiLang.Syntax.Ast.Expression;

namespace KuiLang.Syntax
{
    public abstract record Ast
    {
        public abstract record Statement : Ast
        {
            public sealed record Block( IReadOnlyList<Statement> Statements ) : Statement;
            public abstract record Definition( string Name ) : Statement
            {
                public sealed record TypeDeclaration(
                string Name,
                IReadOnlyList<Definition> Fields ) : Definition( Name );


                public sealed record Parameter( FieldLocation SignatureType, string Name, Expression? InitValue ) : Definition( Name );
                public sealed record FieldDeclaration( FieldLocation SignatureType, string Name, Expression? InitValue ) : Definition( Name );
                public sealed record MethodDeclaration(
                    FieldLocation ReturnType, string Name, IReadOnlyList<Parameter> Arguments,
                    Statement TheStatement
                ) : Definition( Name );
            }

            public sealed record MethodCallStatement( MethodCall MethodCallExpression ) : Statement;

            public sealed record Return(
                Expression? ReturnedValue
            ) : Statement;

            public sealed record If(
                Expression Condition,
                Statement TheStatement
            ) : Statement;


            public sealed record FieldAssignation( FieldLocation VariableLocation, Expression NewFieldValue ) : Statement;

        }


        public abstract record Expression : Ast
        {
            public sealed record MethodCall( FieldLocation FunctionToCall, IReadOnlyList<Expression> Arguments ) : Expression;
            public sealed record FieldReference( FieldLocation VariableLocation ) : Expression;
            public record Literal : Expression
            {
                public sealed record Number( decimal Value ) : Literal();
            }

            public record Operator( Expression Left, Expression Right ) : Expression
            {
                public sealed record Multiply( Expression Left, Expression Right ) : Operator( Left, Right );
                public sealed record Divide( Expression Left, Expression Right ) : Operator( Left, Right );
                public sealed record Add( Expression Left, Expression Right ) : Operator( Left, Right );
                public sealed record Subtract( Expression Left, Expression Right ) : Operator( Left, Right );
            }
        }
    }

}
