using System.Collections;
using System.Collections.Concurrent;
using TemplateParser.Contracts;

namespace TemplateParser.Defaults;

public class DefaultGlobalVariables : IGlobalVariables
{
    private string GetAndReplaceKeyValue(string key)
    {
        if (globalVariables.TryGetValue(key, out string? value))
        {
            value = ReplaceWithVariables(value);
        }

        return value ?? string.Empty;
    }

    private readonly ConcurrentDictionary<string, string> globalVariables;
    private readonly string? prependedString;

    public DefaultGlobalVariables(string? prependedString = null)
        : this(new ConcurrentDictionary<string, string>(), prependedString)
    {

    }

    public DefaultGlobalVariables(IEnumerable<KeyValuePair<string, string>> dictionary,
        string? prependedString = null)
    {
        globalVariables = new ConcurrentDictionary<string, string>(dictionary);
        this.prependedString = prependedString;
    }

    public string? this[string key]
    {
        get => GetAndReplaceKeyValue(key);
        set {
            if (string.IsNullOrWhiteSpace(value))
            {
                globalVariables.TryRemove(key, out _);
                return;
            }

            if (globalVariables.TryAdd(key, value))
            {
                globalVariables[key] = value;
            }
        }
    }

    string IReadOnlyDictionary<string, string>.this[string key] { 
        get => GetAndReplaceKeyValue(key) ?? throw new KeyNotFoundException(); 
    }

    IEnumerable<string> IReadOnlyDictionary<string, string>.Keys => globalVariables.Keys;
    IEnumerable<string> IReadOnlyDictionary<string, string>.Values => globalVariables.Values;
    int IReadOnlyCollection<KeyValuePair<string, string>>.Count => globalVariables.Count;

    bool IReadOnlyDictionary<string, string>.ContainsKey(string key)
    {
        return globalVariables.ContainsKey(key);
    }

    IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
    {
        return globalVariables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return globalVariables.GetEnumerator();
    }

    bool IReadOnlyDictionary<string, string>.TryGetValue(string key, out string value)
    {
        value = GetAndReplaceKeyValue(key)!;
        return !string.IsNullOrWhiteSpace(value);
    }

    public string ReplaceWithVariables(string value)
    {
        foreach (var (k, v) in globalVariables)
        {
            value = value.Replace(
                !string.IsNullOrWhiteSpace(prependedString)
                ? $"{prependedString}{k}"
                : k, v);
        }

        return value;
    }
}
