using System;
using System.Collections.Generic;

namespace KuiLang
{
    public class Identifier
    {
        public Identifier(string firstPart)
        {
            Parts = new string[] { firstPart };
        }

        Identifier(ReadOnlyMemory<string> parts)
        {
            if( parts.IsEmpty ) throw new Wat();
            Parts = parts;
        }

        public ReadOnlyMemory<string> Parts { get; }
        public Identifier Prepend(string partToAppend)
        {
            Memory<string> parts = new string[Parts.Length + 1];
            Parts.CopyTo(parts[1..]);
            parts.Span[0] = partToAppend;
            return new(parts);
        }

        public Identifier Append(string part)
        {
            Memory<string> parts = new string[Parts.Length + 1];
            Parts.CopyTo(parts);
            parts.Span[^1] = part;
            return new(parts);
        }

        public Identifier SubParts => new(Parts[1..]);
        public Identifier ParentLocation => new( Parts[..1] );
        public string Name => Parts.Span[^1];
        public override string ToString() => string.Join(".", Parts.ToArray());

        public bool Empty => Parts.Length == 0;

    }
}
