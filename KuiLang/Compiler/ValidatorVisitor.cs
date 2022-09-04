using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler
{
    class ValidatorVisitor : SymbolVisitor<object>
    {
        readonly DiagnosticChannel _diagnostics;

        public ValidatorVisitor( DiagnosticChannel diagnostics )
        {
            _diagnostics = diagnostics;
        }

        protected override object Visit( IExpressionSymbol symbol )
        {
            _diagnostics.CompilerErrorIfTrue( symbol.ReturnType is null );
            return base.Visit( symbol );
        }

        protected override object Visit( MethodCallExpressionSymbol symbol )
        {
            _diagnostics.CompilerErrorIfTrue( symbol.CallTarget is null );
            _diagnostics.CompilerErrorIfTrue( symbol.TargetMethod is null );
            return base.Visit( symbol );
        }
    }
}
