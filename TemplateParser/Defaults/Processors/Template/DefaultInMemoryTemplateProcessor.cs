
using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultInMemoryTemplateProcessor : BaseTemplateProcessor
{
    public DefaultInMemoryTemplateProcessor() : base(TemplateType.InMemoryTemplate)
    {
    }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
