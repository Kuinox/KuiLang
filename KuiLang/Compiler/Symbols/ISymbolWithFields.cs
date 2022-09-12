namespace KuiLang.Semantic
{
    public interface ISymbolWithFields : ISymbol
    {
        OrderedDictionary<string, FieldSymbol> Fields { get; }
    }
}
