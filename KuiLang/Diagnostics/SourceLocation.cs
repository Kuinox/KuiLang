using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Diagnostics
{
    /// <summary>
    /// Purpose of this class is to be a pointer to a chunk of code,
    /// which can be a file, or a string directly given to the compiler.
    /// </summary>
    public record SourceLocation(
        SourceOrSourcePath SourceOrSourcePath,
        int StartLine,
        int StartColumn,
        int EndLine,
        int EndColumn
    )
    {
        public override string ToString()
            => $"{SourceOrSourcePath}({StartLine},{StartColumn},{EndLine},{EndColumn})";
    }
}
