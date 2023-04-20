namespace TemplateParser.Contracts;

public interface IGlobalVariables : IReadOnlyDictionary<string, string>
{
    new string? this[string key] { get; set; }
    string ReplaceWithVariables(string value);
}
