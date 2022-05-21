using KuiLang.Semantic;
using KuiLang.Syntax;

namespace KuiLang.Compiler.Symbols
{
    public abstract class OperatorExpressionBaseSymbol<T> : IExpressionSymbol, ISymbol<T> where T : Ast.Expression.Operator
    {
        public OperatorExpressionBaseSymbol( IExpressionSymbol left, IExpressionSymbol right, T symbolAst )
        {
            Left = left;
            Right = right;
            SymbolAst = symbolAst;
        }

        public IExpressionSymbol Left { get; }
        public IExpressionSymbol Right { get; }
        public T SymbolAst { get; }

        public TypeSymbol ReturnType { get; internal set; } = null!;

    }

    public class MultiplyExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Multiply>
    {
        public MultiplyExpressionSymbol( IExpressionSymbol left, IExpressionSymbol right, Ast.Expression.Operator.Multiply symbolAst ) : base( left, right, symbolAst )
        {
        }
    }

    public class DivideExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Divide>
    {
        public DivideExpressionSymbol( IExpressionSymbol left, IExpressionSymbol right, Ast.Expression.Operator.Divide symbolAst ) : base( left, right, symbolAst )
        {
        }
    }

    public class AddExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Add>
    {
        public AddExpressionSymbol( IExpressionSymbol left, IExpressionSymbol right, Ast.Expression.Operator.Add symbolAst ) : base( left, right, symbolAst )
        {
        }
    }

    public class SubstractExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Substract>
    {
        public SubstractExpressionSymbol( IExpressionSymbol left, IExpressionSymbol right, Ast.Expression.Operator.Substract symbolAst ) : base( left, right, symbolAst )
        {
        }
    }
}
