namespace Doco.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool IsEqualTo<TKey, TValue>(
            this IDictionary<TKey, TValue> first,
            IDictionary<TKey, TValue> second)
        {
            if (first.Count != second.Count) return false;

            foreach (var kvp in first)
            {
                if (!second.TryGetValue(kvp.Key, out var secondValue)) return false;
                if (!Equals(kvp.Value, secondValue)) return false;
            }

            return true;
        }
    }
}
