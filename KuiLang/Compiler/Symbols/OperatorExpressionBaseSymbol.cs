using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public abstract class OperatorExpressionBaseSymbol<T> : IExpressionSymbol<T> where T : Ast.Expression.Operator
    {
        public OperatorExpressionBaseSymbol( IExpressionSymbol<Ast.Expression> left, IExpressionSymbol<Ast.Expression> right, T symbolAst ) : base( symbolAst )
        {
            Left = left;
            Right = right;
        }

        public IExpressionSymbol<Ast.Expression> Left { get; }
        public IExpressionSymbol<Ast.Expression> Right { get; }
    }

    public class MultiplyExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Multiply>
    {
        public MultiplyExpressionSymbol( IExpressionSymbol<Ast.Expression> left, IExpressionSymbol<Ast.Expression> right, Ast.Expression.Operator.Multiply symbolAst ) : base( left, right, symbolAst )
        {
        }
    }

    public class DivideExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Divide>
    {
        public DivideExpressionSymbol( IExpressionSymbol<Ast.Expression> left, IExpressionSymbol<Ast.Expression> right, Ast.Expression.Operator.Divide symbolAst ) : base( left, right, symbolAst )
        {
        }
    }

    public class AddExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Add>
    {
        public AddExpressionSymbol( IExpressionSymbol<Ast.Expression> left, IExpressionSymbol<Ast.Expression> right, Ast.Expression.Operator.Add symbolAst ) : base( left, right, symbolAst )
        {
        }
    }

    public class SubstractExpressionSymbol : OperatorExpressionBaseSymbol<Ast.Expression.Operator.Substract>
    {
        public SubstractExpressionSymbol( IExpressionSymbol<Ast.Expression> left, IExpressionSymbol<Ast.Expression> right, Ast.Expression.Operator.Substract symbolAst ) : base( left, right, symbolAst )
        {
        }
    }
}
