using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public abstract class BaseTemplateProcessor : ITemplateProcessor
{
    private IDictionary<string, string>? globalVariables;
    public abstract Task Process(ITemplate template, CancellationToken cancellationToken);

    protected virtual void Dispose()
    {

    }

    void IDisposable.Dispose()
    {
        Dispose();
        GC.SuppressFinalize(this);
    }

    protected virtual void OnGlobalVariablesUpdated(IDictionary<string, string>? globalVariables)
    {
        this.globalVariables = globalVariables;
    }

    public virtual bool CanProcess(ITemplate template)
    {
        return Type.HasFlag(template.Type);
    }

    public BaseTemplateProcessor(TemplateType type)
    {
        Type = type;
    }

    public TemplateType Type { get; protected set; }
    public IDictionary<string, string>? GlobalVariables { get => globalVariables; set => OnGlobalVariablesUpdated(value); }
    TemplateType ITemplateProcessor.Type { get; }
}
