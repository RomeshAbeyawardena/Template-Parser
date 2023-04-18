
using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class InMemoryTemplate : BaseTemplateProcessor
{
    public InMemoryTemplate() : base(TemplateType.InMemoryTemplate)
    {
    }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
