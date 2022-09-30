using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Linq;
using static KuiLang.Syntax.Ast.Expression;
using static KuiLang.Syntax.Ast.Expression.Literal;
using static KuiLang.Syntax.Ast.Expression.FuncCall;
using static KuiLang.Syntax.Ast.Statement.Definition;
using static KuiLang.Syntax.Ast.Statement.Definition.Typed;

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

        protected override object Visit( Method method )
        {
            var methodType = _current.GetContainingType();
            var current = _current;
            var funcSymbol = new FunctionExpressionSymbol( methodType, method.Name, method );
            var field = new FieldSymbol( new Field( method.ReturnTypeIdentifier, method.Name, null ), methodType )
            {
                InitValue = funcSymbol
            };
            methodType.Fields.Add( method.Name, field);
            _current = funcSymbol;
            base.Visit( method );
            _current = current;
            return default!;
        }

        protected override object Visit( Parameter ast )
        {
            var curr = (FunctionExpressionSymbol)_current;
            var symbol = new ParameterSymbol( ast, curr );

            curr.Parameters.Add( ast.Name, symbol );

            return default!;
        }

        protected override object Visit( Ast.Statement.Definition.Type type )
        {
            var current = _current;
            var root = _current.GetRoot();
            var symbol = new TypeSymbol( root, type );
            root.Add( symbol );
            _current = symbol;
            base.Visit( type );
            _current = current;
            return default!;
        }

        protected override object Visit( Field field )
        {
            if( _current is TypeSymbol type )
            {
                var fieldSymbol = new FieldSymbol( field, type );
                type.Fields.Add( fieldSymbol.Ast.Name, fieldSymbol );
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
            var symbol = new FieldAssignationStatementSymbol
            (
                (StatementSymbol)_current,
                assignation
            );
            var prev = _current;
            _current = symbol;
            symbol.FieldOwner = (IdentifierValueExpressionSymbol)Visit( assignation.FieldSelector );
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

        protected override object Visit( Ast.Statement.ExpressionStatement expressionStatement )
        {
            var symbol = new ExpressionStatementSymbol( _current );
            var prev = _current;
            _current = symbol;
            symbol.Expression = Visit( expressionStatement.TheExpression );
            _current = prev;
            return default!;
        }

        // Expressions:

        protected override IExpressionSymbol Visit( Ast.Expression expression ) => (IExpressionSymbol)base.Visit( expression );

        
        protected override IExpressionSymbol Visit( FuncCall funcCall )
        {
            var prev = _current;
            var expr = new FunctionCallExpressionSymbol( _current, funcCall );
            _current = expr;
            expr.Arguments = funcCall.Arguments.Select( Visit ).ToList();
            expr.CallTarget = Visit( funcCall.CallTarget );
            _current = prev;
            return expr;
        }

        protected override IdentifierValueExpressionSymbol Visit( IdentifierValue variable )
            => new( _current, variable );

        protected override IExpressionSymbol Visit( Literal literal ) => (IExpressionSymbol)base.Visit( literal );
        protected override NumberLiteralSymbol Visit( Number constant ) => new( _current, constant );

    }
}
