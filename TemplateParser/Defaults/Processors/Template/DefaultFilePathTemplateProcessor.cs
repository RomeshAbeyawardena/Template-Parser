using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFilePathTemplateProcessor : DefaultFileTemplateProcessor
{
    public DefaultFilePathTemplateProcessor()
    {
        Type = TemplateType.FilePathTemplate;
    }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        return base.Process(template, cancellationToken);
    }
}
