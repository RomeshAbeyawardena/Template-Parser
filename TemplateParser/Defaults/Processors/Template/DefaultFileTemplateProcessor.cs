using Microsoft.Extensions.FileProviders;
using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFileTemplateProcessor : BaseTemplateProcessor
{
    private IFileProvider? fileProvider;
    private string? directory;

    protected string? TargetDirectory => directory;

    public override void OnGlobalVariablesUpdated(IDictionary<string, string>? globalVariables)
    {
        directory = Environment.CurrentDirectory;

        if (globalVariables != null
            && globalVariables.TryGetValue(CommandVariables.COMMAND_BASE_PATH, out var basePath))
        {
            directory = basePath;
        }

        fileProvider = new PhysicalFileProvider(directory);

        base.OnGlobalVariablesUpdated(globalVariables);
    }

    public DefaultFileTemplateProcessor() : base(TemplateType.FileTemplate)
    {
        
    }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        if (fileProvider !=null && !string.IsNullOrWhiteSpace(directory) 
                && !string.IsNullOrWhiteSpace(template.FileName))
        {
            var path = Path.Combine(directory, template.FileName);
            if (!fileProvider.GetFileInfo(template.FileName).Exists
                && !string.IsNullOrWhiteSpace(template.Content))
            {
                return File.WriteAllTextAsync(path, template.Content, cancellationToken);
            }
        }

        return Task.CompletedTask;
    }
}
