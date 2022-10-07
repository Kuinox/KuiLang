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
            foreach( var method in symbol.Fields.Values )
            {
                Visit( method );
            }
            return Visit( symbol.MainFunction );
        }

        protected virtual T Visit( TypeSymbol symbol )
        {
            foreach( var field in symbol.Fields.Values )
            {
                Visit( field );
            }
            return default!;
        }

        protected virtual T Visit( FieldSymbol symbol )
        {
            if( symbol.InitValue != null ) Visit( symbol.InitValue );
            return default!;
        }

        protected virtual T Visit( FunctionExpressionSymbol symbol )
        {
            foreach( var arguments in symbol.Parameters)
            {
                Visit( arguments.Value );
            }

            var res = Visit( symbol.Statement );
            return default!;
        }

        protected virtual T Visit( ParameterSymbol symbol ) => default!;

        protected virtual T Visit( StatementSymbol symbolBase ) => symbolBase switch
        {
            ExpressionStatementSymbol s => Visit( s ),
            FieldAssignationStatementSymbol s => Visit( s ),
            ReturnStatementSymbol s => Visit( s ),
            StatementBlockSymbol s => Visit( s ),
            IfStatementSymbol s => Visit( s ),
            VariableSymbol s => Visit( s ),
            null => throw new NullReferenceException(),
            _ => throw new ArgumentException( $"Unknown statement symbol{symbolBase}." )
        };

        protected virtual T Visit( ExpressionStatementSymbol s ) => Visit( s.Expression );
        protected virtual T Visit( FieldAssignationStatementSymbol statement ) => Visit( statement.NewFieldValue );
        protected virtual T Visit( ReturnStatementSymbol statement ) => statement.ReturnedValue != null ? Visit( statement.ReturnedValue ) : default!;
        protected virtual T Visit( IfStatementSymbol statement )
        {
            Visit( statement.Condition );
            Visit( statement.Statement );
            return default!;
        }

        protected virtual T Visit( VariableSymbol variableDeclaration )
        {
            if( variableDeclaration.InitValue != null ) Visit( variableDeclaration.InitValue );
            return default!;
        }
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
            IdentifierValueExpressionSymbol s => Visit( s ),
            FunctionCallExpressionSymbol s => Visit( s ),
            NumberLiteralSymbol s => Visit( s ),
            HardcodedExpressionsSymbol s => Visit( s ),
            InstantiateObjectExpression s => Visit( s ),
            FunctionExpressionSymbol s => Visit(s),
            _ => throw new ArgumentException( $"Unknown expression symbol {symbol}" )
        };

        protected virtual T Visit( InstantiateObjectExpression symbol ) => default!;
        protected virtual T Visit( IdentifierValueExpressionSymbol symbol ) => default!;

        protected virtual T Visit( FunctionCallExpressionSymbol symbol )
        {
            Visit( symbol.CallTarget );
            foreach( var arg in symbol.Arguments )
            {
                Visit( arg );
            }
            return default!;
        }

        protected virtual T Visit( NumberLiteralSymbol numberLiteral ) => default!;

        protected virtual T Visit( HardcodedExpressionsSymbol symbol ) => symbol switch
        {
            HardcodedExpressionsSymbol.NumberAddSymbol s => Visit( s ),
            HardcodedExpressionsSymbol.NumberSubstractSymbol s => Visit( s ),
            HardcodedExpressionsSymbol.NumberMultiplySymbol s => Visit( s ),
            HardcodedExpressionsSymbol.NumberDivideSymbol s => Visit( s ),
            _ => throw new InvalidOperationException( "Unknown symbol." )
        };

        protected virtual T Visit( HardcodedExpressionsSymbol.NumberAddSymbol symbol ) => default!;
        protected virtual T Visit( HardcodedExpressionsSymbol.NumberSubstractSymbol symbol ) => default!;
        protected virtual T Visit( HardcodedExpressionsSymbol.NumberMultiplySymbol symbol ) => default!;
        protected virtual T Visit( HardcodedExpressionsSymbol.NumberDivideSymbol symbol ) => default!;
    }
}
