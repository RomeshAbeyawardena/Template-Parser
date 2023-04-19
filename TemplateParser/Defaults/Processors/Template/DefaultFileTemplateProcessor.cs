using Microsoft.Extensions.FileProviders;
using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFileTemplateProcessor : DefaultFilePathTemplateProcessor
{
    private IFileOperation? fileOperation;
    
    public DefaultFileTemplateProcessor()
    {
        
    }

    public override async Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        await base.Process(template, cancellationToken);

        if (!string.IsNullOrWhiteSpace(TargetDirectory) 
                && !string.IsNullOrWhiteSpace(template.FileName))
        {
            fileOperation ??= new DefaultPhysicalFileOperation(TargetDirectory, template.FileName);
            
            if (!fileOperation.Exists
                && !string.IsNullOrWhiteSpace(template.Content))
            {
                await fileOperation.Create(cancellationToken);
            }
        }
    }
}
