using OneOf;

namespace KuiLang.Diagnostics
{
    /// <summary>
    /// Simple specialisation of <see cref="OneOfBase{,}"/>  to allow
    /// </summary>
    [GenerateOneOf]
    public partial class SourceOrSourcePath : OneOfBase<string, string>
    {
        public string Source => AsT0;
        public string SourcePath => AsT1;

        public static SourceOrSourcePath FromSourcePath(string path)
            => new(OneOf<string, string>.FromT1(path));

        public static SourceOrSourcePath FromSource(string source)
            => new(OneOf<string, string>.FromT0(source));
    }
}
