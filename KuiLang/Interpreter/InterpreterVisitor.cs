using KuiLang.Compiler;
using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using KuiLang.Semantic;
using KuiLang.Syntax;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Interpreter
{
    public class InterpreterVisitor : SymbolVisitor<object>
    {
        readonly Stack<Dictionary<ISymbol, object>> _stack = new();
        readonly DiagnosticChannel _diagnostics;

        public InterpreterVisitor( DiagnosticChannel diagnostics )
        {
            _diagnostics = diagnostics;
        }

        Dictionary<ISymbol, object> Current => _stack.Peek();

        public override object Visit( ProgramRootSymbol root )
        {
            _stack.Push( new Dictionary<ISymbol, object>() );
            var res = Visit( root.Statement );
            _stack.Pop();
            if( res is ReturnControlFlow rcf ) return rcf.ReturnValue!;
            return default!;
        }
        protected override object Visit( VariableSymbol variableDeclaration )
        {
            Current.Add( variableDeclaration, variableDeclaration.InitValue != null ? Visit( variableDeclaration.InitValue ) : null! );
            return base.Visit( variableDeclaration );
        }

        protected override object Visit( FieldSymbol variableDeclaration )
        {
            Current.Add( variableDeclaration, variableDeclaration.InitValue != null ? Visit( variableDeclaration.InitValue ) : null! );
            return base.Visit( variableDeclaration );
        }

        protected override object Visit( FieldAssignationStatementSymbol symbol )
        {
            var scope = LocateSymbolScope( symbol.AssignedField );
            scope[symbol.AssignedField] = Visit( symbol.NewFieldValue );
            return default!; // assignement is a statement.
            // I don't want 'if(thing = true)' errors happening.
        }

        protected override object Visit( VariableAssignationStatementSymbol symbol )
        {
            var scope = LocateSymbolScope( symbol.AssignedField );
            scope[symbol.AssignedField] = Visit( symbol.NewFieldValue );
            return default!;
        }

        protected override object Visit( IdentifierValueExpressionSymbol variable )
            => LocateSymbolScope( variable.Field )[variable.Field];

        protected override object Visit( NumberLiteralSymbol constant ) => constant.Value;

        protected override object Visit( AddExpressionSymbol add ) => (decimal)Visit( add.Left ) + (decimal)Visit( add.Right );
        protected override object Visit( SubtractExpressionSymbol add ) => (decimal)Visit( add.Left ) - (decimal)Visit( add.Right );
        protected override object Visit( MultiplyExpressionSymbol add ) => (decimal)Visit( add.Left ) * (decimal)Visit( add.Right );
        protected override object Visit( DivideExpressionSymbol add ) => (decimal)Visit( add.Left ) / (decimal)Visit( add.Right );

        protected override object Visit( MethodCallExpressionSymbol functionCall )
        {
            var newScope = new Dictionary<ISymbol, object>();
            for( int i = 0; i < functionCall.Arguments.Count; i++ )
            {
                var expressionValue = functionCall.Arguments[i];
                var parameter = functionCall.TargetMethod.ParameterSymbols[i];
                newScope[parameter.Value] = Visit( expressionValue );
            }

            _stack.Push( newScope );

            var val = Visit( functionCall.TargetMethod.Statement );
            if( val is ReturnControlFlow rcf ) return rcf.ReturnValue!;
            return default!;
        }


        protected override object Visit( StatementBlockSymbol block )
        {
            foreach( var statement in block.Statements )
            {
                var val = Visit( statement );
                if( val is ReturnControlFlow returning ) return returning;
            }
            return null!;
        }

        protected override object Visit( IfStatementSymbol @if )
        {
            var ret = (decimal)Visit( @if.Condition );
            if( ret == 1 )
            {
                return Visit( @if.Statement );
            }
            return default!;
        }

        record ControlFlow();
        record ReturnControlFlow( object? ReturnValue ) : ControlFlow;
        protected override object Visit( ReturnStatementSymbol returnStatement )
            => returnStatement.ReturnedValue != null ?
                new ReturnControlFlow( Visit( returnStatement.ReturnedValue ) )
                : new ReturnControlFlow( null );

        Dictionary<ISymbol, object> LocateSymbolScope( ISymbol symbol )
        {
            if( symbol is null ) throw new ArgumentNullException( nameof( symbol ) );
            foreach( var item in _stack )
            {
                if( item.ContainsKey( symbol ) ) return item;
            }
            throw new InvalidOperationException();
        }
    }
}
