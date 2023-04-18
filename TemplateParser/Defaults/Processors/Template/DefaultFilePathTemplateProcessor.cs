using Microsoft.Extensions.FileProviders;
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
        if (!string.IsNullOrWhiteSpace(template.Path) 
            && !Directory.Exists(template.Path))
        {
            Directory.CreateDirectory(template.Path);
        }
        return base.Process(template, cancellationToken);
    }
}
