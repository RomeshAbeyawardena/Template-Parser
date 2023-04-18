using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFilePathProcessor : DefaultFileTemplateProcessor
{
    public DefaultFilePathProcessor()
    {
        Type = TemplateType.FilePathTemplate;
    }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        return base.Process(template, cancellationToken);
    }
}
