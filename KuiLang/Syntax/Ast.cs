using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Syntax
{
    public abstract record Ast
    {
        public abstract record Statement : Ast
        {
            public sealed record Block(IReadOnlyList<Statement> Statements) : Statement;
            public abstract record Definition(string Name) : Statement
            {
                public sealed record TypeDef(
                FieldLocation? AccessLevel,
                string Name,
                IReadOnlyList<Definition> Fields) : Definition(Name);


                //Kuinox: TBH I don't know what is the proper way to do it.
                //I choose this representation of the argument/field/signature only because it was fun.
                //PR a better representation if you know how it should be.
                public sealed record Argument(FieldLocation SignatureType, string Name) : Definition(Name);
                public sealed record Field(bool IsStatic, FieldLocation? AccessModifier, Argument Signature) : Definition(Signature.Name);
                public sealed record MethodSignature(Field Signature, IReadOnlyList<Argument> Arguments) : Definition(Signature.Name);
                public sealed record Method(
                    MethodSignature Signature,
                    Block Statements
                ) : Definition(Signature.Signature.Name);
            }

            public sealed record ExpressionStatement(Expression TheExpression) : Statement;
            public sealed record VariableDeclaration(FieldLocation Type, string Name, Expression? InitValue) : Statement;
            public sealed record VariableAssignation(FieldLocation VariableLocation, Expression NewVariableValue) : Statement;
            public sealed record Return(
                Expression? ReturnedValue
            ) : Statement;
        }


        public abstract record Expression : Ast
        {
            public sealed record FunctionCall(FieldLocation FunctionToCall, IReadOnlyList<Expression> Arguments) : Expression;
            public sealed record Variable(FieldLocation VariableLocation) : Expression;
        }
    }

}
