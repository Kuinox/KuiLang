using System;
using System.Collections.Generic;

namespace KuiLang
{
    public class FullName
    {
        readonly string[] _parts;

        public FullName(string firstPart)
        {
            _parts = new string[] { firstPart };
        }

        public FullName(string newPart, FullName existing)
        {
            _parts = new string[existing._parts.Length + 1];
            existing._parts.CopyTo(_parts, 0);
            _parts[^1] = newPart;
        }
        public IReadOnlyCollection<string> Parts => _parts;
        public FullName Append(string part) => new(part, this);

        public override string ToString() => string.Join(".", _parts);
    }
}