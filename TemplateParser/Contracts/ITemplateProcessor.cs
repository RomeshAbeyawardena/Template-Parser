namespace TemplateParser.Contracts;

public interface ITemplateProcessor
{
    IDictionary<string, string>? GlobalVariables { get; internal set; }
    TemplateType Type { get; }
    bool CanProcess(ITemplate template);
    Task Process(ITemplate template, CancellationToken cancellationToken);
}
