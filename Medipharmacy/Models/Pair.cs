using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Pair<K,V>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public Pair(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public Pair()
        {
        }
    }
}
