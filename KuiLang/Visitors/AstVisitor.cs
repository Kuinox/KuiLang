using KuiLang.Compiler.Symbols;
using KuiLang.Syntax;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static KuiLang.Syntax.Ast;
using static KuiLang.Syntax.Ast.Expression;
using static KuiLang.Syntax.Ast.Expression.Literal;
using static KuiLang.Syntax.Ast.Expression.Operator;
using static KuiLang.Syntax.Ast.Statement;
using static KuiLang.Syntax.Ast.Statement.Definition;

namespace KuiLang
{
    public abstract class AstVisitor<T>
    {
        public virtual object Test() => null!;

        public virtual object Visit( Ast ast ) => ast switch
        {
            Statement statement => Visit( statement ),
            Expression expression => Visit( expression ),
            _ => throw new InvalidOperationException( $"Unknown Ast{ast}" )
        };

        protected virtual object Visit( Statement statement ) => statement switch
        {
            Block block => Visit( block ),
            Definition definition => Visit( definition ),
            ExpressionStatement expression => Visit( expression ),
            FieldAssignation assignation => Visit( assignation ),
            Return returnStatement => Visit( returnStatement ),
            If @if => Visit( @if ),
            _ => throw new InvalidOperationException( $"Unknown Statement{statement}" )
        };

        protected virtual object Visit( If @if )
        {
            Visit( @if.Condition );
            Visit( @if.TheStatement );
            return default!;
        }

        protected virtual object Visit( Return returnStatement )
            => Visit( returnStatement.ReturnedValue! );

        protected virtual object Visit( ExpressionStatement expressionStatement )
            => Visit( expressionStatement.TheExpression );

        protected virtual object Visit( FieldAssignation assignation )
            => Visit( assignation.NewFieldValue );

        protected virtual object Visit( Block statementBlock )
        {
            foreach( var statement in statementBlock.Statements )
            {
                Visit( statement );
            }
            return default!;
        }

        protected virtual object Visit( Definition definition ) => definition switch
        {
            TypeDeclaration type => Visit( type ),
            Parameter argument => Visit( argument ),
            FieldDeclaration field => Visit( field ),
            MethodDeclaration method => Visit( method ),
            _ => throw new InvalidOperationException( $"Unknown Definition {definition}" )
        };

        protected virtual object Visit( TypeDeclaration type )
        {
            foreach( var field in type.Fields )
            {
                Visit( field );
            }
            return default!;
        }

        protected virtual object Visit( Parameter argument ) => default!;
        protected virtual object Visit( FieldDeclaration field ) => default!;

        protected virtual object Visit( MethodDeclaration method )
        {
            foreach( var arg in method.Arguments )
            {
                Visit( arg );
            }

            Visit( method.TheStatement );

            return default!;
        }

        protected virtual object Visit( Expression expression ) => expression switch
        {
            MethodCall functionCall => Visit( functionCall ),
            FieldReference variable => Visit( variable ),
            Literal literal => Visit( literal ),
            Operator @operator => Visit( @operator ),
            _ => throw new InvalidOperationException( $"Unknown Statement{expression}" )
        };

        protected virtual object Visit( Operator @operator ) => @operator switch
        {
            Add add => Visit( add ),
            Substract sub => Visit( sub ),
            Multiply multiply => Visit( multiply ),
            Divide divide => Visit( divide ),
            _ => throw new InvalidOperationException( $"Unknown operator{@operator}" )
        };

        protected virtual object Visit( Add add )
        {
            Visit( add.Left );
            Visit( add.Right );
            return default!;
        }

        protected virtual object Visit( Substract substract )
        {
            Visit( substract.Left );
            Visit( substract.Right );
            return default!;
        }

        protected virtual object Visit( Multiply multiply )
        {
            Visit( multiply.Left );
            Visit( multiply.Right );
            return default!;
        }

        protected virtual object Visit( Divide divide )
        {
            Visit( divide.Left );
            Visit( divide.Right );
            return default!;
        }

        protected virtual object Visit( Literal literal ) => literal switch
        {
            Number number => Visit( number ),
            _ => throw new InvalidOperationException( $"Unknown literal{literal}" )
        };

        protected virtual object Visit( Number number ) => default!;

        protected virtual object Visit( MethodCall methodCall )
        {
            foreach( var argument in methodCall.Arguments )
            {
                Visit( argument );
            }
            return default!;
        }

        protected virtual object Visit( FieldReference variable ) => default!;
    }
}
