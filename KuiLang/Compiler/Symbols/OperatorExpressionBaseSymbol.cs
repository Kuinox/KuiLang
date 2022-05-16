using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public abstract class OperatorExpressionBaseSymbol<T> : ExpressionBaseSymbol<T> where T : Ast.Expression.Operator
    {
        public OperatorExpressionBaseSymbol( ExpressionBaseSymbol<Ast.Expression> left, ExpressionBaseSymbol<Ast.Expression> right, T symbolAst ) : base( symbolAst )
        {
            Left = left;
            Right = right;
        }

        public ExpressionBaseSymbol<Ast.Expression> Left { get; }
        public ExpressionBaseSymbol<Ast.Expression> Right { get; }
    }

    public class MultiplyExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Multiply>
    {
        public MultiplyExpressionSymbol( ExpressionBaseSymbol<Ast.Expression> left, ExpressionBaseSymbol<Ast.Expression> right, Ast.Expression.Operator.Multiply symbolAst ) : base( left, right, symbolAst )
        {
        }
    }

    public class DivideExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Divide>
    {
        public DivideExpressionSymbol( ExpressionBaseSymbol<Ast.Expression> left, ExpressionBaseSymbol<Ast.Expression> right, Ast.Expression.Operator.Divide symbolAst ) : base( left, right, symbolAst )
        {
        }
    }

    public class AddExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Add>
    {
        public AddExpressionSymbol( ExpressionBaseSymbol<Ast.Expression> left, ExpressionBaseSymbol<Ast.Expression> right, Ast.Expression.Operator.Add symbolAst ) : base( left, right, symbolAst )
        {
        }
    }

    public class SubstractExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Substract>
    {
        public SubstractExpressionSymbol( ExpressionBaseSymbol<Ast.Expression> left, ExpressionBaseSymbol<Ast.Expression> right, Ast.Expression.Operator.Substract symbolAst ) : base( left, right, symbolAst )
        {
        }
    }
}
