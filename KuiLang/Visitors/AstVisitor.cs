using KuiLang.Syntax;
using System;
using static KuiLang.Syntax.Ast;
using static KuiLang.Syntax.Ast.Expression;
using static KuiLang.Syntax.Ast.Statement;
using static KuiLang.Syntax.Ast.Statement.Definition;

namespace KuiLang
{
    public abstract class AstVisitor<T>
    {
        public virtual T Visit(Ast ast) => ast switch
        {
            Statement statement => Visit(statement),
            Expression expression => Visit(expression),
            _ => throw new InvalidOperationException($"Unknown Ast{ast}")
        };

        protected virtual T Visit(Statement statement) => statement switch
        {
            Block block => Visit(block),
            Definition definition => Visit(definition),
            ExpressionStatement expression => Visit(expression),
            VariableDeclaration variable => Visit(variable),
            VariableAssignation assignation => Visit(assignation),
            Return returnStatement => Visit(returnStatement),
            _ => throw new InvalidOperationException($"Unknown Statement{statement}")
        };

        protected virtual T Visit(Return returnStatement) => Visit(returnStatement.ReturnedValue!);

        protected virtual T Visit(ExpressionStatement expressionStatement) => Visit(expressionStatement.TheExpression);

        protected virtual T Visit(VariableDeclaration variableDeclaration)
            => variableDeclaration.InitValue != null ? Visit(variableDeclaration.InitValue) : default!;

        protected virtual T Visit(VariableAssignation assignation) => Visit(assignation.NewVariableValue);

        protected virtual T Visit(Block statementBlock)
        {
            foreach (var statement in statementBlock.Statements)
            {
                Visit(statement);
            }
            return default!;
        }

        protected virtual T Visit(Definition definition) => definition switch
        {
            TypeDef type => Visit(type),
            Argument argument => Visit(argument),
            Field field => Visit(field),
            MethodSignature methodSignature => Visit(methodSignature),
            Method method => Visit(method),
            _ => throw new InvalidOperationException($"Unknown Definition {definition}")
        };

        protected virtual T Visit(TypeDef type)
        {
            foreach (var field in type.Fields)
            {
                Visit(field);
            }
            return default!;
        }

        protected virtual T Visit(Argument argument) => default!;
        protected virtual T Visit(Field field) => default!;
        protected virtual T Visit(MethodSignature methodSignature)
        {
            foreach (var argument in methodSignature.Arguments)
            {
                Visit(argument);
            }
            return default!;
        }

        protected virtual T Visit(Method method)
        {
            Visit(method.Signature);
            foreach (var statement in method.Statements.Statements)
            {
                Visit(statement);
            }
            return default!;
        }

        protected virtual T Visit(Expression expression) => expression switch
        {
            FunctionCall functionCall => Visit(functionCall),
            Variable variable => Visit(variable),
            _ => throw new InvalidOperationException($"Unknown Statement{expression}")
        };

        protected virtual T Visit(FunctionCall functionCall)
        {
            foreach (var argument in functionCall.Arguments)
            {
                Visit(argument);
            }
            return default!;
        }

        protected virtual T Visit(Variable variable) => default!;
    }
}
