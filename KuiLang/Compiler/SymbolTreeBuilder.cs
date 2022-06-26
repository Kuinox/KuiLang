using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Diagnostics.SymbolStore;
using System.Linq;
using static KuiLang.Syntax.Ast.Expression;
using static KuiLang.Syntax.Ast.Expression.Literal;
using static KuiLang.Syntax.Ast.Statement.Definition;

namespace KuiLang.Compiler
{
    public class SymbolTreeBuilder : AstVisitor<object>
    {
        readonly DiagnosticChannel _diagnostics;
        public SymbolTreeBuilder( DiagnosticChannel diagnostics )
        {
            _diagnostics = diagnostics;
        }

        ISymbol _current = null!;

        public override ProgramRootSymbol Visit( Ast ast )
        {
            var root = new ProgramRootSymbol( ast );
            _current = root;
            base.Visit( ast );
            _current = null!;
            return root;
        }

        // Definitions :

        protected override object Visit( MethodDeclaration method )
        {
            var current = _current switch
            {
                ISymbolWithMethods methodHolder => methodHolder,
                StatementBlockSymbol block when block.Parent is ProgramRootSymbol root => root,
                _=> throw new InvalidOperationException("Unknown method parent")
            };
            var symbol = new MethodSymbol( current, method );
            current.Methods.Add( symbol.Name, symbol );
            _current = symbol;
            base.Visit( method );
            _current = symbol.Parent;
            return default!;
        }

        protected override object Visit( Parameter ast )
        {
            new MethodParameterSymbol( ast.Name, ast, (MethodSymbol)_current );
            return default!;
        }

        protected override object Visit( TypeDeclaration type )
        {
            var current = (ProgramRootSymbol)_current;
            var symbol = new TypeSymbol( current, type );
            current.Add( symbol );
            _current = symbol;
            base.Visit( type );
            _current = symbol.Parent;
            return default!;
        }

        protected override object Visit( FieldDeclaration field )
        {
            if( _current is TypeSymbol type )
            {
                var fieldSymbol = new FieldSymbol( field, type );
                type.Fields.Add( fieldSymbol.Name, fieldSymbol );
                _current = fieldSymbol;
                fieldSymbol.InitValue = field.InitValue != null ? Visit( field.InitValue ) : null;
                _current = fieldSymbol.Parent;
                return default!;
            }
            var symbol = new VariableSymbol( _current, null!, field );
            _current = symbol;
            symbol.InitValue = field.InitValue != null ? Visit( field.InitValue ) : null;
            _current = symbol.Parent!;

            if( _current is ISymbolWithAStatement singleStatement )
            {
                _diagnostics.EmitDiagnostic( Diagnostic.FieldSingleStatement( field ) );
            }
            return default!;
        }

        // Statements:

        protected override object Visit( Ast.Statement.Block ast )
        {
            var symbol = new StatementBlockSymbol( _current, ast );
            _current = symbol;
            var res = base.Visit( ast );
            _current = symbol.Parent!;
            return res;
        }

        protected override object Visit( Ast.Statement statement )
        {
            return base.Visit( statement );
        }

        protected override object Visit( Ast.Statement.FieldAssignation assignation )
        {
            var symbol = new FieldAssignationStatementSymbol( (StatementSymbol)_current,
                assignation
            );
            var prev = _current;
            _current = symbol;
            symbol.NewFieldValue = Visit( assignation.NewFieldValue );
            base.Visit( assignation );
            _current = prev;
            return default!;
        }

        protected override object Visit( Ast.Statement.If @if )
        {
            var symbol = new IfStatementSymbol( (StatementSymbol)_current, @if );
            var prev = _current;
            _current = symbol;
            symbol.Condition = Visit( @if.Condition );
            var res = Visit( @if.TheStatement );
            _current = prev;
            return res;
        }

        protected override object Visit( Ast.Statement.Return returnStatement )
        {
            var symbol = new ReturnStatementSymbol( _current, returnStatement );
            var prev = _current;
            _current = symbol;
            symbol.ReturnedValue = returnStatement.ReturnedValue != null ? Visit( returnStatement.ReturnedValue ) : null;
            _current = prev;
            return default!;
        }

        protected override object Visit( Ast.Statement.MethodCallStatement methodCallStatement )
        {
            var symbol = new MethodCallStatementSymbol( _current, methodCallStatement );
            var prev = _current;
            _current = symbol;
            symbol.MethodCallExpression = Visit( methodCallStatement.MethodCallExpression );
            _current = prev;
            return default!;
        }

        // Expressions:

        protected override IExpression Visit( Ast.Expression expression ) => (IExpression)base.Visit( expression );

        protected override MethodCallExpressionSymbol Visit( MethodCall methodCall )
        {
            var prev = _current;
            var expr = new MethodCallExpressionSymbol( _current, methodCall );
            _current = expr;
            expr.Arguments = methodCall.Arguments.Select( Visit ).ToList();
            _current = prev;
            return expr;
        }

        protected override IdentifierValueExpressionSymbol Visit( IdentifierValue variable )
            => new( _current, variable );

        protected override IExpression Visit( Literal literal ) => (IExpression)base.Visit( literal );
        protected override NumberLiteralSymbol Visit( Number constant ) => new( _current, constant );

        protected override AddExpressionSymbol Visit( Operator.Add add )
        {
            var prev = _current;
            var expr = new AddExpressionSymbol( _current, add );
            _current = expr;
            expr.Left = Visit( add.Left );
            expr.Right = Visit( add.Right );
            _current = prev;
            return expr;
        }

        protected override DivideExpressionSymbol Visit( Operator.Divide divide )
        {
            var prev = _current;
            var expr = new DivideExpressionSymbol( _current, divide );
            _current = expr;
            expr.Left = Visit( divide.Left );
            expr.Right = Visit( divide.Right );
            _current = prev;
            return expr;
        }

        protected override MultiplyExpressionSymbol Visit( Operator.Multiply multiply )
        {
            var prev = _current;
            var expr = new MultiplyExpressionSymbol( _current, multiply );
            _current = expr;
            expr.Left = Visit( multiply.Left );
            expr.Right = Visit( multiply.Right );
            _current = prev;
            return expr;
        }

        protected override SubtractExpressionSymbol Visit( Operator.Subtract subtract )
        {
            var prev = _current;
            var expr = new SubtractExpressionSymbol( _current, subtract );
            _current = expr;
            expr.Left = Visit( subtract.Left );
            expr.Right = Visit( subtract.Right );
            _current = prev;
            return expr;
        }
    }
}
