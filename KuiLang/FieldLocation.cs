using System;
using System.Collections.Generic;

namespace KuiLang
{
    public class FieldLocation
    {

        public FieldLocation() => Parts = Array.Empty<string>();

        public FieldLocation(string firstPart)
        {
            Parts = new string[] { firstPart };
        }

        FieldLocation(ReadOnlyMemory<string> parts)
        {
            Parts = parts;
        }

        public ReadOnlyMemory<string> Parts { get; }
        public FieldLocation Prepend(string partToAppend)
        {
            Memory<string> parts = new string[Parts.Length + 1];
            Parts.CopyTo(parts[1..]);
            parts.Span[0] = partToAppend;
            return new(parts);
        }

        public FieldLocation Append(string part)
        {
            Memory<string> parts = new string[Parts.Length + 1];
            Parts.CopyTo(parts);
            parts.Span[^1] = part;
            return new(parts);
        }

        public FieldLocation SubParts => new(Parts[1..]);

        public override string ToString() => string.Join(".", Parts);

        public bool Empty => Parts.Length == 0;

    }
}