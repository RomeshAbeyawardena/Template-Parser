using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFilePathTemplateProcessor : BaseTemplateProcessor
{
    private string? directory;

    protected string? TargetDirectory => directory;

    protected override void OnGlobalVariablesUpdated(IDictionary<string, string>? globalVariables)
    {
        directory = Environment.CurrentDirectory;

        if (globalVariables != null
            && globalVariables.TryGetValue(CommandVariables.COMMAND_BASE_PATH, out var basePath))
        {
            directory = basePath;
        }

        base.OnGlobalVariablesUpdated(globalVariables);
    }

    public DefaultFilePathTemplateProcessor()
        : base(TemplateType.FilePathTemplate)
    {

    }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        string fullyQualifiedPath;
        if (!string.IsNullOrWhiteSpace(TargetDirectory) 
            && !string.IsNullOrWhiteSpace(template.Path) 
            && !Directory.Exists(fullyQualifiedPath = Path.Combine(template.Path)))
        {
            Directory.CreateDirectory(fullyQualifiedPath);
            directory = fullyQualifiedPath;
        }

        return Task.CompletedTask;
    }
}
