using TemplateParser.Contracts;

namespace TemplateParser.Defaults.Processors.Template;

public class DefaultOutputToConsoleTemplateProcessor : BaseTemplateProcessor
{
    public DefaultOutputToConsoleTemplateProcessor() : base(
        TemplateType.InMemoryTemplate 
        | TemplateType.FileTemplate 
        | TemplateType.FilePathTemplate)
    {
    }

    public override Task Process(ITemplate template, CancellationToken cancellationToken)
    {
        Console.WriteLine(template);
        return Task.CompletedTask;
    }
}
