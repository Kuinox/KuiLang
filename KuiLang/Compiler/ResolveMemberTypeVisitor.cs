using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using KuiLang.Semantic;

namespace KuiLang.Compiler
{
    public class ResolveMemberTypeVisitor : SymbolVisitor<object>
    {
        readonly DiagnosticChannel _diagnostics;

        public ResolveMemberTypeVisitor( DiagnosticChannel diagnostics )
        {
            _diagnostics = diagnostics;
        }

        protected override object Visit( MethodSymbol symbol )
        {
            symbol.ReturnType = symbol.FindType( symbol.Ast.ReturnTypeIdentifier );
            return base.Visit( symbol );
        }

        protected override object Visit( MethodParameterSymbol symbol )
        {
            symbol.Type = symbol.Parent.FindType( symbol.Ast.TypeIdentifier );
            return base.Visit( symbol );
        }

        protected override object Visit( FieldSymbol symbol )
        {
            symbol.Type = symbol.Parent.FindType( symbol.SymbolAst.TypeIdentifier );
            return base.Visit( symbol );
        }

        protected override object Visit( VariableSymbol symbol )
        {
            symbol.Type = symbol.FindType( symbol.Ast.TypeIdentifier );
            return base.Visit( symbol );
        }

        protected override object Visit( MethodCallExpressionSymbol symbol )
        {
            symbol.TargetMethod = symbol.FindMethod( symbol.Ast.FunctionIdentifier );
            return base.Visit( symbol );
        }

        protected override object Visit( IdentifierValueExpressionSymbol symbol )
        {
            symbol.Field = symbol.FindIdentifierValueDeclaration( symbol.Ast.Identifier );
            return base.Visit( symbol );
        }
    }
}
