namespace TemplateParser.Contracts;

public interface ITemplate
{
    IGlobalVariables Variables { get; }
    IList<string> UsedTemplates { get; set; }
    string? TemplateName { get; }
    string? Path { get; }
    string? FileName { get;}
    string? Content { get; }
    TemplateType? Type { get; }
}
