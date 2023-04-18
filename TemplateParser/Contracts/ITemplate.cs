namespace TemplateParser.Contracts;

public interface ITemplate
{
    IDictionary<string, string> Variables { get; }
    string? UseTemplateName { get; }
    string? TemplateName { get; }
    string? Path { get; }
    string? FileName { get;}
    string? Content { get; }
    TemplateType Type { get; }
}
