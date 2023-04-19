using Microsoft.Extensions.FileProviders;
using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFileTemplateProcessor : DefaultFilePathTemplateProcessor
{
    private IFileProvider? fileProvider;

    protected override void Dispose()
    {
        (fileProvider as IDisposable)?.Dispose();
        base.Dispose();
    }

    public DefaultFileTemplateProcessor()
    {
        
    }

    public override async Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        await base.Process(template, cancellationToken);

        if (!string.IsNullOrWhiteSpace(TargetDirectory) 
                && !string.IsNullOrWhiteSpace(template.FileName))
        {
            fileProvider = new PhysicalFileProvider(TargetDirectory);
            var path = Path.Combine(TargetDirectory, template.FileName);
            if (!fileProvider.GetFileInfo(template.FileName).Exists
                && !string.IsNullOrWhiteSpace(template.Content))
            {
                await File.WriteAllTextAsync(path, template.Content, cancellationToken);
            }
        }
    }
}
