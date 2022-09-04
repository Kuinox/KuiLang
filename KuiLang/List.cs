namespace KuiLang
{
    public class MyList<T> : System.Collections.Generic.List<T>
    {
        public override string ToString() => $"[{string.Join( ',', this )}]";
    }
}
