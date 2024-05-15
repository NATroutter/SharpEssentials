using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class Pair<K,V> {

        public K first;
        public V second;

        public Pair(K first, V second) {
            this.first = first;
            this.second = second;
        }

        public K GetFirst() {
            return first;
        }

        public V GetSecond() {
            return second;
        }

    }
}
