using KuiLang.Compiler;
using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using KuiLang.Semantic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KuiLang.Interpreter
{
    public class InterpreterVisitor : SymbolVisitor<object>
    {
        readonly Stack<RuntimeObject> _stack = new();
        readonly DiagnosticChannel _diagnostics;

        public InterpreterVisitor( DiagnosticChannel diagnostics )
        {
            _diagnostics = diagnostics;
        }

        public static object _constructorRef = new();
        public override RuntimeReference Visit( ProgramRootSymbol root )
        {
            var rootScope = new RuntimeObject();
            _stack.Push(rootScope);
            foreach( var type in root.TypesSymbols.Values )
            {
                var heapType = new RuntimeObject();
                rootScope.Fields[type] = heapType;
                heapType.Fields[_constructorRef] = type.Constructor;
            }

            var res = Visit( root.MainFunction.Statement );
            _stack.Pop();
            if( res is ReturnControlFlow rcf ) return rcf.ReturnValue!;
            return default!;
        }
        protected override object Visit( VariableSymbol variableDeclaration )
        {
            _stack.Peek().Fields.Add( variableDeclaration, variableDeclaration.InitValue != null ? Visit( variableDeclaration.InitValue ) : null! );
            return base.Visit( variableDeclaration );
        }

        protected override object Visit( FieldSymbol variableDeclaration )
        {
            _stack.ToArray()[^2].Fields.Add( variableDeclaration, variableDeclaration.InitValue != null ? Visit( variableDeclaration.InitValue ) : null! );
            return base.Visit( variableDeclaration );
        }

        protected override object Visit( FieldAssignationStatementSymbol symbol )
        {
            var scope = LocateSymbolScope( symbol.AssignedField );
            scope.Fields[symbol.AssignedField] = Visit( symbol.NewFieldValue );
            return default!; // assignement is a statement.
            // I don't want 'if(thing = true)' errors happening.
        }

        protected override RuntimeReference Visit( IExpressionSymbol symbolBase ) => (RuntimeReference)base.Visit( symbolBase );

        protected override RuntimeReference Visit( InstantiateObjectExpression symbol )
        {
            var obj = new RuntimeObject();
            var root = _stack.ToArray()[0];
            root.Fields[obj] = obj;
            return new RuntimeReference( root, obj );
        }

        protected override RuntimeReference Visit( IdentifierValueExpressionSymbol variable )
            => new( LocateSymbolScope( variable.Field ), variable.Field );

        protected override RuntimeReference Visit( NumberLiteralSymbol constant )
        {
            var root = _stack.ToArray()[0];
            var obj = new RuntimeObject();
            root.Fields[obj] = obj;
            obj.Fields.Add( constant.GetRoot().HardcodedSymbols.NumberValueField, constant.Value );
            return new RuntimeReference(root, obj);
        }

        protected override RuntimeReference Visit( FunctionCallExpressionSymbol methodCall )
        {
            var targetRef = Visit( methodCall.CallTarget );
            var callTarget = targetRef.Owner.Fields[targetRef.Field].AsT2;

            var newScope = new RuntimeObject();
            int i = 0;
            foreach( var parameter in methodCall.TargetMethod.Parameters.Values )
            {
                var expressionValue = methodCall.Arguments[i++];
                newScope.Fields[parameter] = Visit( expressionValue );
            }
            _stack.Push( targetRef.Owner );
            _stack.Push( newScope );

            var val = Visit( methodCall.TargetMethod.Statement );

            if( val is ReturnControlFlow rcf ) return rcf.ReturnValue!;
            return default!;
        }

        protected override object Visit( FunctionExpressionSymbol symbol )
            => throw new InvalidOperationException( "wat" );


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
            var retRef = Visit( @if.Condition );
            var retVal = retRef.Owner.Fields[retRef.Field].AsT0;
            if( (decimal)retVal.Fields[@if.GetRoot().HardcodedSymbols.NumberValueField].AsT1 == 1 )
            {
                return Visit( @if.Statement );
            }
            return default!;
        }

        protected override RuntimeReference Visit( HardcodedExpressionsSymbol.NumberAddSymbol symbol )
        {
            var numberField = symbol.GetRoot().HardcodedSymbols.NumberValueField;
            var left = (decimal)GetCurrentInstance().Fields[numberField].AsT1;
            var right = (decimal)_stack.Peek().Fields[numberField].AsT1;
            var obj = new RuntimeObject()
            {
                Fields =
                {
                    {symbol.GetRoot().HardcodedSymbols.NumberValueField, left+right }
                }
            };
            return new RuntimeReference( obj, obj.Fields.Single() );
        }

        protected override RuntimeReference Visit( HardcodedExpressionsSymbol.NumberDivideSymbol symbol )
        {
            var numberField = symbol.GetRoot().HardcodedSymbols.NumberValueField;
            var left = (decimal)GetCurrentInstance().Fields[numberField].AsT1;
            var right = (decimal)_stack.Peek().Fields[numberField].AsT1;
            var obj = new RuntimeObject()
            {
                Fields =
                {
                    {symbol.GetRoot().HardcodedSymbols.NumberValueField, left/right }
                }
            };
            return new RuntimeReference( obj, obj.Fields.Single() );
        }

        protected override RuntimeReference Visit( HardcodedExpressionsSymbol.NumberMultiplySymbol symbol )
        {
            var numberField = symbol.GetRoot().HardcodedSymbols.NumberValueField;
            var left = (decimal)GetCurrentInstance().Fields[numberField].AsT1;
            var right = (decimal)_stack.Peek().Fields[numberField].AsT1;
            var obj = new RuntimeObject()
            {
                Fields =
                {
                    {symbol.GetRoot().HardcodedSymbols.NumberValueField, left*right }
                }
            };
            return new RuntimeReference( obj, obj.Fields.Single() );
        }

        protected override RuntimeReference Visit( HardcodedExpressionsSymbol.NumberSubstractSymbol symbol )
        {
            var numberField = symbol.GetRoot().HardcodedSymbols.NumberValueField;
            var left = (decimal)GetCurrentInstance().Fields[numberField].AsT1;
            var right = (decimal)_stack.Peek().Fields[numberField].AsT1;
            var obj = new RuntimeObject()
            {
                Fields =
                {
                    {symbol.GetRoot().HardcodedSymbols.NumberValueField, left-right }
                }
            };
            return new RuntimeReference( obj, obj.Fields.Single() );
        }

        record ControlFlow();
        record ReturnControlFlow( RuntimeReference? ReturnValue ) : ControlFlow;
        protected override object Visit( ReturnStatementSymbol returnStatement )
            => returnStatement.ReturnedValue != null ?
                new ReturnControlFlow( Visit( returnStatement.ReturnedValue ) )
                : new ReturnControlFlow( null );

        RuntimeObject GetCurrentInstance() => _stack.ToArray()[^2];

        RuntimeObject LocateSymbolScope( ISymbol symbol )
        {
            return symbol switch
            {
                TypeSymbol => _stack.ToArray()[0],
                FieldSymbol => GetCurrentInstance(),
                VariableSymbol => _stack.Peek(),
                _ => throw new InvalidOperationException()
            };
        }
    }
}
