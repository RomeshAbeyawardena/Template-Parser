using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultFilePathTemplateProcessor : BaseTemplateProcessor
{
    private string? directory;
    private IDirectoryOperation? directoryOperation;
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

    public IDirectoryOperation DirectoryOperation { set => directoryOperation = value; }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        
        if (!string.IsNullOrWhiteSpace(TargetDirectory) 
            && !string.IsNullOrWhiteSpace(template.Path))
        {
            directoryOperation ??= new DefaultPhysicalDirectoryOperation(Path.Combine(template.Path));
            
            if (!directoryOperation.Exists)
            {
                directoryOperation.Create(cancellationToken);
            }
            directory = directoryOperation.FullName;
        }

        return Task.CompletedTask;
    }
}
