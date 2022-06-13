using OneOf;
using System.Diagnostics.CodeAnalysis;

namespace KuiLang.Diagnostics
{
    /// <summary>
    /// Simple specialisation of <see cref="OneOfBase{,}"/>  to allow
    /// </summary>
    public partial class SourceOrSourcePath
    {
        [MemberNotNullWhen( true, nameof( Source ) )]
        [MemberNotNullWhen( false, nameof( SourcePath ) )]

        public bool IsRawSource { get; }
        public string? Source { get; }
        public string? SourcePath { get; }

        public SourceOrSourcePath( bool isRawSource, string str )
        {
            IsRawSource = isRawSource;
            if( isRawSource )
            {
                Source = str;
            }
            else
            {
                Source = str;
            }
        }
    }
}
