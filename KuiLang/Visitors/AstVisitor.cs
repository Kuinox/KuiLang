using KuiLang.Syntax;
using System;
using static KuiLang.Syntax.Ast;
using static KuiLang.Syntax.Ast.Expression;
using static KuiLang.Syntax.Ast.Expression.Literal;
using static KuiLang.Syntax.Ast.Expression.FuncCall;
using static KuiLang.Syntax.Ast.Statement;
using static KuiLang.Syntax.Ast.Statement.Definition;
using static KuiLang.Syntax.Ast.Statement.Definition.Typed;
using static KuiLang.Syntax.Ast.Expression.FuncCall.MethodCall;

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
            FieldAssignation assignation => Visit( assignation ),
            Return returnStatement => Visit( returnStatement ),
            If @if => Visit( @if ),
            ExpressionStatement expressionStatemtn => Visit( expressionStatemtn ),
            _ => throw new InvalidOperationException( $"Unknown Statement{statement}" )
        };

        protected virtual object Visit( ExpressionStatement expressionStatement )
            => Visit( expressionStatement.TheExpression );

        protected virtual object Visit( If @if )
        {
            Visit( @if.Condition );
            Visit( @if.TheStatement );
            return default!;
        }

        protected virtual object Visit( Return returnStatement )
            => Visit( returnStatement.ReturnedValue! );

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
            Definition.Type type => Visit( type ),
            Typed type => Visit( type ),
            _ => throw new InvalidOperationException( $"Unknown Definition {definition}" )
        };

        protected virtual object Visit( Typed typedDef ) => typedDef switch
        {
            Parameter argument => Visit( argument ),
            Field field => Visit( field ),
            Method method => Visit( method ),
            _ => throw new InvalidOperationException( $"Unknown Typed Definition {typedDef}" )
        };

        protected virtual object Visit( Definition.Type type )
        {
            foreach( var field in type.Fields )
            {
                Visit( field );
            }
            return default!;
        }

        protected virtual object Visit( Parameter parameter ) => default!;
        protected virtual object Visit( Field field ) => default!;

        protected virtual object Visit( Method method )
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
            FuncCall functionCall => Visit( functionCall ),
            IdentifierValue variable => Visit( variable ),
            Literal literal => Visit( literal ),
            _ => throw new InvalidOperationException( $"Unknown Statement{expression}" )
        };


        protected virtual object Visit( Literal literal ) => literal switch
        {
            Number number => Visit( number ),
            _ => throw new InvalidOperationException( $"Unknown literal{literal}" )
        };

        protected virtual object Visit( Number number ) => default!;

        protected virtual object Visit( FuncCall funcCall )
        {
            if( funcCall is MethodCall m ) Visit( m );

            foreach( var argument in funcCall.Arguments )
            {
                Visit( argument );
            }
            return default!;
        }

        protected virtual object Visit( MethodCall methodCall )
        {
            if( methodCall is Operator s ) Visit( s );
            return Visit( methodCall.Target );
        }

        protected virtual object Visit( Operator @operator ) => default!;

        protected virtual object Visit( IdentifierValue variable ) => default!;
    }
}
