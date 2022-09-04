using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KuiLang.Diagnostics
{
    public class DiagnosticChannel
    {
        readonly MyList<Diagnostic> _diagnostics = new();
        public void EmitDiagnostic( Diagnostic diagnostic ) => _diagnostics.Add( diagnostic );

        public bool HaveError => _diagnostics.Any( s => s.Severity >= Severity.Error );

        public void PrintDiagnostics( TextWriter writer )
        {
            foreach( var item in _diagnostics )
            {
                writer.WriteLine( item.ToString() );
            }
        }
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

        public static void CompilerError( this DiagnosticChannel @this, string message )
            => @this.EmitDiagnostic( new Diagnostic(
            Severity.Error,
            null,
            message,
            null
        ) );

        public static void CompilerErrorIfTrue( this DiagnosticChannel @this, bool emit, [CallerArgumentExpression( "emit" )] string? message = null )
        {
            if(emit)
            {
                @this.CompilerError( message! );
            }
        }
    }
}
