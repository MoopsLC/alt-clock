#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prod.Data
{
    public class DefaultDict<K, V> : IDictionary<K, V> where V : struct 
    {
        private V defaultValue;
        private IDictionary<K,V> dict = new Dictionary<K,V>();

        public DefaultDict(IDictionary<K,V> entries, V defaultValue)
        {
            this.dict = new Dictionary<K, V>(entries);
            this.defaultValue = defaultValue;
        }

        public DefaultDict(V defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public DefaultDict()
        {
            
        }

        public V this[K key]
        {
            get
            {
                if (dict.ContainsKey(key))
                {
                    return dict[key];
                }
                return defaultValue;
            }
            set
            {
                if (!dict.ContainsKey(key))
                {
                    dict[key] = defaultValue;
                }
                dict[key] = value;
            }
        }



        #region IDictionary<K,V> Members

        public void Add(K key, V value)
        {
            this[key] = value;
        }

        public bool ContainsKey(K key)
        {
            return dict.ContainsKey(key);
        }

        public ICollection<K> Keys
        {
            get { return dict.Keys; }
        }

        public bool Remove(K key)
        {
            return dict.Remove(key);
        }

        public bool TryGetValue(K key, out V value)
        {
            return dict.TryGetValue(key, out value);
        }

        public ICollection<V> Values
        {
            get { return dict.Values; }
        }

        #endregion

        #region ICollection<KeyValuePair<K,V>> Members

        public void Add(KeyValuePair<K, V> item)
        {
            dict.Add(item);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return dict.Contains(item);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            dict.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return dict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return dict.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<K,V>> Members

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        #endregion
    }
}
