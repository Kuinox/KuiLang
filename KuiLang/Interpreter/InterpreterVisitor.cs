using Farkle.Builder.OperatorPrecedence;
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

        public override RuntimeObject Visit( ProgramRootSymbol root )
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

        protected override RuntimeObject Visit( IExpressionSymbol symbolBase ) => (RuntimeObject)base.Visit( symbolBase );

        protected override object Visit( InstantiateObjectExpression symbol )
            => new RuntimeObject( symbol.ReturnType );

        protected override object Visit( IdentifierValueExpressionSymbol variable )
        {
            var scope = LocateSymbolScope( variable.Field );
            bool res = scope.TryGetValue( variable.Field, out var val );
            if( !res ) throw new InvalidOperationException( "Runtime error: Couldn't retrieve value." );
            if( val == null ) throw new InvalidOperationException( "Runtime error: uninitialized value was accessed." );
            return val!;
        }

        protected override object Visit( NumberLiteralSymbol constant )
        {
            var obj = new RuntimeObject( HardcodedSymbols.NumberType );
            obj.Fields.Add( HardcodedSymbols.NumberValueField, constant.Value );
            return obj;
        }

        protected override RuntimeObject Visit( FunctionCallExpressionSymbol functionCall )
        {
            var newScope = new Dictionary<ISymbol, object>();
            int i = 0;
            foreach( var parameter in functionCall.TargetMethod.ParameterSymbols )
            {
                var expressionValue = functionCall.Arguments[i++];
                newScope[parameter.Value] = Visit( expressionValue );
            }
            _stack.Push( newScope );

            var val = Visit( functionCall.TargetMethod.Statement );
            if( val is ReturnControlFlow rcf ) return (RuntimeObject)rcf.ReturnValue!;
            return default!;
        }

        protected override RuntimeObject Visit( MethodCallExpressionSymbol methodCall )
        {

            var callTarget = Visit( methodCall.CallTarget );

            var newScope = new Dictionary<ISymbol, object>();
            int i = 0;
            foreach( var parameter in methodCall.TargetMethod.ParameterSymbols )
            {
                var expressionValue = methodCall.Arguments[i++];
                newScope[parameter.Value] = Visit( expressionValue );
            }

            _stack.Push( callTarget.Fields );

            _stack.Push( newScope );

            var val = Visit( methodCall.TargetMethod.Statement );

            if( val is ReturnControlFlow rcf ) return (RuntimeObject)rcf.ReturnValue!;
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
            var ret = Visit( @if.Condition );

            if( (decimal)ret.Fields[HardcodedSymbols.NumberValueField] == 1 )
            {
                return Visit( @if.Statement );
            }
            return default!;
        }

        protected override RuntimeObject Visit( HardcodedExpressionsSymbol.NumberAddSymbol symbol )
        {
            var left = (decimal)GetCurrentInstance()[HardcodedSymbols.NumberValueField];
            var right = (decimal)_stack.Peek().Values.Single();
            return new RuntimeObject( HardcodedSymbols.NumberType )
            {
                Fields =
                {
                    {HardcodedSymbols.NumberValueField, left+right }
                }
            };
        }

        protected override RuntimeObject Visit( HardcodedExpressionsSymbol.NumberDivideSymbol symbol )
        {
            var left = (decimal)GetCurrentInstance()[HardcodedSymbols.NumberValueField];
            var right = (decimal)_stack.Peek().Values.Single();
            return new RuntimeObject( HardcodedSymbols.NumberType )
            {
                Fields =
                {
                    {HardcodedSymbols.NumberValueField, left/right }
                }
            };
        }

        protected override RuntimeObject Visit( HardcodedExpressionsSymbol.NumberMultiplySymbol symbol )
        {
            var left = (decimal)GetCurrentInstance()[HardcodedSymbols.NumberValueField];
            var right = (decimal)_stack.Peek().Values.Single();
            return new RuntimeObject( HardcodedSymbols.NumberType )
            {
                Fields =
                {
                    {HardcodedSymbols.NumberValueField, left*right }
                }
            };
        }

        protected override RuntimeObject Visit( HardcodedExpressionsSymbol.NumberSubstractSymbol symbol )
        {
            var left = (decimal)GetCurrentInstance()[HardcodedSymbols.NumberValueField];
            var right = (decimal)_stack.Peek().Values.Single();
            return new RuntimeObject( HardcodedSymbols.NumberType )
            {
                Fields =
                {
                    {HardcodedSymbols.NumberValueField, left-right }
                }
            };
        }

        record ControlFlow();
        record ReturnControlFlow( RuntimeObject? ReturnValue ) : ControlFlow;
        protected override object Visit( ReturnStatementSymbol returnStatement )
            => returnStatement.ReturnedValue != null ?
                new ReturnControlFlow( Visit( returnStatement.ReturnedValue ) )
                : new ReturnControlFlow( null );

        Dictionary<ISymbol, object> GetCurrentInstance() => _stack.ToArray()[^2];

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
