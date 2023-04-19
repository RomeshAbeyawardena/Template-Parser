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
        string fullyQualifiedPath;
        if (!string.IsNullOrWhiteSpace(TargetDirectory) 
            && !string.IsNullOrWhiteSpace(template.Path) 
            && !Directory.Exists(fullyQualifiedPath = Path.Combine(template.Path)))
        {
            Directory.CreateDirectory(fullyQualifiedPath);
        }

        return base.Process(template, cancellationToken);
    }
}
