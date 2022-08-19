using KuiLang.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class HardcodedExpressionsSymbol : IExpression, ISymbol
    {
        public HardcodedExpressionsSymbol( ISymbol parent, TypeSymbol returnType)
        {
            Parent = parent;
            ReturnType = returnType;
        }
        public TypeSymbol ReturnType { get; }

        public ISymbol Parent { get; }


        public class NumberAddSymbol : HardcodedExpressionsSymbol
        {
            public NumberAddSymbol( ISymbol parent, TypeSymbol returnType ) : base( parent, returnType )
            {
            }
        }

        public class NumberSubstractSymbol : HardcodedExpressionsSymbol
        {
            public NumberSubstractSymbol( ISymbol parent, TypeSymbol returnType ) : base( parent, returnType )
            {
            }
        }

        public class NumberMultiplySymbol : HardcodedExpressionsSymbol
        {
            public NumberMultiplySymbol( ISymbol parent, TypeSymbol returnType ) : base( parent, returnType )
            {
            }
        }

        public class NumberDivideSymbol : HardcodedExpressionsSymbol
        {
            public NumberDivideSymbol( ISymbol parent, TypeSymbol returnType ) : base( parent, returnType )
            {
            }
        }
    }
}
