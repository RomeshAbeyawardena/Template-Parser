using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public abstract class BaseTemplateProcessor : ITemplateProcessor
{
    public abstract Task Process(ITemplate template, CancellationToken cancellationToken);

    public virtual bool CanProcess(ITemplate template)
    {
        return Type == template.Type;
    }

    public BaseTemplateProcessor(TemplateType type)
    {
        Type = type;
    }

    public TemplateType Type { get; protected set; }
}
