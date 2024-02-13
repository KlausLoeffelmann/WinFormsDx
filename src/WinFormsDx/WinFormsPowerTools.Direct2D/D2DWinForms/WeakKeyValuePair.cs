namespace System.Windows.Forms.Direct2D
{
    internal class WeakKeyValuePair<T, U> where T : class
    {
        private readonly WeakReference<T> _key;
        private readonly U _value;

        public WeakKeyValuePair(T key, U value)
        {
            _key = new WeakReference<T>(key);
            _value = value;
        }

        public T? Key
        {
            get
            {
                if (_key.TryGetTarget(out var t))
                {
                    return t;
                }

                return null;
            }
        }

        public U? Value => _value;
    }
}
