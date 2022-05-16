using KuiLang.Semantic;
using KuiLang.Syntax;
using KuiLang.Visitors;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Interpreter
{
    public class InterpreterVisitor : AstVisitor<
        object, object, object, object,
        object, object, object, object,
        object, object, object, object,
        object, object, object, object,
        object, object, object, object,
        object>
    {
        readonly Stack<Scope> _stack = new() { new() };
        readonly IReadOnlyDictionary<string, IReadOnlyCollection<SymbolBase>> _symbols;

        public InterpreterVisitor(IReadOnlyDictionary<string, IReadOnlyCollection<SymbolBase>> symbols)
        {
            _symbols = symbols;
        }

        public override string Test() => null!;

        Scope CurrentScope => _stack.Peek();

        public override object Visit(Ast ast)
        {
            var res = base.Visit(ast);
            if (res is ReturnControlFlow rcf) return rcf.ReturnValue!;
            return default!;
        }

        }

        protected override object Visit(Ast.Statement.Definition definition)
        {
            return default!; // we don't care of the definitions
        }

        protected override object Visit(Ast.Statement.VariableDeclaration variableDeclaration)
        {
            CurrentScope.AddVariable(variableDeclaration.Type, variableDeclaration.Name);
            CurrentScope.SetVariable(variableDeclaration.Name,
                base.Visit(variableDeclaration)
            );
            return default!;
        }

        protected override object Visit(Ast.Statement.FieldAssignation assignation)
        {
            var scope = LocateScope(assignation.VariableLocation);
            scope.SetVariable(assignation.VariableLocation.Parts.Span[^1], base.Visit(assignation));
            return default!;
        }

        protected override object Visit(Ast.Expression.FieldReference variable)
        {
            Scope scope = LocateScope(variable.VariableLocation);
            return scope.GetVariableValue(variable.VariableLocation.Parts.Span[^1]);
        }

        protected override object Visit(Ast.Expression.Constant constant) => constant.Value;

        protected override object Visit(Ast.Expression.Add add) => (decimal)Visit(add.Left) + (decimal)Visit(add.Right);
        protected override object Visit(Ast.Expression.Substract add) => (decimal)Visit(add.Left) - (decimal)Visit(add.Right);
        protected override object Visit(Ast.Expression.Multiply add) => (decimal)Visit(add.Left) * (decimal)Visit(add.Right);
        protected override object Visit(Ast.Expression.Divide add) => (decimal)Visit(add.Left) / (decimal)Visit(add.Right);

        protected override object Visit(Ast.Statement.ExpressionStatement expressionStatement)
        {
            base.Visit(expressionStatement.TheExpression);
            return null!;
        }

        protected override object Visit(Ast.Expression.MethodCall functionCall)
        {
            var newScope = new Scope();
            var argumentValues = functionCall.Arguments.Select(Visit).ToArray();
            var resolvedFunction = ResolveMethodSymbol(functionCall.FunctionToCall, argumentValues);
            var argsDef = resolvedFunction.Method.Signature.Arguments;
            for (int i = 0; i < functionCall.Arguments.Count; i++)
            {
                var value = argumentValues[i];
                newScope.AddVariable(argsDef[i].SignatureType, argsDef[i].Name);
                newScope.SetVariable(argsDef[i].Name, value);
            }

            _stack.Push(newScope);

            var val = Visit(resolvedFunction.Method.Statements);
            if (val is ReturnControlFlow rcf) return rcf.ReturnValue!;
            return default!;
        }


        protected override object Visit(Ast.Statement.Block statementBlock)
        {
            foreach (var statement in statementBlock.Statements)
            {
                var val = Visit(statement);
                if (val is ReturnControlFlow returning) return returning;
            }
            return null!;
        }

        protected override object Visit(Ast.Statement.If @if)
        {
            var ret = (decimal)Visit(@if.Condition);
            if (ret == 1)
            {
                return Visit(@if.TheStatement);
            }
            return default!;
        }

        record ControlFlow();
        record ReturnControlFlow(object? ReturnValue) : ControlFlow;
        protected override object Visit(Ast.Statement.Return returnStatement)
            => returnStatement.ReturnedValue != null ?
                new ReturnControlFlow(Visit(returnStatement.ReturnedValue))
                : new ReturnControlFlow(null);

        MethodSymbol ResolveMethodSymbol(FieldLocation symbolLocation, object[] arguments)
        {
            //var searchingPart = symbolLocation.Parts.Span[0];
            //foreach (Scope? scope in _stack)
            //{
            //    scope.TryGetVariable(searchingPart)
            //}
            return null!;
        }
        //=> _symbols[symbolLocation.ToString()];

        Scope LocateScope(FieldLocation location)
        {
            if (location.Parts.Length > 1) throw new NotImplementedException();
            return CurrentScope;
        }
    }
}
