using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Diagnostics
{
    /// <summary>
    /// Represent a message emited by the compiler.
    /// </summary>
    public record Diagnostic(Severity Severity, string? ErrorCode, string Message, SourceLocation? Location)
    {
        public override string ToString() => $"{Location}: {Severity} {ErrorCode}: {Message}";

        public static Diagnostic FieldSingleStatement( Ast.Statement.Definition.Typed.Field ast )
           => new( Severity.Error, null, "A Variable Declaration cannot be used as an embedded statement.", null );
    }
}
