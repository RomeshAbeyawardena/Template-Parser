namespace TemplateParser.Contracts;

public interface ITemplateProcessor
{
    TemplateType Type { get; }
    bool CanProcess(ITemplate template);
    Task Process(ITemplate template, CancellationToken cancellationToken);
}
