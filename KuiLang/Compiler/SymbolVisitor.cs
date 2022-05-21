using KuiLang.Compiler.Symbols;
using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler
{
    public abstract class SymbolVisitor<T>
    {
        public virtual T Visit( ProgramRootSymbol symbol )
        {
            foreach( var type in symbol.TypesSymbols.Values )
            {
                Visit( type );
            }
            return default!;
        }

        protected virtual T Visit( TypeSymbol symbol )
        {
            foreach( var field in symbol.Fields.Values )
            {
                Visit( field );
            }

            foreach( var method in symbol.Methods.Values )
            {
                Visit( method );
            }
            return default!;
        }

        protected virtual T Visit( FieldSymbol symbol ) => default!;

        protected virtual T Visit( MethodSymbol symbol )
        {
            foreach( var arguments in symbol.ParameterSymbols.Values )
            {
                Visit( arguments );
            }

            Visit( symbol.Statement );
            return default!;
        }

        protected virtual T Visit( MethodParameterSymbol symbol ) => default!;

        protected virtual T Visit( IStatementSymbol symbol ) => symbol switch
        {
            ExpressionStatementSymbol expression => Visit( expression ),
            FieldAssignationStatementSymbol fieldAssignation => Visit( fieldAssignation ),
            ReturnStatementSymbol returnStatement => Visit( returnStatement ),
            StatementBlockSymbol statement => Visit( statement ),
            IfStatementSymbol ifStatement => Visit( ifStatement ),
            _ => throw new ArgumentException( $"Unknown statement symbol{symbol}." )
        };

        protected virtual T Visit( ExpressionStatementSymbol statement ) => Visit( statement.Expression );
        protected virtual T Visit( FieldAssignationStatementSymbol statement ) => Visit( statement.NewFieldValue );
        protected virtual T Visit( ReturnStatementSymbol statement ) => statement.ReturnedValue != null ? Visit( statement.ReturnedValue ) : default!;
        protected virtual T Visit( IfStatementSymbol statement ) => Visit( statement.Statement );
        protected virtual T Visit( StatementBlockSymbol statement )
        {
            foreach( var item in statement.Statements )
            {
                Visit( item );
            }
            return default!;
        }

        protected virtual T Visit( IExpressionSymbol symbol ) => symbol switch
        {
            FieldReferenceExpressionSymbol fieldReference => Visit( fieldReference ),
            MethodCallExpressionSymbol methodCall => Visit( methodCall ),
            NumberLiteralSymbol numberLiteral => Visit( numberLiteral ),
            MultiplyExpressionSymbol multiply => Visit( multiply ),
            DivideExpressionSymbol divide => Visit( divide ),
            AddExpressionSymbol add => Visit( add ),
            SubtractExpressionSymbol subtract => Visit( subtract ),
            _ => throw new ArgumentException( $"Unknown expression symbol {symbol}" )
        };

        protected virtual T Visit( FieldReferenceExpressionSymbol symbol ) => default!;
        protected virtual T Visit( MethodCallExpressionSymbol symbol )
        {
            foreach( var arg in symbol.Arguments )
            {
                Visit( arg );
            }
            return default!;
        }

        protected virtual T Visit( NumberLiteralSymbol numberLiteral ) => default!;
        protected virtual T Visit( MultiplyExpressionSymbol multiply )
        {
            Visit( multiply.Left );
            Visit( multiply.Right );
            return default!;
        }
        protected virtual T Visit( DivideExpressionSymbol divide )
        {
            Visit( divide.Left );
            Visit( divide.Right );
            return default!;
        }
        protected virtual T Visit( AddExpressionSymbol add )
        {
            Visit( add.Left );
            Visit( add.Right );
            return default!;
        }

        protected virtual T Visit( SubtractExpressionSymbol multiply )
        {
            Visit( multiply.Left );
            Visit( multiply.Right );
            return default!;
        }
    }
}
