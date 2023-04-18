namespace TemplateParser.Contracts;

public interface IConsoleConfig
{
    string? Input { get; }
    string? KeyValues { get; }
    string? OutputFiles { get; }
    string? KeyValueSeparator { get; }
    bool? Test { get; }
    IDictionary<string, string> ValueDictionary { get; }
}
