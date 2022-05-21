using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Diagnostics
{
    public class DiagnosticChannel
    {
        readonly List<Diagnostic> _diagnostics = new();
        public void EmitDiagnostic( Diagnostic diagnostic ) => _diagnostics.Add( diagnostic );
    }
}
