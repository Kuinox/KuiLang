using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KuiLang.Diagnostics
{
    public class DiagnosticChannel
    {
        readonly List<Diagnostic> _diagnostics = new();
        public void EmitDiagnostic( Diagnostic diagnostic ) => _diagnostics.Add( diagnostic );
    }

    public static class DiagnosticsExtensions
    {
        public static void Error( this DiagnosticChannel @this, string message )
            => @this.EmitDiagnostic( new Diagnostic(
            Severity.Error,
            null,
            message,
            null
        ) );
    }
}
