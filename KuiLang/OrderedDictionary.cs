using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang
{
    public class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
    {
        int _nextId = 0;
        readonly Dictionary<TKey, (int, TValue)> _dic = new();

        public ICollection<TKey> Keys => _dic.Keys;

        public ICollection<TValue> Values => _dic.Values.Select( s => s.Item2 ).ToArray();

        public int Count => _dic.Count;

        public bool IsReadOnly => false;

        public TValue this[TKey key] { get => _dic[key].Item2; set => _dic[key] = (_dic[key].Item1, value); }

        public void Add( TKey key, TValue value )
            => _dic.Add( key, (_nextId++, value) );

        public bool ContainsKey( TKey key ) => _dic.ContainsKey( key );

        public bool Remove( TKey key )
        {
            return _dic.Remove( key );
        }

        public bool TryGetValue( TKey key, [MaybeNullWhen( false )] out TValue value )
        {
            var res = _dic.TryGetValue( key, out var t );
            value = t.Item2;
            return res;
        }

        public void Add( KeyValuePair<TKey, TValue> item ) => Add( item.Key, item.Value );

        public void Clear()
        {
            _dic.Clear();
            _nextId = 0;
        }

        public bool Contains( KeyValuePair<TKey, TValue> item )
        {
            if( !_dic.TryGetValue( item.Key, out var val ) ) return false;
            return item.Value?.Equals( val.Item2 ) ?? val.Item2 == null;
        }

        public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
        {
            throw new NotImplementedException( "TODO" );
        }

        public bool Remove( KeyValuePair<TKey, TValue> item )
        {
            throw new NotImplementedException( "TODO" );
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => _dic.OrderBy( s => s.Value.Item1 )
            .Select( s => new KeyValuePair<TKey, TValue>( s.Key, s.Value.Item2 ) )
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
