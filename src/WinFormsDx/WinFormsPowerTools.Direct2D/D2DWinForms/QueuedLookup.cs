using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Forms.Direct2D
{
    internal class QueuedLookup<T>
    {
        private int _cacheSize;
        private Dictionary<int, T> _lookUp;

        public QueuedLookup(int cacheSize)
        {
            _cacheSize = cacheSize;
            _lookUp = new(cacheSize);
        }

        public int Add<U>(U keyObject, T objectToCache)
        {
            CheckCapacity();
            int hash = HashCode.Combine(keyObject);
            _lookUp.Add(hash, objectToCache);

            return hash;
        }

        public int Add<U1, U2>(U1 keyObject1, U2 keyObject2, T objectToCache)
        {
            CheckCapacity();
            int hash = HashCode.Combine(keyObject1, keyObject2);
            _lookUp.Add(hash, objectToCache);

            return hash;
        }

        public int Add<U1, U2, U3>(U1 keyObject1, U2 keyObject2, U3 keyObject3, T objectToCache)
        {
            CheckCapacity();
            int hash = HashCode.Combine(keyObject1, keyObject2, keyObject3);
            _lookUp.Add(hash, objectToCache);
            return hash;
        }

        private void CheckCapacity()
        {
            if (_lookUp.Count == _cacheSize &&
                _lookUp.Keys.FirstOrDefault() is int firstKey)
            {
                _lookUp.Remove(firstKey);
            }
        }

        public bool TryGetValue<U>(U keyObject, out T? obj)
        {
            int hash = HashCode.Combine(keyObject);

            return _lookUp.TryGetValue(hash, out obj);
        }

        public bool TryGetValue<U1, U2>(U1 keyObject1, U2 keyObject2, out T? obj)
        {
            int hash = HashCode.Combine(keyObject1, keyObject2);

            return _lookUp.TryGetValue(hash, out obj);
        }

        public bool TryGetValue<U1, U2, U3>(U1 keyObject1, U2 keyObject2, U3 keyObject3, out T? obj)
        {
            int hash = HashCode.Combine(keyObject1, keyObject2, keyObject3);

            return _lookUp.TryGetValue(hash, out obj);
        }
    }
}
