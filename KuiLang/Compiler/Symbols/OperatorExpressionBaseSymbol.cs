using KuiLang.Semantic;
using KuiLang.Syntax;

namespace KuiLang.Compiler.Symbols
{
    public abstract class OperatorExpressionBaseSymbol<T> : IExpressionSymbol, ISymbol where T : Ast.Expression.Operator
    {
        public OperatorExpressionBaseSymbol(ISymbol parent, T symbolAst )
        {
            Parent = parent;
            SymbolAst = symbolAst;
        }

        public ISymbol Parent { get; }
        public IExpressionSymbol Left { get; internal set; } = null!;
        public IExpressionSymbol Right { get; internal set; } = null!;
        public T SymbolAst { get; }

        public TypeSymbol ReturnType { get; internal set; } = null!;

    }

    public class MultiplyExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Multiply>
    {
        public MultiplyExpressionSymbol( ISymbol parent, Ast.Expression.Operator.Multiply symbolAst ) : base( parent, symbolAst )
        {
        }
    }

    public class DivideExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Divide>
    {
        public DivideExpressionSymbol( ISymbol parent, Ast.Expression.Operator.Divide symbolAst ) : base( parent, symbolAst )
        {
        }
    }

    public class AddExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Add>
    {
        public AddExpressionSymbol( ISymbol parent, Ast.Expression.Operator.Add symbolAst ) : base( parent, symbolAst )
        {
        }
    }

    public class SubtractExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Subtract>
    {
        public SubtractExpressionSymbol( ISymbol parent, Ast.Expression.Operator.Subtract symbolAst ) : base( parent, symbolAst )
        {
        }
    }
}
