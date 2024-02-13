using System.Collections.Generic;

namespace System.Windows.Forms.Direct2D
{
    internal class WeakCache<T, U> where T : class
    {
        private Queue<WeakKeyValuePair<T, U>> _internalList;
        private int _maxElements;

        public WeakCache(int maxElements)
        {
            _maxElements = maxElements;
            _internalList = new Queue<WeakKeyValuePair<T, U>>(maxElements);
        }

        public bool TryGetValue(T key, out U? value)
        {
            var enumerator = _internalList.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var lookupKey = enumerator.Current;
                if (Equals(key, lookupKey.Key))
                {
                    value = lookupKey.Value;
                    return true;
                }
            }

            value = default(U);

            return false;
        }

        /// <summary>
        /// Adds a Key/Value pair to the cache, where key is a weak reference. 
        /// If the cache overflows, the value to overflow is returned, so it can be disposed.
        /// </summary>
        /// <param name="key">Lookup key, which is stored as weak reference.</param>
        /// <param name="value">Value which is assigned to the key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public U? Cache(T key, U value)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            U? returnValue = default(U);

            if (_internalList.Count == _maxElements)
            {
                returnValue = _internalList.Dequeue().Value;
            }

            _internalList.Enqueue(new WeakKeyValuePair<T, U>(key, value));

            return returnValue;
        }

        public void ClearCache(Action<U> disposeAction)
        {
            while (_internalList.Count > 0)
            {
                var keyValue = _internalList.Dequeue();

                if (keyValue.Value is { } value)
                {
                    disposeAction?.Invoke(value);
                }
            }
        }
    }
}
