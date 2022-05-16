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
    public record Diagnostic(Severity Severity, string ErrorCode, string Message, SourceLocation Location)
    {
        public override string ToString() => $"{Location}: {Severity} {ErrorCode}: {Message}";
    }
}
