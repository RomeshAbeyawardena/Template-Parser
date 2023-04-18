using Microsoft.Extensions.FileProviders;
using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFileTemplateProcessor : BaseTemplateProcessor
{
    private readonly IFileProvider fileProvider;
    private readonly string directory;
    public DefaultFileTemplateProcessor() : base(TemplateType.FileTemplate)
    {
        directory = Environment.CurrentDirectory;

        if (this.GlobalVariables != null
            && this.GlobalVariables.TryGetValue(CommandVariables.COMMAND_BASE_PATH, out var basePath))
        {
            directory = basePath;
        }

        fileProvider = new PhysicalFileProvider(directory);
    }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(template.FileName))
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
