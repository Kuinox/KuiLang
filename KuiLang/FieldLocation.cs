using System;
using System.Collections.Generic;

namespace KuiLang
{
    public class FieldLocation
    {
        readonly string[] _parts;

        public FieldLocation(string firstPart)
        {
            _parts = new string[] { firstPart };
        }

        public FieldLocation(string newPart, FieldLocation existing)
        {
            _parts = new string[existing._parts.Length + 1];
            existing._parts.CopyTo(_parts, 0);
            _parts[^1] = newPart;
        }
        public IReadOnlyCollection<string> Parts => _parts;
        public FieldLocation Append(string part) => new FieldLocation(part, this);

        public override string ToString() => string.Join(".", _parts);
    }
}