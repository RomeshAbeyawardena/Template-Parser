namespace TemplateParser.Contracts;

public interface ITemplateProcessor : IDisposable
{
    IGlobalVariables? GlobalVariables { get; internal set; }
    TemplateType Type { get; }
    int OrderIndex { get; }
    bool CanProcess(ITemplate template);
    Task Process(ITemplate template, CancellationToken cancellationToken);
}
