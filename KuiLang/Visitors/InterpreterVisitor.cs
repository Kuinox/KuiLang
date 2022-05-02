using KuiLang.Semantic;
using KuiLang.Syntax;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Visitors
{
    public class InterpreterVisitor : AstVisitor<object>
    {
        readonly Stack<Scope> _stack = new() { new() };
        readonly IReadOnlyDictionary<string, ISymbol> _symbols;

        public InterpreterVisitor(IReadOnlyDictionary<string, ISymbol> symbols)
        {
            _symbols = symbols;
        }

        Scope CurrentScope => _stack.Peek();
        protected override object Visit(Ast.Statement.VariableDeclaration variableDeclaration)
        {
            CurrentScope.AddVariable(variableDeclaration.Type, variableDeclaration.Name);
            CurrentScope.SetVariable(variableDeclaration.Name,
                base.Visit(variableDeclaration)
            );
            return default!;
        }

        protected override object Visit(Ast.Statement.VariableAssignation assignation)
        {
            var scope = LocateScope(assignation.VariableLocation);
            scope.SetVariable(assignation.VariableLocation.Parts.Span[^1], base.Visit(assignation));
            return default!;
        }

        protected override object Visit(Ast.Expression.Variable variable)
        {
            Scope scope = LocateScope(variable.VariableLocation);
            return scope.GetVariableValue(variable.VariableLocation.Parts.Span[^1]);
        }

        protected override object Visit(Ast.Statement.ExpressionStatement expressionStatement)
        {
            base.Visit(expressionStatement.TheExpression);
            return null!;
        }

        protected override object Visit(Ast.Expression.FunctionCall functionCall)
        {
            var newScope = new Scope();

            var resolvedFunction = (MethodSymbol)ResolveSymbol(functionCall.FunctionToCall);
            var argsDef = resolvedFunction.Method.Signature.Arguments;
            for (int i = 0; i < functionCall.Arguments.Count; i++)
            {
                var value = Visit(functionCall.Arguments[i]);
                newScope.AddVariable(argsDef[i].SignatureType, argsDef[i].Name);
                newScope.SetVariable(argsDef[i].Name, value);
            }

            _stack.Push(newScope);

            return Visit(resolvedFunction.Method.Statements);
        }
        protected override object Visit(Ast.Statement.Block statementBlock)
        {
            foreach (var statement in statementBlock.Statements)
            {
                var val = Visit(statement);
                if (val is ReturnControlFlow returning) return returning.ReturnValue!;
            }
            return null!;
        }

        record ControlFlow();
        record ReturnControlFlow(object? ReturnValue) : ControlFlow;
        protected override object Visit(Ast.Statement.Return returnStatement)
            => returnStatement.ReturnedValue != null ?
                new ReturnControlFlow(Visit(returnStatement.ReturnedValue))
                : new ReturnControlFlow(null);

        ISymbol ResolveSymbol(FieldLocation symbolLocation)
            => _symbols[symbolLocation.ToString()];

        Scope LocateScope(FieldLocation location)
        {
            if (location.Parts.Length > 1) throw new NotImplementedException();
            return CurrentScope;
        }
    }
}
