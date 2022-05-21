using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Linq;
using static KuiLang.Syntax.Ast.Expression;
using static KuiLang.Syntax.Ast.Expression.Literal;
using static KuiLang.Syntax.Ast.Statement.Definition;

namespace KuiLang.Compiler
{
    public class SymbolTableBuilderVisitor : AstVisitor<object>
    {
        readonly DiagnosticChannel _diagnostics;
        public SymbolTableBuilderVisitor( DiagnosticChannel diagnostics )
        {
            _diagnostics = diagnostics;
        }

        ISymbol<Ast> _current = null!;

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
            var current = (TypeSymbol)_current;
            var symbol = new MethodSymbol( current, method );
            current.Add( symbol );
            _current = (ISymbol<Ast>)symbol;
            base.Visit( method );
            _current = (ISymbol<Ast>)symbol.Parent;
            return default!;
        }

        protected override object Visit( TypeDeclaration type )
        {
            var current = (ProgramRootSymbol)_current;
            var symbol = new TypeSymbol( current, type );
            current.Add( symbol );
            _current = (ISymbol<Ast>)symbol;
            base.Visit( type );
            _current = symbol.Parent;
            return default!;
        }

        protected override object Visit( FieldDeclaration field )
        {
            if( _current is TypeSymbol type )
            {
                var symbol = new FieldSymbol( field, type );
                type.Add( symbol );
                _current = (ISymbol<Ast>)symbol;
                symbol.InitValue = field.InitValue != null ? Visit( field.InitValue ) : null;
                _current = (ISymbol<Ast>)symbol.Parent;
                return default!;
            }

            if( _current is ISymbolWithAStatement singleStatement )
            {
                _diagnostics.EmitDiagnostic( Diagnostic.FieldSingleStatement( field ) );
                var symbol = new VariableDeclarationSymbol( SingleOrMultiStatementSymbol.From( singleStatement ), null!, field );
                singleStatement.Statement = symbol;
                _current = (ISymbol<Ast>)symbol;
                symbol.InitValue = field.InitValue != null ? Visit( field.InitValue ) : null;
                _current = (ISymbol<Ast>)symbol.Parent;
                return default!;
            }

            var block = _current as StatementBlockSymbol;

            base.Visit( field );
            return default!;
        }

        // Statements:

        protected override object Visit( Ast.Statement.Block statementBlock )
        {
            return base.Visit( statementBlock );
        }

        protected override object Visit( Ast.Statement.FieldAssignation assignation )
        {
            var symbol = new FieldAssignationStatementSymbol(
                SingleOrMultiStatementSymbol.From( _current ),
                assignation
            );
            var prev = _current;
            _current = (ISymbol<Ast>)symbol;
            symbol.NewFieldValue = Visit( assignation.NewFieldValue );
            base.Visit( assignation );
            _current = prev;
            return default!;
        }

        protected override object Visit( Ast.Statement.If @if )
        {
            var symbol = new IfStatementSymbol( SingleOrMultiStatementSymbol.From( _current ), @if );
            var prev = _current;
            _current = (ISymbol<Ast>)symbol;
            base.Visit( @if );
            _current = prev;
            return default!;
        }

        protected override object Visit( Ast.Statement.Return returnStatement )
        {
            var symbol = new ReturnStatementSymbol( SingleOrMultiStatementSymbol.From( _current ), returnStatement );
            var prev = _current;
            _current = (ISymbol<Ast>)symbol;
            base.Visit( returnStatement );
            _current = prev;
            return default!;
        }


        protected override object Visit( Ast.Statement.ExpressionStatement expressionStatement )
        {
            var symbol = new ExpressionStatementSymbol( SingleOrMultiStatementSymbol.From( _current ), expressionStatement );
            var prev = _current;
            _current = (ISymbol<Ast>)symbol;
            symbol.Expression = Visit( expressionStatement.TheExpression );
            _current = prev;
            return default!;
        }

        // Expressions:

        protected override IExpressionSymbol Visit( Ast.Expression expression ) => (IExpressionSymbol)base.Visit( expression );

        protected override MethodCallExpressionSymbol Visit( MethodCall methodCall ) => new( methodCall, methodCall.Arguments.Select( Visit ).ToList() );

        protected override FieldReferenceExpressionSymbol Visit( FieldReference variable ) => new( variable );

        protected override IExpressionSymbol Visit( Literal literal ) => (IExpressionSymbol)base.Visit( literal );
        protected override NumberLiteralSymbol Visit( Number constant ) => new( constant );

        protected override IExpressionSymbol Visit( Operator @operator ) => (IExpressionSymbol)base.Visit( @operator );

        protected override AddExpressionSymbol Visit( Operator.Add add ) => new( Visit( add.Left ), Visit( add.Right ), add );
        protected override DivideExpressionSymbol Visit( Operator.Divide divide ) => new( Visit( divide.Left ), Visit( divide.Right ), divide );
        protected override MultiplyExpressionSymbol Visit( Operator.Multiply multiply ) => new( Visit( multiply.Left ), Visit( multiply.Right ), multiply );
        protected override SubtractExpressionSymbol Visit( Operator.Subtract subtract ) => new( Visit( subtract.Left ), Visit( subtract.Right ), subtract );
    }
}
