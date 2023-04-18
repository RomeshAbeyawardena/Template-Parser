namespace TemplateParser.Extensions;

public static class DictionaryExtensions
{
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        TKey key, TValue value)
    {
        if (!dictionary.TryAdd(key, value))
        {
            dictionary[key] = value;
        }
    }
}
