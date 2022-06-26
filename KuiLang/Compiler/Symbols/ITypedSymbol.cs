namespace KuiLang.Semantic
{
    public interface ITypedSymbol : ISymbol
    {
        public TypeSymbol Type { get; }
    }
}
