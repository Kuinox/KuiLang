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
            return Visit( symbol.Statement );
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
            foreach( var arguments in symbol.ParameterSymbols )
            {
                Visit( arguments.Value );
            }

            Visit( symbol.Statement );
            return default!;
        }

        protected virtual T Visit( MethodParameterSymbol symbol ) => default!;

        protected virtual T Visit( StatementSymbol symbolBase ) => symbolBase switch
        {
            MethodCallStatementSymbol s => Visit( s ),
            FieldAssignationStatementSymbol s => Visit( s ),
            VariableAssignationStatementSymbol s => Visit( s ),
            ReturnStatementSymbol s => Visit( s ),
            StatementBlockSymbol s => Visit( s ),
            IfStatementSymbol s => Visit( s ),
            VariableSymbol s => Visit( s ),
            
            _ => throw new ArgumentException( $"Unknown statement symbol{symbolBase}." )
        };

        protected virtual T Visit( MethodCallStatementSymbol statement ) => Visit( statement.MethodCallExpression );
        protected virtual T Visit( FieldAssignationStatementSymbol statement ) => Visit( statement.NewFieldValue );
        protected virtual T Visit( ReturnStatementSymbol statement ) => statement.ReturnedValue != null ? Visit( statement.ReturnedValue ) : default!;
        protected virtual T Visit( IfStatementSymbol statement ) => Visit( statement.Statement );
        protected virtual T Visit( VariableSymbol variableDeclaration )
        {
            if( variableDeclaration.InitValue != null ) Visit( variableDeclaration.InitValue );
            return default!;
        }
        protected virtual T Visit( VariableAssignationStatementSymbol symbol ) => Visit( symbol.NewFieldValue );

        protected virtual T Visit( StatementBlockSymbol statement )
        {
            foreach( var item in statement.Statements )
            {
                Visit( item );
            }
            return default!;
        }

        protected virtual T Visit( IExpressionSymbol symbolBase ) => symbolBase switch
        {
            FieldReferenceExpressionSymbol s => Visit( s ),
            VariableReferenceExpressionSymbol s => Visit( s ),
            MethodCallExpressionSymbol s => Visit( s ),
            NumberLiteralSymbol s => Visit( s ),
            MultiplyExpressionSymbol s => Visit( s ),
            DivideExpressionSymbol s => Visit( s ),
            AddExpressionSymbol s => Visit( (IExpressionSymbol)s ),
            SubtractExpressionSymbol s => Visit( s ),
            _ => throw new ArgumentException( $"Unknown expression symbol {symbolBase}" )
        };

        protected virtual T Visit( FieldReferenceExpressionSymbol symbol ) => default!;
        protected virtual T Visit( VariableReferenceExpressionSymbol symbol ) => default!;
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
