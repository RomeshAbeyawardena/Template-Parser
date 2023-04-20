using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFilePathTemplateProcessor : BaseTemplateProcessor
{
    private string? directory;
    private IDirectoryOperation? directoryOperation;
    protected string? GetBasePath(IGlobalVariables? globalVariables)
    {
        directory = Environment.CurrentDirectory;

        if (globalVariables != null
            && globalVariables.TryGetValue($"{CommandVariables.COMMAND_BASE_PATH}", out var basePath))
        {
            directory = basePath;
        }
        return directory;
    }

    protected string? TargetDirectory => directory;

    protected override void OnGlobalVariablesUpdated(IGlobalVariables? globalVariables)
    {
        GetBasePath(GlobalVariables);
        base.OnGlobalVariablesUpdated(globalVariables);
    }

    public DefaultFilePathTemplateProcessor()
        : base(TemplateType.FilePathTemplate, 20)
    {
        
    }

    public IDirectoryOperation DirectoryOperation { set => directoryOperation = value; }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        Console.WriteLine("Processing {0}", template.Type);
        if (GlobalVariables != null 
            && !string.IsNullOrWhiteSpace(directory) 
            && !string.IsNullOrWhiteSpace(template.Path))
        {
            directoryOperation = new DefaultPhysicalDirectoryOperation(Path.Combine(directory, GlobalVariables.ReplaceWithVariables(template.Path)));

            Console.WriteLine("{0}", directory);
            if (!directoryOperation.Exists)
            {
                directoryOperation.Create(cancellationToken);
            }
            directory = directoryOperation.FullName;
        }

        return Task.CompletedTask;
    }
}
