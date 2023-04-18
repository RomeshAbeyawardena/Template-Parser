using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFileTemplateProcessor : BaseTemplateProcessor
{
    public DefaultFileTemplateProcessor() : base(TemplateType.FileTemplate)
    {
    }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
